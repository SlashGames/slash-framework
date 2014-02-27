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

        void Deserialize(BinaryDeserializer serializer);

        void Serialize(BinarySerializer serializer);

        #endregion
    }
}