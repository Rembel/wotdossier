using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Ookii.Dialogs.Wpf;
using WotDossier.Applications.ViewModel.Rows;

namespace WotDossier.Applications.Logic.Export
{
    public class CsvExportProvider : ExportProviderBase<TankStatisticRowViewModel>
    {
        public override void Export(List<TankStatisticRowViewModel> list, List<Type> exportInterfaces)
        {
            StringBuilder builder = new StringBuilder();

            Type itemType = list.GetType().GetGenericArguments()[0];

            List<PropertyInfo> properties = new List<PropertyInfo>();

            foreach (Type exportInterface in exportInterfaces)
            {
                if (exportInterface.IsAssignableFrom(itemType))
                {
                    properties.AddRange(exportInterface.GetProperties());
                }
            }

            builder.AppendLine(string.Join(";", properties.Select(x => x.Name).ToArray()));

            foreach (TankStatisticRowViewModel item in list)
            {
                List<object> values = properties.Select(propertyInfo => propertyInfo.GetValue(item, null)).ToList();
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
    }
}