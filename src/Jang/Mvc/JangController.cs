// -----------------------------------------------------------------------
// <copyright file="JangController.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Jang.Mvc
{
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Controller responsible for returning all the view engines for the site
    /// </summary>
    public class JangController : Controller
    {
        /// <summary>
        /// Builds the views into a single file
        /// </summary>
        /// <returns>text/html result of a compilation of all view engine views</returns>
        public ActionResult Views()
        {
            List<Tuple<string, string>> viewNames = new List<Tuple<string, string>>();
            StringBuilder sb = new StringBuilder();
            foreach (var templateFile in HtmlExtensionMethods.ViewEngine.GetTemplates())
            {
                viewNames.Add(new Tuple<string, string>(templateFile.Id, templateFile.ViewName));
                sb.AppendFormat("<script type='text/x-jquery-tmpl' id='{0}'>", templateFile.Id);
                sb.Append(System.IO.File.ReadAllText(templateFile.FullPath));
                sb.Append("</script>");
            }

            sb.AppendLine();
            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("jang.templatesDownloaded = true;");
            sb.AppendLine("jang.views = [");
            foreach (var view in viewNames) {
                sb.Append("{ 'view': '").Append(view.Item2).Append("',");
                sb.Append("'fullPath': '").Append(view.Item1).AppendLine("' },");
            }
            sb.Length -= 3;
            sb.AppendLine("];");
            sb.AppendLine("</script>");

            return new ContentResult
            {
                Content = sb.ToString(),
                ContentType = "text/html"
            };
        }

        public ActionResult Engine()
        {
            string jangViewsUrl = Url.RouteUrl(new {controller = "jang", action = "views"});
            
            StringBuilder sb = new StringBuilder();

            // read jang.js
            StringReader reader = new StringReader(GetTemplate());
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("{viewEngineRender}"))
                {
                    line = line.Replace("{viewEngineRender}", HtmlExtensionMethods.ViewEngine.Renderer());
                }

                if (line.Contains("{jangViewsUrl}")) {
                    line = line.Replace("{jangViewsUrl}", jangViewsUrl);
                }
                sb.AppendLine(line);
            }

            return new ContentResult()
            {
                Content = sb.ToString(),
                ContentType = "application/javascript"
            };
        }

        private static string GetTemplate()
        {
            using (Stream resourceStream = typeof(JangController).Assembly.GetManifestResourceStream("Jang.jang.js"))
            {
                using (var reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

    }
}