// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListIntComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.SampleProject.EntityComponents
{
    using System.Collections.Generic;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Components;
    using Slash.GameBase.Inspector.Attributes;

    [InspectorComponent]
    public class ListIntComponent : IEntityComponent
    {
        #region Constants

        /// <summary>
        ///   Attribute: List
        /// </summary>
        public const string AttributeList = "ListIntComponent.List";

        /// <summary>
        ///   Attribute default: List
        /// </summary>
        public const List<int> DefaultList = null;

        #endregion

        #region Constructors and Destructors

        public ListIntComponent()
        {
            this.List = DefaultList;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   List
        /// </summary>
        [InspectorInt(AttributeList, Default = DefaultList, Description = "List", List = true)]
        public List<int> List { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this component with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="attributeTable">Component data.</param>
        public void InitComponent(IAttributeTable attributeTable)
        {
            this.List = attributeTable.GetValueOrDefault(AttributeList, DefaultList);
        }

        #endregion
    }
}