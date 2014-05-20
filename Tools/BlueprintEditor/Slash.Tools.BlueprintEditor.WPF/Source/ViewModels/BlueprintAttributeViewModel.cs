// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintAttributeViewModel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using MonitoredUndo;

    public class BlueprintAttributeViewModel : ISupportsUndo
    {
        #region Fields

        private object value;

        #endregion

        #region Constructors and Destructors

        public BlueprintAttributeViewModel(object value)
        {
            this.value = value;
        }

        #endregion

        #region Public Properties

        public BlueprintViewModel Blueprint { get; set; }

        public object DefaultValue { get; set; }

        public bool Inherited
        {
            get
            {
                return this.value == null;
            }
        }

        public string Key { get; set; }

        public BlueprintManagerViewModel Root { get; set; }

        public object Value
        {
            get
            {
                if (this.value != null)
                {
                    return this.value;
                }

                // Search in parents.
                if (this.Blueprint.Parent != null && this.Blueprint.Parent.HasAttribute(this.Key))
                {
                    bool tmp;
                    return this.Blueprint.Parent.GetCurrentAttributeValue(this.Key, out tmp);
                }

                // Return default value.
                return this.DefaultValue;
            }
            set
            {
                var defaultValue = this.DefaultValue;

                // Check parent for values.
                if (this.Blueprint.Parent != null && this.Blueprint.Parent.HasAttribute(this.Key))
                {
                    bool tmp;
                    defaultValue = this.Blueprint.Parent.GetCurrentAttributeValue(this.Key, out tmp);
                }

                var oldValue = this.value;

                // Check if property is set to default/inherited value.
                if (Equals(value, defaultValue))
                {
                    this.value = null;
                    this.Blueprint.Blueprint.AttributeTable.RemoveValue(this.Key);
                }
                else
                {
                    this.value = value;
                    this.Blueprint.Blueprint.AttributeTable.SetValue(this.Key, value);
                }

                DefaultChangeFactory.Current.OnChanging(
                    this, "Value", oldValue, this.value, "Property Changed: " + this.Key);
            }
        }

        #endregion

        #region Public Methods and Operators

        public object GetUndoRoot()
        {
            return this.Root;
        }

        public override string ToString()
        {
            return string.Format("Key: {0}", this.Key);
        }

        #endregion
    }
}