using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace MVCContrib.Export.Renderer
{
   
    public class ExportModel<T> 
        where T : class
    {
        private readonly ColumnBuilder<T> _columnBuilder = new ColumnBuilder<T>();
        public Renderer<T> Renderer { get; set; }
        public ColumnBuilder<T> Columns { get; set; }
        public ExportModel()
        {
            Columns = new ColumnBuilder<T>();
        }
        public ExportModel<T> AddColumns(Action<ColumnBuilder<T>> columnBuilder)
        {
            var builder = new ColumnBuilder<T>();
            columnBuilder(builder);

            foreach (var column in builder)
            {
                this.Columns.Add(column);

            }
            return this;
        }
        public void AutoGenerateColumns()
        {


            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties.OrderBy(prop => prop.Name))//good for testing ;-)
            {
                var propertyExpression = PropertyToExpression(property);
                Columns.For(propertyExpression);
            }
        }
        internal static Expression<Func<T, object>> PropertyToExpression(PropertyInfo property)
        {
            var parameterExpression = Expression.Parameter(typeof(T), "x");
            Expression propertyExpression = Expression.Property(parameterExpression, property);

            if (property.PropertyType.IsValueType)
            {
                propertyExpression = Expression.Convert(propertyExpression, typeof(object));
            }

            var expression = Expression.Lambda(
                typeof(Func<T, object>),
                propertyExpression,
                parameterExpression
            );

            return (Expression<Func<T, object>>)expression;
        }
        private void EnsureColumns()
        {
            if (Columns == null || Columns.Count == 0)
                AutoGenerateColumns();

            Renderer.Columns = Columns;
        }
        public byte[] Result()
        {

            EnsureColumns();
            return Renderer.GetResult();
        }
        public string ResultAsString()
        {
            EnsureColumns();

            StringRenderer<T> st = Renderer as StringRenderer<T>;
            if (st != null)
            {
                return st.GetFileContentsAsString();
            }
            else
            {
                return Encoding.UTF8.GetString(Renderer.GetResult());
            }


        }


    }
}
