using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ookii.Dialogs.Wpf;
using WotDossier.Common.Reflection;

namespace WotDossier.Applications.Logic.Export
{
    public class CsvExportProvider : ExportProviderBase
    {
        public override void Export(IList list, List<Type> exportInterfaces)
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

            VistaSaveFileDialog dialog = new VistaSaveFileDialog();
            dialog.DefaultExt = ".csv"; // Default file extension
            dialog.Filter = "CSV (.csv)|*.csv"; // Filter files by extension 
            dialog.Title = Resources.Resources.WondowCaption_Export;
            bool? showDialog = dialog.ShowDialog();
            if (showDialog == true)
            {
                string fileName = dialog.FileName;
                using (StreamWriter writer = File.CreateText(fileName))
                {
                    writer.Write(builder);
                }
            }
        }

        private IEnumerable<Type> ExtendWithParent(List<Type> exportInterfaces)
        {
            return exportInterfaces;
        }
    }
}