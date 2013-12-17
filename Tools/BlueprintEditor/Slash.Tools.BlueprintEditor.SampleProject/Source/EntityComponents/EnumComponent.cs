// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.SampleProject.EntityComponents
{
    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Components;
    using Slash.GameBase.Inspector.Attributes;

    public enum TestEnum
    {
        One,

        Forbidden,

        Two,

        Default,

        Three
    }

    [InspectorComponent(Description = "A test component with a enum attribute.")]
    public class EnumComponent : IEntityComponent
    {
        #region Constants

        /// <summary>
        ///   Attribute: Test attribute.
        /// </summary>
        public const string AttributeValue = "EnumComponent.Value";

        /// <summary>
        ///   Attribute default: Test attribute.
        /// </summary>
        public const TestEnum DefaultValue = TestEnum.Default;

        #endregion

        #region Fields

        private TestEnum value = DefaultValue;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Test attribute.
        /// </summary>
        [InspectorEnum(AttributeValue, typeof(TestEnum), Description = "Test enum attribute", Default = DefaultValue,
            ForbiddenValues = new object[] { TestEnum.Forbidden })]
        public TestEnum Value
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
            attributeTable.TryGetValue(AttributeValue, out this.value);
        }

        #endregion
    }
}