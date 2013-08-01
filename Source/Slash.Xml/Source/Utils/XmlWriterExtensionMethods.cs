// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlWriterExtensionMethods.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Xml.Utils
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XmlWriterExtensionMethods
    {
        #region Public Methods and Operators

        public static void WriteObject(this XmlWriter writer, object obj, Type type)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            xmlSerializer.Serialize(writer, obj);
        }

        #endregion
    }
}