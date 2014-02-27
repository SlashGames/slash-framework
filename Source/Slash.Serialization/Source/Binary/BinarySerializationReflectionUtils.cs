// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinarySerializationReflectionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Binary
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class BinarySerializationReflectionUtils
    {
        #region Public Methods and Operators

        public static IEnumerable<FieldInfo> ReflectFields(Type type)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Sort fields by name to prevent re-ordering members from being a breaking change.
            Array.Sort(fields, (first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));

            foreach (FieldInfo field in fields)
            {
                if (Attribute.IsDefined(field, typeof(SerializeMemberAttribute)))
                {
                    yield return field;
                }
            }
        }

        public static IEnumerable<PropertyInfo> ReflectProperties(Type type)
        {
            PropertyInfo[] properties =
                type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Sort properties by name to prevent re-ordering members from being a breaking change.
            Array.Sort(properties, (first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));

            foreach (PropertyInfo property in properties)
            {
                if (Attribute.IsDefined(property, typeof(SerializeMemberAttribute)))
                {
                    yield return property;
                }
            }
        }

        #endregion
    }
}