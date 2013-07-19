// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyValueEditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.GUI.ValueEditors
{
    using System;
    using System.Reflection;

    public class PropertyValueEditorContext : IValueEditorContext
    {
        #region Fields

        private readonly object obj;

        private readonly PropertyInfo propertyInfo;

        #endregion

        #region Constructors and Destructors

        public PropertyValueEditorContext(object obj, PropertyInfo propertyInfo)
        {
            this.obj = obj;
            this.propertyInfo = propertyInfo;
        }

        #endregion

        #region Public Properties

        public string Description { get; set; }

        public object Key
        {
            get
            {
                return this.propertyInfo.GetHashCode() + this.obj.GetHashCode();
            }
        }

        public string Name
        {
            get
            {
                return this.propertyInfo.Name;
            }
        }

        public Type Type
        {
            get
            {
                return this.propertyInfo.PropertyType;
            }
        }

        public object Value
        {
            get
            {
                return this.propertyInfo.GetValue(this.obj, null);
            }
            set
            {
                this.propertyInfo.SetValue(this.obj, value, null);
            }
        }

        #endregion

        #region Public Methods and Operators

        public T GetValue<T>()
        {
            return ValueEditorContext.GetValue<T>(this.Value);
        }

        public bool TryGetValue<T>(out T value)
        {
            return ValueEditorContext.TryGetValue(this.Value, out value);
        }

        #endregion
    }
}