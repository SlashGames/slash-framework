// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorTypeTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.Reflection.Utils;

    /// <summary>
    ///   Lookup table for designer components. Avoids expensive reflection
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
        ///   Components accessible to the user in the landscape designer.
        /// </summary>
        private readonly Dictionary<Type, InspectorType> inspectorTypes;

        #endregion

        #region Constructors and Destructors

        public InspectorTypeTable()
        {
            this.inspectorTypes = new Dictionary<Type, InspectorType>();
            this.conditionalInspectors =
                new Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute>();
        }

        #endregion

        #region Public Indexers

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
                                && Attribute.IsDefined(type, typeof(InspectorTypeAttribute)));

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
        ///   Types accessible to the user in the landscape designer.
        /// </summary>
        /// <returns>All types accessible to the user in the landscape designer.</returns>
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