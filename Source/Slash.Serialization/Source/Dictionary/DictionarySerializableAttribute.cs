// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionarySerializableAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Dictionary
{
    using System;

    /// <summary>
    ///   Class or property can be serialized to a dictionary with reflection.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
    public class DictionarySerializableAttribute : Attribute
    {
    }
}