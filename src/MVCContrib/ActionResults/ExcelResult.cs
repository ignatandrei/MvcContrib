using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCContrib.Export;
using MVCContrib.Export.ST_Renderer;
using MVCContrib.Export.Renderer;

namespace MvcContrib.ActionResults
{
    /// <summary>
    /// export to Excel 2003
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelResult<T> : BaseResult<T>
       where T : class
    {
        /// <summary>
        /// please ensure that you have default templates folder
        /// and excel2003.st file if you use the default constructor
        /// </summary>
        /// <param name="exp"></param>
        public ExcelResult(ExportModel<T> exp) :
            base("application/vnd.ms-excel", exp)
        {

        }

        /// <summary>
        /// most complete constructor
        /// </summary>
        /// <param name="datasource">the data source to export</param>
        /// <param name="FolderTemplate">where to find st parameters</param>
        /// <param name="TemplateName">name of the template without .st </param>
        public ExcelResult(IEnumerable<T> datasource, string FolderTemplate, string TemplateName) :
            this(new ExportModel<T>() { Renderer = new STExcel2003Renderer<T>(FolderTemplate) { dataSource = datasource, StringTemplateFileName = TemplateName } })
        {

        }
        /// <summary>
        /// ensure Excel2003.st does exists in FolderTemplate
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="FolderTemplate"></param>
        public ExcelResult(IEnumerable<T> datasource, string FolderTemplate) :
            this(datasource, FolderTemplate, "Excel2003")
        {
        }

    }
}
