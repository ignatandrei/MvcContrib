using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace MVCContrib.Export.Renderer
{
    //shameless copy from MvcContrib.UI.Grid.ColumnBuilder
    public class ColumnBuilder<T> : List<Column<T>> where T : class
    {

        public Column<T> For(Expression<Func<T, object>> propertySpecifier)
        {
            var memberExpression = GetMemberExpression(propertySpecifier);
            var type = GetTypeFromMemberExpression(memberExpression);
            var inferredName = memberExpression == null ? null : memberExpression.Member.Name;

            var column = new Column<T>(propertySpecifier.Compile(), inferredName);
            this.Add(column);
            return column;
        }
        public Column<T> For(string name)
        {
            var column = new Column<T>(ExportModel<T>.PropertyToExpression(typeof(T).GetProperty(name)).Compile(), name);
            this.Add(column);
            return column;
        }
        public static MemberExpression GetMemberExpression(LambdaExpression expression)
        {
            return RemoveUnary(expression.Body) as MemberExpression;
        }

        private static Type GetTypeFromMemberExpression(MemberExpression memberExpression)
        {
            if (memberExpression == null) return null;

            var dataType = GetTypeFromMemberInfo(memberExpression.Member, (PropertyInfo p) => p.PropertyType);
            if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (MethodInfo m) => m.ReturnType);
            if (dataType == null) dataType = GetTypeFromMemberInfo(memberExpression.Member, (FieldInfo f) => f.FieldType);

            return dataType;
        }

        private static Type GetTypeFromMemberInfo<TMember>(MemberInfo member, Func<TMember, Type> func) where TMember : MemberInfo
        {
            if (member is TMember)
            {
                return func((TMember)member);
            }
            return null;
        }

        private static Expression RemoveUnary(Expression body)
        {
            var unary = body as UnaryExpression;
            if (unary != null)
            {
                return unary.Operand;
            }
            return body;
        }

    }
}
