using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WotDossier.Common
{
    /// <summary>
    /// Business objects to Xml serializer
    /// </summary>
    public static class XmlSerializer
    {
        private static Dictionary<Type, System.Xml.Serialization.XmlSerializer> _dictionary = new Dictionary<Type, System.Xml.Serialization.XmlSerializer>();

        /// <summary>
        /// Loads the object from XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">XML data.</param>
        /// <returns></returns>
        public static T LoadObjectFromXml<T>(string xml)
        {
            if (xml == null) return default(T);
            Type type = typeof (T);
            if (xml.Length == 0) return (T)Activator.CreateInstance(type);
            using (StringReader reader = new StringReader(xml))
            {
                System.Xml.Serialization.XmlSerializer sr = GetXmlSerializer(type);
                    //SerializerCache.GetSerializer(type);
                return (T) sr.Deserialize(reader);
            }
        }

        /// <summary>
        /// Stores the object in XML.
        /// </summary>
        /// <param name="obj">The obj to serialize.</param>
        /// <returns></returns>
        public static string StoreObjectInXml(object obj)
        {
            if (obj == null) return string.Empty;
            Type type = obj.GetType();
            System.Xml.Serialization.XmlSerializer sr = GetXmlSerializer(type);
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                sr.Serialize(w, obj, new XmlSerializerNamespaces(new[] {new XmlQualifiedName(string.Empty)}));
                return sb.ToString();
            }
        }

        private static System.Xml.Serialization.XmlSerializer GetXmlSerializer(Type type)
        {
            if (_dictionary.ContainsKey(type))
            {
                return _dictionary[type];
            }
            
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
            _dictionary[type] = xmlSerializer;
            return xmlSerializer;
        }
    }
}
