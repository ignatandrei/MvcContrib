using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MVCContrib.Export.Renderer;

namespace MVCContrib.Export
{
    public abstract class BaseResult<T> : FileResult
       where T : class
    {
        public ExportModel<T> export;

        public BaseResult(string contentType, ExportModel<T> exp)
            : base(contentType)
        {
            export = exp;
        }
        public BaseResult(string contentType)
            : this(contentType, null)
        {

        }
        protected override void WriteFile(System.Web.HttpResponseBase response)
        {

            byte[] b = export.Result();
            response.OutputStream.Write(b, 0, b.Length);
        }
    }
    public static class Extensions
    {
        public static ExportModel<T> Render<T>(this IEnumerable<T> dataSource, Renderer<T> render)
            where T : class
        {
            ExportModel<T> exp = new ExportModel<T>();
            exp.Renderer = render;
            render.dataSource = dataSource;
            return exp;
        }

    }
}
