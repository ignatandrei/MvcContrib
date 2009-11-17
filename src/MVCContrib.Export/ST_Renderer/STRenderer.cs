using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.StringTemplate;
using MVCContrib.Export.Renderer;



namespace MVCContrib.Export.ST_Renderer
{
    /// <summary>
    /// base class to use String template as renderer
    /// see http://www.stringtemplate.org/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class STRenderer<T> : StringRenderer<T>
        where T : class
    {
        public bool CodeFormat = false;
        public string StringTemplateFileName = "";
        protected StringTemplateGroup group;
        protected StringTemplate stFile;

        public STRenderer(string FolderTemplate)
        {
            group = new StringTemplateGroup("templates", new FileSystemTemplateLoader(FolderTemplate, e));
            group.RegisterAttributeRenderer(typeof(string), new AdvancedStringRenderer());
            group.RegisterAttributeRenderer(typeof(DateTime), new AdvancedDateTimeRenderer());
            group.RegisterAttributeRenderer(typeof(decimal), new AdvancedDecimalRenderer());

        }
        public STRenderer<T> ApplyCodeFormat(bool code)
        {
            CodeFormat = code;
            return this;
        }
        public STRenderer<T> NameTemplate(string Name)
        {
            StringTemplateFileName = Name;
            return this;
        }

        protected StringTemplate StringTemplateFromFile()
        {
            if (stFile == null)
            {
                stFile = group.GetInstanceOf(StringTemplateFileName);
            }
            return stFile;
        }
        public override string GetFileContentsAsString()
        {
            Dictionary<int, Column<T>> cols = new Dictionary<int, Column<T>>();
            foreach (var col in this.Columns.Where(x => x._visible))
            {

                cols.Add(cols.Count, col);
            }
            Dictionary<long, Dictionary<string, object>> values = new Dictionary<long, Dictionary<string, object>>();
            long row = 0;
            foreach (var val in this.dataSource)
            {
                row++;
                values.Add(row, new Dictionary<string, object>());
                foreach (var col in cols.Values)
                {
                    var value = CodeFormat ? col.GetValueFormatted(val) : col.GetValue(val);
                    values[row].Add(col.DisplayName, value);

                }
            }

            StringTemplate st = StringTemplateFromFile();
            //if you want, add another attributes
            //st.SetAttribute("DateCreated", DateTime.Now);
            st.SetAttribute("header", cols);
            st.SetAttribute("lines", values);
            return st.ToString().TrimEnd();
        }

    }
}
