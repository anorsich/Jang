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
    /// TODO: Update summary.
    /// </summary>
    public abstract class JavascriptViewEngine : IViewEngine
    {
        //File locations
        public string[] Locations { get; private set; }

        public abstract string Extension { get; }
        protected static readonly ConcurrentDictionary<string, string> Cache;

        static JavascriptViewEngine()
        {
            Cache = new ConcurrentDictionary<string, string>();
        }

        protected JavascriptViewEngine(params string[] locations)
        {
            var defaultLocations = new[] {
                "~/Templates" //default locations is root/Templates/{controller}
            };

            Locations = defaultLocations.Concat((!IsNullOrEmpty(locations)) ? locations : new string[] { }).ToArray();
        }

        public abstract string Render(string template, string destination);

        private static bool IsNullOrEmpty(IEnumerable<string> source)
        {
            return source != null && source.Any();
        }

        public Template GetTemplate(string template, ControllerContext context)
        {
            string controllerName = context.RouteData.GetRequiredString("controller");
            string key = string.Format("{0}/{1}{2}", controllerName, template, Extension);
            var templatePath = Cache.GetOrAdd(key, k =>
            {
                string fullPath;

                //template exists
                if (TemplateExists(template, controllerName, out fullPath))
                {
                    //javascriptify the full path into a valid id
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
            foreach (var path in Locations)
            {
                string sourcePath = HttpContext.Current.Server.MapPath(Path.Combine(path, controllerName, template + Extension));
                if (PathExists(sourcePath))
                {
                    //we need just the location path in this case...
                    fullPath = Path.Combine(path, controllerName, template + Extension);
                    return true;
                }

                string sharedPath = HttpContext.Current.Server.MapPath(Path.Combine(path, "shared", template + Extension));
                if (PathExists(sharedPath))
                {
                    fullPath = Path.Combine(path, "shared", template + Extension);
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

            foreach (var path in Locations)
            {
                string sourcePath = HttpContext.Current.Server.MapPath(string.Format(path));
                foreach (var filePath in Directory.GetFiles(sourcePath, "*" + Extension, SearchOption.AllDirectories))
                {
                    string relativePath = filePath.Replace(root, "");
                    yield return new Template(relativePath, filePath);
                }
            }
        }

        public abstract string Renderer();
    }
}
