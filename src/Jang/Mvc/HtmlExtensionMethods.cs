// -----------------------------------------------------------------------
// <copyright file="HtmlExtensionMethods.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

namespace Jang.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Adds extenion methods to the HtmlHelper in MVC.
    /// </summary>
    public static class HtmlExtensionMethods
    {
        /// <summary>
        /// Initializes static members of the HtmlExtensionMethods class.
        /// </summary>
        static HtmlExtensionMethods()
        {
            ViewEngine = new JazzViewEngine();
        }

        /// <summary>
        /// Gets a list of ViewEngines available
        /// </summary>
        public static Jang.IViewEngine ViewEngine
        {
            get;
            private set;
        }

        /// <summary>
        /// Method to create a jQuery call to render the template to the current location
        /// </summary>
        /// <typeparam name="T">Model of the Razor page</typeparam>
        /// <param name="helper">HtmlHelper&lt;T&gt; object to attach the extension method to</param>
        /// <param name="template">The name of the template to render</param>
        /// <param name="model">object to attach to the template for rendering</param>
        /// <returns>IHtmlString containing a &lt;script&gt; tag which executes javascript on the client to render the view.</returns>
        public static IHtmlString RenderTemplate<T>(this HtmlHelper<T> helper, string template, object model)
        {
            // this is used to reference the script and append the template after it
            // no need for document.write();
            List<string> locationsSearched = new List<string>();
                try
                {
                    StringBuilder sb = new StringBuilder();

                    Template view = ViewEngine.GetTemplate(template, helper.ViewContext.Controller.ControllerContext);

                    string tagId = 'j' + Guid.NewGuid().ToString("N");
                    var viewEngineTag = BuildScriptTag(tagId);

                    // document load function
                    sb.AppendLine("$(function() {");

                    sb.AppendLine(BuildModelString(model));
                    sb.AppendLine(ViewEngine.Render(view.Id, tagId));

                    sb.AppendLine("});");

                    viewEngineTag.InnerHtml = sb.ToString();

                    return new HtmlString(viewEngineTag.ToString());
                }
                catch (ViewNotFoundException viewNotFoundException)
                {
                    // view wasn't found - store this?
                    viewNotFoundException.Locations
                        .ToList()
                        .ForEach(locationsSearched.Add);
                }

            throw new ViewNotFoundException(locationsSearched.ToArray());
        }

        /// <summary>
        /// Creates a script tag for using in inserting after the resulting view.
        /// </summary>
        /// <param name="tagId">Contains the Id of the script tag to be used in inserting the view</param>
        /// <returns>TagBuilder instance of a script tag</returns>
        private static TagBuilder BuildScriptTag(string tagId)
        {
            TagBuilder viewEngineTag = new TagBuilder("script");
            viewEngineTag.Attributes.Add("type", "text/javascript");
            viewEngineTag.Attributes.Add("id", tagId);
            return viewEngineTag;
        }

        /// <summary>
        /// Converts the passed in model into a JSON encoded string
        /// </summary>
        /// <param name="model">The model to convert to JSON</param>
        /// <returns>Returns a string containing the JSON encoded model</returns>
        private static string BuildModelString(object model)
        {
            return string.Format("var model = {0};", Newtonsoft.Json.JsonConvert.SerializeObject(model));
        }
    }
}