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

    using Slash.GameBase.Attributes;
    using Slash.GameBase.Inspector.Validation;

    public class InspectorPropertyData : IDataErrorInfo, INotifyPropertyChanged
    {
        #region Constants

        /// <summary>
        ///   Validation message when provided string can't be converted to value.
        /// </summary>
        private const string ValidationMessageConversionFailed = "String can't be converted to value.";

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
                this.StringValue = this.value != null ? this.inspectorProperty.ConvertToString(this.value) : null;
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
                if (this.InspectorProperty.TryConvertFromString(this.StringValue, out newValue))
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
                string result = null;
                if (columnName == "StringValue")
                {
                    object convertedValue;
                    bool isValid = this.InspectorProperty.TryConvertFromString(this.StringValue, out convertedValue);
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

        private void OnValueChanged(object newValue, object oldValue)
        {
            InspectorControlValueChangedDelegate handler = this.ValueChanged;
            if (handler != null)
            {
                handler(this.inspectorProperty, newValue, oldValue);
            }
        }

        private void SetValue(object newValue, bool updateStringValue)
        {
            if (Equals(newValue, this.value))
            {
                return;
            }

            object oldValue = this.value;
            this.value = newValue;

            if (updateStringValue)
            {
                // Update string value.
                this.StringValue = this.inspectorProperty.ConvertToString(this.value);
            }

            // Raise event.
            this.OnValueChanged(this.value, oldValue);
            this.OnPropertyChanged("Value");
        }

        #endregion
    }
}