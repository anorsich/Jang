// -----------------------------------------------------------------------
// <copyright file="JavascriptViewEngine.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

namespace Jang
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Base ViewEngine for javascript
    /// </summary>
    public abstract class JavascriptViewEngine : IViewEngine
    {

        /// <summary>
        /// Stores a Dictionary of templates called
        /// </summary>
        protected static readonly ConcurrentDictionary<string, string> Cache;

        /// <summary>
        /// Static constructor initializes cache for js templates
        /// </summary>
        static JavascriptViewEngine()
        {
            Cache = new ConcurrentDictionary<string, string>();
        }

        protected JavascriptViewEngine(params string[] locations)
        {
            var defaultLocations = new[] {
                "~/Templates" // default locations is root/Templates/{controller}
            };

            this.Locations = defaultLocations.Concat((!IsNullOrEmpty(locations)) ? locations : new string[] { }).ToArray();
        }

        /// <summary>
        /// Gets the locations to search for views
        /// </summary>
        public string[] Locations
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the extension for this view engine
        /// </summary>
        public abstract string Extension
        {
            get;
        }

        public virtual string Render(string template, string destination)
        {
            return string.Format("jang.render('{0}', '{1}', model);", template, destination);
        }

        private static bool IsNullOrEmpty(IEnumerable<string> source)
        {
            return source != null && source.Any();
        }

        public Template GetTemplate(string template, ControllerContext context)
        {
            string controllerName = context.RouteData.GetRequiredString("controller");
            string key = string.Format("{0}/{1}{2}", controllerName, template, this.Extension);
            var templatePath = Cache.GetOrAdd(
                key, 
                k => {
                string fullPath;

                if (TemplateExists(template, controllerName, out fullPath))
                {
                    return fullPath;
                }

                return null;
            });

            if (templatePath != null)
            {
                return new Template(templatePath, null);
            }

            throw new ViewNotFoundException(templatePath);
        }

        private bool TemplateExists(string template, string controllerName, out string fullPath)
        {
            fullPath = null;
            foreach (var path in this.Locations)
            {
                string sourcePath;
                if (template.StartsWith("~"))
                {
                    // it's an absolute path
                    sourcePath = HttpContext.Current.Server.MapPath(template);
                }
                else
                {
                    // relative path
                    sourcePath = HttpContext.Current.Server.MapPath(Path.Combine(path, controllerName, template + this.Extension));
                }
                if (PathExists(sourcePath))
                {
                    // we need just the location path in this case...
                    fullPath = Path.Combine(path, controllerName, template + this.Extension);
                    return true;
                }

                string sharedPath = HttpContext.Current.Server.MapPath(Path.Combine(path, "shared", template + this.Extension));
                if (PathExists(sharedPath))
                {
                    fullPath = Path.Combine(path, "shared", template + this.Extension);
                    return true;
                }
            }

            return false;
        }

        private static bool PathExists(string path)
        {
            return File.Exists(path);
        }

        public IEnumerable<Template> GetTemplates()
        {
            string root = HttpContext.Current.Server.MapPath("~/");

            foreach (var path in this.Locations)
            {
                string sourcePath = HttpContext.Current.Server.MapPath(string.Format(path));
                foreach (var filePath in Directory.GetFiles(sourcePath, "*" + this.Extension, SearchOption.AllDirectories))
                {
                    string relativePath = filePath.Replace(root, string.Empty);
                    yield return new Template(relativePath, filePath) {
                        ViewName = Path.GetFileNameWithoutExtension(new FileInfo(filePath).Name)
                    };
                }
            }
        }

        public abstract string Renderer();
    }
}
