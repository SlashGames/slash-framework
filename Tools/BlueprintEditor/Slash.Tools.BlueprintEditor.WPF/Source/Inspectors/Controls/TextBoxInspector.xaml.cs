// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxInspector.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors.Controls
{
    using BlueprintEditor.ViewModels;

    using Slash.GameBase.Inspector.Attributes;

    /// <summary>
    ///   Inspector to edit a value in a textbox.
    /// </summary>
    public partial class TextBoxInspector
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public TextBoxInspector()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        public override void Init(
            InspectorPropertyAttribute inspectorProperty,
            LocalizationContext localizationContext,
            object currentValue,
            bool valueInherited)
        {
            base.Init(inspectorProperty, localizationContext, currentValue, valueInherited);

            // Disable for localized strings.
            // TODO(np): Enable editing while not showing RAW values.
            var stringProperty = inspectorProperty as InspectorStringAttribute;
            if (stringProperty != null && stringProperty.Localized)
            {
                this.IsEnabled = false;
            }
        }

        #endregion
    }
}