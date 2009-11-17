using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.StringTemplate;


namespace MVCContrib.Export.ST_Renderer
{
    public class STExcel2003Renderer<T> : STRenderer<T>
        where T : class
    {

        public STExcel2003Renderer(string FolderTemplate)
            : base(FolderTemplate)
        {
            StringTemplateFileName = "Excel2003";
        }


    }
}
