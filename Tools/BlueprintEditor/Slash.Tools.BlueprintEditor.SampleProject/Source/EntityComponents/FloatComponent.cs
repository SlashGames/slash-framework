// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FloatComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.SampleProject.EntityComponents
{
    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Attributes;
    using Slash.GameBase.Components;

    [InspectorComponent]
    public class FloatComponent : IEntityComponent
    {
        #region Constants

        /// <summary>
        ///   Attribute: Test attribute.
        /// </summary>
        public const string AttributeValue = "FloatComponent.Float";

        /// <summary>
        ///   Attribute default: Test attribute.
        /// </summary>
        public const int DefaultValue = 0;

        #endregion

        #region Fields

        private float value = DefaultValue;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Test attribute.
        /// </summary>
        [InspectorFloat(AttributeValue, Default = DefaultValue, Min = -128.0f, Max = 127.87f)]
        public float Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
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
            attributeTable.TryGetFloat(AttributeValue, out this.value);
        }

        #endregion
    }
}