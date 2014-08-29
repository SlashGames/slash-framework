// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorTypeTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.ECS.Inspector.Attributes;
    using Slash.Reflection.Extensions;
    using Slash.Reflection.Utils;

    /// <summary>
    ///   Lookup table for inspector components. Avoids expensive reflection
    ///   calls at runtime.
    /// </summary>
    public class InspectorTypeTable : IEnumerable<InspectorType>
    {
        #region Fields

        /// <summary>
        ///   Attributes whose inspectors are shown only if a certain condition is satisfied.
        /// </summary>
        private readonly Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute>
            conditionalInspectors;

        /// <summary>
        ///   Components accessible to the user in the inspector.
        /// </summary>
        private readonly Dictionary<Type, InspectorType> inspectorTypes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new, empty lookup table for inspector components.
        /// </summary>
        public InspectorTypeTable()
        {
            this.inspectorTypes = new Dictionary<Type, InspectorType>();
            this.conditionalInspectors =
                new Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute>();
        }

        #endregion

        #region Public Indexers

        /// <summary>
        ///   Inspector data for the specified type.
        /// </summary>
        /// <param name="type">Type to get inspector data for.</param>
        /// <returns>Inspector type for the specified type.</returns>
        public InspectorType this[Type type]
        {
            get
            {
                return this.GetInspectorType(type);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Finds all types accessible to the user in the inspector via reflection.
        /// </summary>
        /// <param name="baseType">Base type of types to search for. Null if all inspector types should be found.</param>
        /// <returns>Inspector type table containing all available inspector types.</returns>
        public static InspectorTypeTable FindInspectorTypes(Type baseType)
        {
            InspectorTypeTable inspectorTypeTable = new InspectorTypeTable();

            foreach (var assembly in AssemblyUtils.GetLoadedAssemblies())
            {
                var inspectorTypes =
                    assembly.GetTypes()
                            .Where(
                                type =>
                                baseType == null
                                || baseType.IsAssignableFrom(type)
                                && type.IsAttributeDefined<InspectorTypeAttribute>());

                foreach (var inspectorType in inspectorTypes)
                {
                    InspectorType inspectorTypeData = InspectorType.GetInspectorType(inspectorType);

                    inspectorTypeTable.inspectorTypes.Add(inspectorType, inspectorTypeData);
                }
            }

            return inspectorTypeTable;
        }

        /// <summary>
        ///   Gets the condition for the specified attribute to have its inspector shown,
        ///   or <c>null</c> if the inspector should always be shown.
        /// </summary>
        /// <param name="attribute">Attribute to get the inspector condition of.</param>
        /// <returns>
        ///   Condition for the specified attribute to have its inspector shown,
        ///   or <c>null</c> if the inspector should always be shown.
        /// </returns>
        public InspectorConditionalPropertyAttribute GetCondition(InspectorPropertyAttribute attribute)
        {
            InspectorConditionalPropertyAttribute condition;
            this.conditionalInspectors.TryGetValue(attribute, out condition);
            return condition;
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<InspectorType> GetEnumerator()
        {
            return this.inspectorTypes.Values.GetEnumerator();
        }

        /// <summary>
        ///   Gets the inspector type for the specified type.
        /// </summary>
        /// <param name="type">Type to get the inspector type for.</param>
        /// <returns>Inspector type for the specified type.</returns>
        public InspectorType GetInspectorType(Type type)
        {
            return this.inspectorTypes[type];
        }

        /// <summary>
        ///   Whether the specified type is accessible to the user in the inspector.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>
        ///   <c>true</c>, if the specified type is accessible to the user in the inspector, and
        ///   <c>false</c>, otherwise.
        /// </returns>
        public bool HasType(Type type)
        {
            return this.inspectorTypes.ContainsKey(type);
        }

        /// <summary>
        ///   Types accessible to the user in the inspector.
        /// </summary>
        /// <returns>All types accessible to the user in the inspector.</returns>
        public IEnumerable<Type> Types()
        {
            return this.inspectorTypes.Keys;
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}