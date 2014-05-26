using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WotDossier.Common.Reflection
{
    /// <summary>
    /// Replection extension methods 
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the public properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static PropertyInfoEx[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new List<PropertyInfoEx>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    propertyInfos.InsertRange(0, typeProperties.Select(x => new PropertyInfoEx { PropertyInfo = x, Type = subType }));
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy
                | BindingFlags.Public | BindingFlags.Instance).Select(x => new PropertyInfoEx { PropertyInfo = x, Type = type }).ToArray();
        }

        /// <summary>
        /// Gets the public property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfoEx GetPublicProperty(this Type type, string propertyName)
        {
            PropertyInfo propertyInfo;

            if (type.IsInterface)
            {
                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    propertyInfo = subType.GetProperty(propertyName, 
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    if (propertyInfo != null)
                    {
                        return new PropertyInfoEx{Type = subType, PropertyInfo = propertyInfo};
                    }
                }

                return null;
            }

            propertyInfo = type.GetProperty(propertyName, BindingFlags.FlattenHierarchy
                | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null)
            {
                return new PropertyInfoEx { Type = type, PropertyInfo = propertyInfo };
            }

            return null;
        }
    }
}
