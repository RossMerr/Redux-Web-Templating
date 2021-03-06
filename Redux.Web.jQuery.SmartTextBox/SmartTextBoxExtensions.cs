﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Redux.Web.Html;
using Redux.Web.Templating;

namespace Redux.Web.jQuery.SmartTextBox
{
    public static class SmartTextBoxExtensions
    {
        public static ISmartTextBoxConfiguration<TModel> SmartTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            name = Identifier.GenerateId(name);

            htmlHelper.ViewContext.Writer.WriteLine(htmlHelper.TextBoxFor(expression).ToHtmlString());
            return new SmartTextBoxConfiguration<TModel>(htmlHelper, name);
        }

        public static string SmartTextBoxApi(this HtmlHelper htmlHelper)
        {
            var script = new TagBuilder("script");
            script.MergeAttribute("type", "text/javascript");
            script.MergeAttribute("src", "/SmartTextBox/Api");
            return script.ToString(TagRenderMode.Normal);            
        }

        public static string SmartTextBoxStyle(this HtmlHelper htmlHelper)
        {
            var script = new TagBuilder("link");
            script.MergeAttribute("type", "text/css");
            script.MergeAttribute("rel", "stylesheet");
            script.MergeAttribute("href", "/SmartTextBox/Style");
            return script.ToString(TagRenderMode.Normal);
        }
    }
}
