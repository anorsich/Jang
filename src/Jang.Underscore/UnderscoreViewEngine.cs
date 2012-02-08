using System;
using System.Collections.Generic;
using System.Text;

namespace Jang.Underscore
{
    public class UnderscoreViewEngine : JavascriptViewEngine
    {
        public override string Extension
        {
            get { return ".underscore"; }
        }

        public override string Renderer()
        {
            return "return _.template($(\"#\" + template).html(), model);";
        }
    }
}
