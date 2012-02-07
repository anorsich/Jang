using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jang.Mustache
{
    public class MustacheViewEngine : JavascriptViewEngine
    {
        public override string Extension
        {
            get { return ".mustache"; }
        }

        public override string Renderer()
        {
            return "return Mustache.render($(\"#\" + template).text(), model);";
        }
    }
}
