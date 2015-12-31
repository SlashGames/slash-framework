// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Components
{
    using Slash.Collections.AttributeTables;

    /// <summary>
    ///   Base class for an entity component.
    /// </summary>
    public class EntityComponent : IEntityComponent
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this component with the data stored in the specified attribute table.
        /// </summary>
        /// <param name="attributeTable">Component data.</param>
        public void InitComponent(IAttributeTable attributeTable)
        {
        }

        #endregion
    }
}