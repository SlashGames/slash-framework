// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.SampleProject.EntityComponents
{
    using Slash.Collections.AttributeTables;
    using Slash.ECS.Components;
    using Slash.ECS.Inspector.Attributes;

    [InspectorComponent(Description = "A test component with a integer attribute.")]
    public class IntegerComponent : IEntityComponent
    {
        #region Constants

        /// <summary>
        ///   Attribute: Test integer attribute.
        /// </summary>
        public const string AttributeInteger = "IntegerComponent.Integer";

        /// <summary>
        ///   Attribute default: Test integer attribute.
        /// </summary>
        public const int DefaultInteger = 0;

        #endregion

        #region Fields

        private int integer = DefaultInteger;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Test integer attribute.
        /// </summary>
        [InspectorInt(AttributeInteger, Description = "Test int attribute", Default = DefaultInteger, Min = 0, Max = 31)
        ]
        public int Integer
        {
            get
            {
                return this.integer;
            }
            set
            {
                this.integer = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this component with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="attributeTable">Component data.</param>
        public void InitComponent(IAttributeTable attributeTable)
        {
            attributeTable.TryGetInt(AttributeInteger, out this.integer);
        }

        #endregion
    }
}