// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniqueBlueprintIdValidationRule.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Validation
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;

    using BlueprintEditor.Controls;

    public class ValidationRuleData : DependencyObject
    {
        #region Static Fields

        public static readonly DependencyProperty ContextProperty = DependencyProperty.Register(
            "Context", typeof(BlueprintControlContext), typeof(ValidationRuleData), new FrameworkPropertyMetadata(null));

        #endregion

        #region Public Properties

        public BlueprintControlContext Context
        {
            get
            {
                return (BlueprintControlContext)this.GetValue(ContextProperty);
            }
            set
            {
                this.SetValue(ContextProperty, value);
            }
        }

        #endregion
    }

    public class UniqueBlueprintIdValidationRule : ValidationRule
    {
        #region Public Properties

        public ValidationRuleData Data { get; set; }

        #endregion

        #region Public Methods and Operators

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string blueprintId = (string)value;
            if (blueprintId == null)
            {
                return new ValidationResult(false, "Blueprint id is null.");
            }

            if (blueprintId == string.Empty)
            {
                return new ValidationResult(false, "Blueprint id is empty.");
            }

            // Check if changed.
            if (blueprintId == this.Data.Context.BlueprintId)
            {
                return new ValidationResult(true, null);
            }

            // Check if unique.
            if (this.Data.Context != null && this.Data.Context.BlueprintManager != null
                && this.Data.Context.BlueprintManager.ContainsBlueprint(blueprintId))
            {
                return new ValidationResult(false, "Blueprint id already exists.");
            }

            return new ValidationResult(true, null);
        }

        #endregion
    }
}