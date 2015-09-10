using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WotDossier.Common.Reflection;

namespace WotDossier.Applications.Logic.Export
{
    public class CsvExportProvider : ExportProviderBase
    {
        public override string Export(IList list, List<Type> exportInterfaces)
        {
            StringBuilder builder = new StringBuilder();

            Type itemType = list.GetType().GetGenericArguments()[0];

            List<PropertyInfoEx> properties = new List<PropertyInfoEx>();

            var interfaces = ExtendWithParent(exportInterfaces);

            foreach (Type exportInterface in interfaces)
            {
                if (exportInterface.IsAssignableFrom(itemType))
                {
                    properties.AddRange(exportInterface.GetPublicProperties());
                }
            }

            builder.AppendLine(string.Join(";", properties.Select(x => x.PropertyInfo.Name).ToArray()));

            foreach (object item in list)
            {
                List<object> values = properties.Where(x => x.PropertyInfo.DeclaringType.IsAssignableFrom(itemType)).Select(propertyInfo => propertyInfo.PropertyInfo.GetValue(item, null)).ToList();
                builder.AppendLine(string.Join(";", values.ToArray()));
            }

            return builder.ToString();
        }

        private IEnumerable<Type> ExtendWithParent(List<Type> exportInterfaces)
        {
            return exportInterfaces;
        }
    }
}