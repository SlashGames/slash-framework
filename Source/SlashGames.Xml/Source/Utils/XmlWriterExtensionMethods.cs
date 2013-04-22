namespace SlashGames.Xml.Utils
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XmlWriterExtensionMethods
    {
        public static void WriteObject(this XmlWriter writer, object obj, Type type)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            xmlSerializer.Serialize(writer, obj);
        }
    }
}