using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Teriyaki
{
    public static class ExpressionExtensions
    {
        public static string GetMemberName<T>(this Expression<T> expression) =>
            expression.Body switch
            {
                MemberExpression m =>
                    m.Member.Name,
                UnaryExpression u when u.Operand is MemberExpression m =>
                    m.Member.Name,
                _ =>
                    throw new NotImplementedException(expression.GetType().ToString())
            };
        public static PropertyInfo GetMemberPropertyInfo<T>(this Expression<T> expression)
        {
            PropertyInfo? propertyInfo = expression.Body switch
            {
                MemberExpression m =>
                    m.Member as PropertyInfo,
                UnaryExpression u when u.Operand is MemberExpression m =>
                    m.Member as PropertyInfo,
                _ =>
                    throw new NotImplementedException(expression.GetType().ToString())
            };
            return propertyInfo;
        }
    }
}
