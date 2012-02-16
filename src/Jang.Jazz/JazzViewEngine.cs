// -----------------------------------------------------------------------
// <copyright file="JazzViewEngine.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

namespace Jang.Jazz
{
    /// <summary>
    /// Jazz View Engine for Jang
    /// </summary>
    public class JazzViewEngine : JavascriptViewEngine
    {
        /// <summary>
        /// Gets the extention assosciated with this view engine
        /// </summary>
        public override string Extension
        {
            get { return ".jazz"; }
        }

        /// <summary>
        /// The rendering call to update the view with a model
        /// </summary>
        /// <returns>Returns a javascript return statement to generate the view</returns>
        public override string Renderer()
        {
            return @"
                    model = model.Model; //required because of the way jazz works
                    return $('#' + template).jazz(model);";
        }
    }
}