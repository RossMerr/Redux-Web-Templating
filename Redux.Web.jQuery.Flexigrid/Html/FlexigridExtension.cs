﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Redux.Web.Html;
using Redux.Web.Templating;

namespace Redux.Web.jQuery.Flexigrid.Html
{
    public static class FlexigridExtension
    {
        public static IFlexigridConfiguration<TModel> Flexigrid<TModel>(this HtmlHelper htmlHelper, string target)
        {
            return new FlexigridConfiguration<TModel>(htmlHelper, target, null);
        }

        public static IFlexigridConfiguration<TModel> Flexigrid<TModel>(this HtmlHelper htmlHelper, string target, IEnumerable<TModel> collection,
            Action<IDataTableConfiguration<TModel>> columns)
        {
            return new FlexigridConfiguration<TModel>(htmlHelper, target, columns );
        }

        public static IFlexigridConfiguration<TModel> Flexigrid<TModel>(this HtmlHelper htmlHelper, IEnumerable<TModel> collection,
           Action<IDataTableConfiguration<TModel>> columns)
        {
            return htmlHelper.Flexigrid(collection, columns, new Dictionary<string, object>());
        }

        public static IFlexigridConfiguration<TModel> Flexigrid<TModel>(this HtmlHelper htmlHelper, IEnumerable<TModel> collection,
           Action<IDataTableConfiguration<TModel>> columns, object htmlAttributes)
        {
            return htmlHelper.Flexigrid(collection, columns,
                                 (IDictionary<string, object>) new RouteValueDictionary(htmlAttributes));
        }

        public static IFlexigridConfiguration<TModel> Flexigrid<TModel>(this HtmlHelper htmlHelper, IEnumerable<TModel> collection,
           Action<IDataTableConfiguration<TModel>> columns, IDictionary<string, object> htmlAttributes)
        {
            var id = GetId(htmlAttributes);
            
            htmlHelper.Table(collection, columns, htmlAttributes);

            return new FlexigridConfiguration<TModel>(htmlHelper, "#" + id, columns);
        }

        private static string GetId(IDictionary<string, object> attributes)
        {
            if (attributes == null) return string.Empty;

            if (!attributes.ContainsKey(Resources.Options.Id))
            {
                attributes.Add(Resources.Options.Id, Identifier.GenerateId());
            }

            return attributes[Resources.Options.Id].ToString();
        }
    }
}
