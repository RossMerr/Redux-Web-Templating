﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Redux.Web.jQuery.Flexigrid;
using Redux.Web.Templating;

namespace Redux.Web.Html
{
    public class TableWriter
    {
        public static void Writer<TModel>(HtmlHelper htmlHelper, IEnumerable<TModel> collection, Action<IDataTableConfiguration<TModel>> columns, IDictionary<string, object> htmlAttributes)
        {
            Writer(htmlHelper, collection, columns, htmlAttributes, true);
        }

        public static void Writer<TModel>(HtmlHelper htmlHelper, IEnumerable<TModel> collection, Action<IDataTableConfiguration<TModel>> columns, IDictionary<string, object> htmlAttributes, bool header)
        {
            var table = new TagBuilder("table");
            table.MergeAttributes(htmlAttributes);

            var sb = new StringBuilder();            

            if (columns != null)
            {
                var dataTable = new DataTable<TModel>();
                columns(dataTable);

                var dataTableColumns = dataTable.GetColumns();

                var delegates = new List<Delegate>();

                var tHead = new TagBuilder("thead");

                var sbHead = new StringBuilder();

                var hRow = new TagBuilder("tr");

                foreach (var column in dataTableColumns)
                {
                    var property = Property.GetExpressFromProperty(column.Property);
                    var dlgText = Property.CreatePropertyDelegate<TModel, object>(property);

                    if (dlgText == null) throw new NullReferenceException("Property not found");

                    delegates.Add(dlgText);

                    var th = new TagBuilder("th") {InnerHtml = property};
                    sbHead.AppendLine(th.ToString(TagRenderMode.Normal));
                }

                if (header)
                {
                    hRow.InnerHtml = sbHead.ToString();
                    tHead.InnerHtml = hRow.ToString(TagRenderMode.Normal);
                    table.InnerHtml = tHead.ToString(TagRenderMode.Normal);
                }

                var tBody = new TagBuilder("tbody");

                var sbBody = new StringBuilder();

                foreach (var row in collection)
                {
                    var bRow = new TagBuilder("tr");

                    var sbData = new StringBuilder();

                    foreach (var dlgText in delegates)
                    {
                        var text = dlgText.DynamicInvoke(row).ToString();
                        var td = new TagBuilder("td") {InnerHtml = text};
                        sbData.Append(td.ToString(TagRenderMode.Normal));
                    }

                    bRow.InnerHtml = sbData.ToString();
                    sbBody.AppendLine(bRow.ToString(TagRenderMode.Normal));
                }

                tBody.InnerHtml = sbBody.ToString();

                table.InnerHtml = table.InnerHtml + tBody.ToString(TagRenderMode.Normal);
            }


            htmlHelper.ViewContext.Writer.Write(table.ToString(TagRenderMode.Normal));
        }
    }
}
