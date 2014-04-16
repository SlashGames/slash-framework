// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListComponent.cs" company="Slash Games">
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
    public class ListComponent : IEntityComponent
    {
        #region Constants

        /// <summary>
        ///   Attribute: ListInt
        /// </summary>
        public const string AttributeListInt = "ListComponent.ListInt";

        /// <summary>
        ///   Attribute: ListString
        /// </summary>
        public const string AttributeListString = "ListComponent.ListString";

        /// <summary>
        ///   Attribute default: ListInt
        /// </summary>
        public const List<int> DefaultListInt = null;

        /// <summary>
        ///   Attribute default: ListString
        /// </summary>
        public const List<string> DefaultListString = null;

        #endregion

        #region Constructors and Destructors

        public ListComponent()
        {
            this.ListInt = DefaultListInt;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   ListInt
        /// </summary>
        [InspectorInt(AttributeListInt, Default = DefaultListInt, Description = "ListInt Int")]
        public List<int> ListInt { get; set; }

        /// <summary>
        ///   ListString
        /// </summary>
        [InspectorString(AttributeListString, Description = "ListString", Default = DefaultListString)]
        public List<string> ListString { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this component with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="attributeTable">Component data.</param>
        public void InitComponent(IAttributeTable attributeTable)
        {
            this.ListInt = attributeTable.GetValueOrDefault(AttributeListInt, DefaultListInt);
        }

        #endregion
    }
}