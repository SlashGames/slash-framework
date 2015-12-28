// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorPropertyData.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.ViewModels;

    using Slash.ECS.Inspector.Attributes;
    using Slash.ECS.Inspector.Validation;
    using Slash.SystemExt.Utils;

    public class InspectorPropertyData : IDataErrorInfo, INotifyPropertyChanged
    {
        #region Fields

        private readonly InspectorPropertyAttribute inspectorProperty;

        private readonly LocalizationContext localizationContext;

        private object value;

        #endregion

        #region Constructors and Destructors

        public InspectorPropertyData(
            InspectorPropertyAttribute inspectorProperty, LocalizationContext localizationContext)
        {
            this.inspectorProperty = inspectorProperty;
            
            // Check for localized attribute.
            var stringProperty = this.inspectorProperty as InspectorStringAttribute;

            if (stringProperty != null && stringProperty.Localized)
            {
                this.localizationContext = localizationContext;

                if (this.localizationContext != null)
                {
                    this.localizationContext.ProjectLanguageChanged += this.OnProjectLanguageChanged;
                }
            }
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event InspectorControlValueChangedDelegate ValueChanged;

        #endregion

        #region Public Properties

        public string Error { get; private set; }

        public EditorContext EditorContext { get; set; }

        public InspectorPropertyAttribute InspectorProperty
        {
            get
            {
                return this.inspectorProperty;
            }
        }

        public string PropertyName
        {
            get
            {
                var propertyName = this.inspectorProperty.Name;
                if (propertyName.Contains("."))
                {
                    propertyName = propertyName.Substring(propertyName.LastIndexOf('.') + 1);
                }
                propertyName = propertyName.SplitByCapitalLetters();
                return propertyName;
            }
        }

        public object Value
        {
            get
            {
                if (this.localizationContext != null)
                {
                    var localizedValue = this.localizationContext.GetLocalizedStringForCurrentBlueprint(this.inspectorProperty.Name);
                    return localizedValue;
                }

                return this.value;
            }
            set
            {
                if (Equals(value, this.value))
                {
                    return;
                }

                // Don't change localization keys via inspector.
                if (this.localizationContext != null)
                {
                    return;
                }

                this.value = value;
                this.ValueInherited = false;

                // Raise event.
                this.OnValueChanged(this.value);
            }
        }

        /// <summary>
        ///   Indicates if the current value was inherited.
        /// </summary>
        public bool ValueInherited { get; set; }

        #endregion

        #region Public Indexers

        public string this[string columnName]
        {
            get
            {
                if (this.localizationContext != null)
                {
                    // Don't validate localized strings.
                    return null;
                }

                // Implements IDataErrorInfo indexer for returning validation error messages.
                string result = null;
                if (columnName == "Value")
                {
                    ValidationError validationError = this.inspectorProperty.Validate(this.value);
                    if (validationError != null)
                    {
                        result = validationError.Message;
                    }
                }
                return result;
            }
        }

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnProjectLanguageChanged(string newLanguage)
        {
            if (this.localizationContext != null)
            {
                // Update inspector controls, showing value for the current language.
                this.OnPropertyChanged("Value");
            }
        }

        private void OnValueChanged(object newValue)
        {
            InspectorControlValueChangedDelegate handler = this.ValueChanged;
            if (handler != null)
            {
                handler(this.inspectorProperty, newValue);
            }
        }

        #endregion
    }
}