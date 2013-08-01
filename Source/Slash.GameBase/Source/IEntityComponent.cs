// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase
{
    using Slash.Collections.AttributeTables;

    /// <summary>
    ///   Contract that all game components have to fulfill if they are to be
    ///   attached to entities.
    /// </summary>
    public interface IEntityComponent
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this component with the data stored in the passed
        ///   attribute table.
        /// </summary>
        /// <param name="attributeTable">Component data.</param>
        void InitComponent(IAttributeTable attributeTable);

        #endregion
    }
}