// -----------------------------------------------------------------------
// <copyright file="JangResult.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.Mvc;

namespace Jang.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class JangResult : JsonResult
    {
        private readonly object _model;
        private string _template;

        public JangResult(object model)
        {
            _model = model;
        }

        public JangResult(string template, object model)
            : this(model)
        {
            _template = template;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            // grab the template name from the action?
            // allow user to specify template?
            if (string.IsNullOrEmpty(_template))
            {
                //pull from the context
                _template = context.RouteData.GetRequiredString("action");
            }

            Template view = HtmlExtensionMethods.ViewEngine.GetTemplate(_template, context);
            this.Data = new
            {
                template = view.Id,
                model = _model
            };

            base.ExecuteResult(context);
        }
    }
}
