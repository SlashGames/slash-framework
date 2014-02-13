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

    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Validation;
    using Slash.SystemExt.Utils;

    public class InspectorPropertyData : IDataErrorInfo, INotifyPropertyChanged
    {
        #region Constants

        /// <summary>
        ///   Validation message when provided string can't be converted to value.
        /// </summary>
        public const string ValidationMessageConversionFailed = "String can't be converted to value.";

        #endregion

        #region Fields

        private InspectorPropertyAttribute inspectorProperty;

        private string stringValue;

        private object value;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event InspectorControlValueChangedDelegate ValueChanged;

        #endregion

        #region Public Properties

        public string Error { get; private set; }

        public InspectorPropertyAttribute InspectorProperty
        {
            get
            {
                return this.inspectorProperty;
            }
            set
            {
                this.inspectorProperty = value;
                this.OnPropertyChanged();

                // Update string value.
                this.StringValue = this.value != null
                                       ? this.inspectorProperty.ConvertValueOrListToString(this.value)
                                       : null;
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

        public string StringValue
        {
            get
            {
                return this.stringValue;
            }
            set
            {
                if (value == this.stringValue)
                {
                    return;
                }

                this.stringValue = value;

                object newValue;
                if (this.InspectorProperty.TryConvertStringToListOrValue(this.StringValue, out newValue))
                {
                    this.SetValue(newValue, false);
                }

                this.OnPropertyChanged();
            }
        }

        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.SetValue(value, true);
            }
        }

        #endregion

        #region Public Indexers

        public string this[string columnName]
        {
            get
            {
                // Implements IDataErrorInfo indexer for returning validation error messages.
                string result = null;
                if (columnName == "StringValue")
                {
                    object convertedValue;
                    bool isValid = this.InspectorProperty.TryConvertStringToListOrValue(
                        this.StringValue, out convertedValue);
                    if (!isValid)
                    {
                        result = ValidationMessageConversionFailed;
                    }
                    else
                    {
                        // Validate value itself.
                        ValidationError validationError = this.inspectorProperty.Validate(this.value);
                        if (validationError != null)
                        {
                            result = validationError.Message;
                        }
                    }
                }
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

        private void OnValueChanged(object newValue)
        {
            InspectorControlValueChangedDelegate handler = this.ValueChanged;
            if (handler != null)
            {
                handler(this.inspectorProperty, newValue);
            }
            this.OnPropertyChanged("Value");
        }

        private void SetValue(object newValue, bool updateStringValue)
        {
            if (Equals(newValue, this.value))
            {
                return;
            }

            this.value = newValue;

            if (updateStringValue)
            {
                // Update string value.
                this.StringValue = this.inspectorProperty.ConvertValueOrListToString(this.value);
            }

            // Raise event.
            this.OnValueChanged(this.value);
        }

        #endregion
    }
}