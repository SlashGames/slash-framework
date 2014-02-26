// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializeMember.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization
{
    using System;

    /// <summary>
    ///   Member should be serialized with reflection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializeMemberAttribute : Attribute
    {
    }
}