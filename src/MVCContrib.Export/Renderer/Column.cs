using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCContrib.Export.Renderer
{
    //shameless copy from MvcContrib.UI.Grid.IGridColumn
    public interface IColumn<T>
        where T : class
    {
        string DisplayName { set; }
        IColumn<T> named(string Name);
        IColumn<T> Format(string format);
        IColumn<T> Visible(bool isVisible);
    }
    public class Column<T> : IColumn<T>
        where T : class
    {

        public bool _visible = true;

        private string _format;
        public Func<T, object> _columnValueFunc;
        private Func<T, bool> _cellCondition = x => true;


        public Column(Func<T, object> columnValueFunc, string name)
        {
            DisplayName = name;
            _columnValueFunc = columnValueFunc;
        }
        public IColumn<T> named(string Name)
        {
            DisplayName = Name;
            return this;

        }
        public string DisplayName { get; set; }
        public IColumn<T> Format(string format)
        {
            _format = format;
            return this;
        }

        public IColumn<T> Visible(bool isVisible)
        {
            _visible = isVisible;
            return this;
        }
        public object GetValue(T instance)
        {
            return _columnValueFunc(instance);
        }
        public object GetValueFormatted(T instance)
        {
            var value = GetValue(instance);
            if (!string.IsNullOrEmpty(_format))
            {
                value = string.Format(_format, value);
            }
            return value;
        }
    }
}
