// -----------------------------------------------------------------------
// <copyright file="JangController.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

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
            StringBuilder sb = new StringBuilder();
            foreach (var templateFile in HtmlExtensionMethods.ViewEngine.GetTemplates())
            {
                string templateName = string.Format("{0}", templateFile.Id);

                sb.AppendFormat("<script type='text/template' id='{0}'>", templateName);
                sb.Append(System.IO.File.ReadAllText(templateFile.FullPath));
                sb.Append("</script>");
            }

            sb.AppendLine();
            sb.AppendLine("<script type='text/javascript'>jang.templatesDownloaded = true;</script>");

            return new ContentResult
            {
                Content = sb.ToString(),
                ContentType = "text/html"
            };
        }

        public ActionResult Engine()
        {
            StringBuilder sb = new StringBuilder();

            // read jang.js
            StringReader reader = new StringReader(GetTemplate());
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("{viewEngineRender}")) {
                    line = line.Replace("{viewEngineRender}", HtmlExtensionMethods.ViewEngine.Renderer());
                }
                sb.AppendLine(line);
            }

            return new ContentResult() {
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