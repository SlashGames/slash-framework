// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinarySerializationReflectionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Binary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Slash.Reflection.Extensions;

    /// <summary>
    ///   Reflection utility methods for binary serialization and deserialization.
    /// </summary>
    public static class BinarySerializationReflectionUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Enumerates all instance fields of the specified type who have the <see cref="SerializeMemberAttribute" />
        ///   applied, ordered by name.
        /// </summary>
        /// <param name="type">Type to enumerate instance fields of.</param>
        /// <returns>
        ///   Instance fields of the specified type who have the <see cref="SerializeMemberAttribute" />
        ///   applied, ordered by name.
        /// </returns>
        public static IEnumerable<FieldInfo> ReflectFields(Type type)
        {
            FieldInfo[] fields = type.GetInstanceFields().ToArray();

            // Sort fields by name to prevent re-ordering members from being a breaking change.
            Array.Sort(fields, (first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));

            return fields.Where(field => field.IsDefined(typeof(SerializeMemberAttribute), true));
        }

        /// <summary>
        ///   Enumerates all instance properties of the specified type who have the <see cref="SerializeMemberAttribute" />
        ///   applied, ordered by name.
        /// </summary>
        /// <param name="type">Type to enumerate instance properties of.</param>
        /// <returns>
        ///   Instance properties of the specified type who have the <see cref="SerializeMemberAttribute" />
        ///   applied, ordered by name.
        /// </returns>
        public static IEnumerable<PropertyInfo> ReflectProperties(Type type)
        {
            PropertyInfo[] properties = type.GetInstanceProperties().ToArray();

            // Sort properties by name to prevent re-ordering members from being a breaking change.
            Array.Sort(properties, (first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));

            return properties.Where(property => property.IsDefined(typeof(SerializeMemberAttribute), true));
        }

        #endregion
    }
}