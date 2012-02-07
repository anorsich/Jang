// -----------------------------------------------------------------------
// <copyright file="JsRenderViewEngine.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Jang.JsRender
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class JsRenderViewEngine : JavascriptViewEngine
    {
        public override string Extension
        {
            get { return ".jsrender"; }
        }

        public override string Renderer()
        {
            return "return $('#' + template).render(model);";
        }
    }
}
