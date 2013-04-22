// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyValueEditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace SlashGames.Unity.Common.GUI.ValueEditors
{
    using System;
    using System.Reflection;

    public class PropertyValueEditorContext : IValueEditorContext
    {
        private object obj;

        private PropertyInfo propertyInfo;

        public PropertyValueEditorContext(object obj, PropertyInfo propertyInfo)
        {
            this.obj = obj;
            this.propertyInfo = propertyInfo;
        }

        public string Description { get; set; }

        public string Name
        {
            get
            {
                return this.propertyInfo.Name;
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

        public object Key
        {
            get
            {
                return this.propertyInfo.GetHashCode() + this.obj.GetHashCode();
            }
        }


        public Type Type
        {
            get
            {
                return this.propertyInfo.PropertyType;
            }
        }

        public T GetValue<T>()
        {
            return ValueEditorContext.GetValue<T>(this.Value);
        }

        public bool TryGetValue<T>(out T value)
        {
            return ValueEditorContext.TryGetValue(this.Value, out value);
        }
    }
}