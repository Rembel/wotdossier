using System.Collections;
using Newtonsoft.Json;

namespace WotDossier.Applications
{
    public static class DictionaryExtensions
    {
        public static T ToObject<T>(this object unpickleObjectDictonary)
        {
            if (unpickleObjectDictonary as IDictionary != null)
            {
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(unpickleObjectDictonary));
            }
            return default(T);
        }
    }
}