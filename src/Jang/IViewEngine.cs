// -----------------------------------------------------------------------
// <copyright file="IViewEngine.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

namespace Jang
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Interface for defining ViewEngines
    /// </summary>
    public interface IViewEngine
    {
        /// <summary>
        /// Gets a list of locations to search for views
        /// </summary>
        string[] Locations { get; }

        /// <summary>
        /// Gets the extension for this ViewEngine
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Creates a javascript method to render the view on the page
        /// </summary>
        /// <param name="template">The name of the template to render</param>
        /// <param name="destination">The element to put the resulting template after</param>
        /// <returns>Javascript string to execute the render on the clientside</returns>
        string Render(string template, string destination);

        /// <summary>
        /// The rendering call to update the view with a model
        /// </summary>
        /// <returns>Returns a javascript return statement to generate the view</returns>
        string Renderer();

        /// <summary>
        /// Creates a template 
        /// </summary>
        /// <param name="template">The name of the template to render</param>
        /// <param name="context">ControllerContext of the executing method</param>
        /// <returns>Returns a Template that contains the </returns>
        Template GetTemplate(string template, ControllerContext context);

        /// <summary>
        /// Returns a list of templates for use in generating a view page
        /// </summary>
        /// <returns>IEnumerable&lt;Template&gt; of all the templates found within the Locations specified</returns>
        IEnumerable<Template> GetTemplates();
    }
}