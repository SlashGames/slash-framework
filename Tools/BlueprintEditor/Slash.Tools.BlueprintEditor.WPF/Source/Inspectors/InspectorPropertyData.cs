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
        #region Fields

        private readonly InspectorPropertyAttribute inspectorProperty;

        private object value;

        #endregion

        #region Constructors and Destructors

        public InspectorPropertyData(InspectorPropertyAttribute inspectorProperty)
        {
            this.inspectorProperty = inspectorProperty;
        }

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
                return this.value;
            }
            set
            {
                this.SetValue(value);
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

        private void OnValueChanged(object newValue)
        {
            InspectorControlValueChangedDelegate handler = this.ValueChanged;
            if (handler != null)
            {
                handler(this.inspectorProperty, newValue);
            }
            this.OnPropertyChanged("Value");
        }

        private void SetValue(object newValue)
        {
            if (Equals(newValue, this.value))
            {
                return;
            }

            this.value = newValue;
            this.ValueInherited = false;

            // Raise event.
            this.OnValueChanged(this.value);
        }

        #endregion
    }
}