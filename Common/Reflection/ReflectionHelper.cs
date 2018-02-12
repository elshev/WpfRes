using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Reflection
{
    public static class ReflectionHelper
    {
        public static string GetPropertyNameFromExpression<T>(Expression<Func<T>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            UnaryExpression unaryExpression = lambda.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            return memberExpression.Member.Name;
        }


    }
}
