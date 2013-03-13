using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WotDossier.Common
{
    /// <summary>
    /// 	Represents helper class for types.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class TypeHelper<T>
    {

        private const string PROPERY_ACCESSOR = ".";

        /// <summary>
        /// 	Get the property info.
        /// </summary>
        private static PropertyInfo GetPropertyInternal(LambdaExpression p)
        {
            MemberExpression memberExpression;

            if (p.Body is UnaryExpression)
            {
                var ue = (UnaryExpression)p.Body;
                memberExpression = (MemberExpression)ue.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)p.Body;
            }

            return (PropertyInfo)(memberExpression).Member;
        }

        /// <summary>
        /// 	Gets name of property.
        /// </summary>
        public static string PropertyName(Expression<Func<T, object>> expression)
        {
            return GetPropertyInternal(expression).Name;
        }

        /// <summary>
        /// 	Gets name of property.
        /// </summary>
        public static string PropertyName<TP>(Expression<Func<TP, object>> pe, Expression<Func<T, object>> expression)
        {
            return string.Join(PROPERY_ACCESSOR,
                        new[]
                            {
                                GetPropertyInternal(pe).Name,
                                GetPropertyInternal(expression).Name
                            });

        }

        /// <summary>
        /// 	Gets name of property.
        /// </summary>
        public static string PropertyName<TP1,TP2>(Expression<Func<TP1, object>> ppe,Expression<Func<TP2, object>> pe,Expression<Func<T, object>> expression)
        {
            return string.Join(PROPERY_ACCESSOR,
                        new []
                            {
                                GetPropertyInternal(ppe).Name, 
                                GetPropertyInternal(pe).Name,
                                GetPropertyInternal(expression).Name
                            });
  
        }

    }

}
