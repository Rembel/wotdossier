using System;
using System.Reflection;

namespace WotDossier.Common.Reflection
{
    /// <summary>
    /// Extended property info
    /// </summary>
    public class PropertyInfoEx
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the property information.
        /// </summary>
        /// <value>
        /// The property information.
        /// </value>
        public PropertyInfo PropertyInfo { get; set; }

        public override string ToString()
        {
            return PropertyInfo.ToString();
        }
    }
}