using System;
using System.Resources;

namespace WotDossier.Common
{
    public static class ResourceManagerExtensions
    {
        public static string GetEnumResource(this ResourceManager resourceManager, Enum item)
        {
            return resourceManager.GetString(string.Format("{0}_{1}", item.GetType().Name, item));
        }

        public static string GetFormatedEnumResource(this ResourceManager resourceManager, Enum item, params object[] formatParams)
        {
            return string.Format(resourceManager.GetEnumResource(item) ?? string.Empty, formatParams);
        }
    }
}
