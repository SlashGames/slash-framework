// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityConfiguration.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Collections.AttributeTables;
    using Slash.Collections.Utils;
    using Slash.Reflection.Utils;
    using Slash.Serialization.Binary;

    /// <summary>
    ///   Contains all data to create and initialize an entity.
    /// </summary>
    [Serializable]
#if !WINDOWS_STORE
    public class EntityConfiguration : ICloneable, IBinarySerializable
#else
    public class EntityConfiguration : IBinarySerializable
#endif
    {
        #region Fields

        /// <summary>
        ///   Entity components that are added to the ones specified in the blueprint.
        /// </summary>
        private List<Type> additionalComponentTypes;

        /// <summary>
        ///   Configuration for the entity.
        /// </summary>
        private AttributeTable configuration;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public EntityConfiguration()
        {
        }

        /// <summary>
        ///   Copy constructor.
        /// </summary>
        /// <param name="entityConfiguration">Configuration to copy.</param>
        public EntityConfiguration(EntityConfiguration entityConfiguration)
        {
            this.BlueprintId = entityConfiguration.BlueprintId;
            this.additionalComponentTypes = entityConfiguration.additionalComponentTypes != null
                                                ? new List<Type>(entityConfiguration.additionalComponentTypes)
                                                : null;
            this.configuration = entityConfiguration.configuration != null
                                     ? new AttributeTable(entityConfiguration.configuration)
                                     : null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Entity components that are added to the ones specified in the blueprint.
        /// </summary>
        public List<Type> AdditionalComponentTypes
        {
            get
            {
                return this.additionalComponentTypes ?? (this.additionalComponentTypes = new List<Type>());
            }

            set
            {
                this.additionalComponentTypes = value;
            }
        }

        /// <summary>
        ///   Name of blueprint to use for the entity.
        /// </summary>
        public string BlueprintId { get; set; }

        /// <summary>
        ///   Configuration for the entity.
        /// </summary>
        public AttributeTable Configuration
        {
            get
            {
                return this.configuration ?? (this.configuration = new AttributeTable());
            }

            set
            {
                this.configuration = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///   A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new EntityConfiguration(this);
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">
        ///   The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((EntityConfiguration)obj);
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.additionalComponentTypes != null ? this.additionalComponentTypes.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.configuration != null ? this.configuration.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.BlueprintId != null ? this.BlueprintId.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        ///   Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///   A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format(
                "BlueprintId: {0}, Configuration: {1}, AdditionalComponentTypes: {2}",
                this.BlueprintId,
                this.Configuration,
                this.AdditionalComponentTypes);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Determines whether the specified <see cref="EntityConfiguration" /> is equal to the current
        ///   <see
        ///     cref="EntityConfiguration" />
        ///   .
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="EntityConfiguration" /> is equal to the current <see cref="EntityConfiguration" />; otherwise, false.
        /// </returns>
        /// <param name="other">
        ///   The <see cref="EntityConfiguration" /> to compare with the current <see cref="EntityConfiguration" />.
        /// </param>
        protected bool Equals(EntityConfiguration other)
        {
            return CollectionUtils.SequenceEqual(this.AdditionalComponentTypes, other.AdditionalComponentTypes)
                   && Equals(this.Configuration, other.Configuration)
                   && string.Equals(this.BlueprintId, other.BlueprintId);
        }

        #endregion

        /// <summary>
        ///   Reads this object from its binary representation.
        /// </summary>
        /// <param name="deserializer">Deserializer to read the object with.</param>
        public void Deserialize(BinaryDeserializer deserializer)
        {
            this.AdditionalComponentTypes =
                deserializer.Deserialize<string[]>().Select(ReflectionUtils.FindType).ToList();
            this.BlueprintId = deserializer.Deserialize<string>();
            this.Configuration = deserializer.Deserialize<AttributeTable>();
        }

        /// <summary>
        ///   Converts this object to its binary representation.
        /// </summary>
        /// <param name="serializer">Serializer to writer this object with.</param>
        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(
                this.AdditionalComponentTypes.Select(componentType => componentType.FullName).ToArray());
            serializer.Serialize(string.IsNullOrEmpty(this.BlueprintId) ? string.Empty : this.BlueprintId);
            serializer.Serialize(this.Configuration);
        }
    }
}