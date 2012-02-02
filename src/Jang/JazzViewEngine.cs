// -----------------------------------------------------------------------
// <copyright file="JazzViewEngine.cs" company="Buildstarted">
// Copyright Ben Dornis 2012
// </copyright>
// -----------------------------------------------------------------------

namespace Jang
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class JazzViewEngine : JavascriptViewEngine
    {
        public override string Extension { get { return ".jazz"; } }

        public override string Render(string template, string destination)
        {
            //return string.Format("jang.render('{0}', '{1}', model, function(template, model) {{ return template.jazz(model); }});", template, destination);
            return string.Format("jang.render('{0}', '{1}', model);", template, destination);
        }

        public override string Renderer()
        {
            return "return $('#' + template).jazz(model);";
        }
    }
}