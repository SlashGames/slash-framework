// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorComponentTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Data
{
    using Slash.ECS.Components;
    using Slash.ECS.Inspector.Data;

    /// <summary>
    ///   Lookup table for designer components. Avoids expensive reflection
    ///   calls at runtime.
    /// </summary>
    public class InspectorComponentTable
    {
        #region Public Properties

        public static InspectorTypeTable Instance { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Finds all components accessible to the user in the landscape designer via reflection.
        /// </summary>
        public static void LoadComponents()
        {
            Instance = InspectorTypeTable.FindInspectorTypes(typeof(IEntityComponent));
        }

        #endregion
    }
}