// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.SampleProject.EntityComponents
{
    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Attributes;
    using Slash.GameBase.Components;

    [InspectorComponent]
    public class StringComponent : IEntityComponent
    {
        #region Constants

        /// <summary>
        ///   Attribute: Test string attribute
        /// </summary>
        public const string AttributeString = "StringComponent.String";

        /// <summary>
        ///   Attribute default: Test string attribute
        /// </summary>
        public const string DefaultString = "";

        #endregion

        #region Public Properties

        /// <summary>
        ///   Test string attribute
        /// </summary>
        [InspectorString(AttributeString, Default = DefaultString)]
        public string String { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this component with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="attributeTable">Component data.</param>
        public void InitComponent(IAttributeTable attributeTable)
        {
            this.String = (string)attributeTable.GetValue(AttributeString);
        }

        #endregion
    }
}