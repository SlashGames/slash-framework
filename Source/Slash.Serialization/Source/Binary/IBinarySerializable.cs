// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBinarySerializable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Binary
{
    public interface IBinarySerializable
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Reads this object from its binary representation.
        /// </summary>
        /// <param name="deserializer">Deserializer to read the object with.</param>
        void Deserialize(BinaryDeserializer deserializer);

        /// <summary>
        ///   Converts this object to its binary representation.
        /// </summary>
        /// <param name="serializer">Serializer to writer this object with.</param>
        void Serialize(BinarySerializer serializer);

        #endregion
    }
}