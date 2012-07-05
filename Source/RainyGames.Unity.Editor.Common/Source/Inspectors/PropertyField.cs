// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyField.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Unity.Editor.Common.Inspectors
{
    using System;
    using System.Reflection;

    using UnityEditor;

    using UnityEngine;

    using Object = System.Object;

    public class PropertyField
    {
        #region Fields

        private readonly MethodInfo getter;

        private readonly PropertyInfo info;

        private readonly Object instance;

        private readonly MethodInfo setter;

        private readonly SerializedPropertyType type;

        #endregion

        #region Constructors and Destructors

        public PropertyField(Object instance, PropertyInfo info, SerializedPropertyType type)
        {
            this.instance = instance;
            this.info = info;
            this.type = type;

            this.getter = this.info.GetGetMethod();
            this.setter = this.info.GetSetMethod();
        }

        #endregion

        #region Public Properties

        public String Name
        {
            get
            {
                return ObjectNames.NicifyVariableName(this.info.Name);
            }
        }

        public SerializedPropertyType Type
        {
            get
            {
                return this.type;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static bool GetPropertyType(PropertyInfo info, out SerializedPropertyType propertyType)
        {
            propertyType = SerializedPropertyType.Generic;

            Type type = info.PropertyType;

            if (type == typeof(int))
            {
                propertyType = SerializedPropertyType.Integer;
                return true;
            }

            if (type == typeof(float))
            {
                propertyType = SerializedPropertyType.Float;
                return true;
            }

            if (type == typeof(bool))
            {
                propertyType = SerializedPropertyType.Boolean;
                return true;
            }

            if (type == typeof(string))
            {
                propertyType = SerializedPropertyType.String;
                return true;
            }

            if (type == typeof(Vector2))
            {
                propertyType = SerializedPropertyType.Vector2;
                return true;
            }

            if (type == typeof(Vector3))
            {
                propertyType = SerializedPropertyType.Vector3;
                return true;
            }

            if (type.IsEnum)
            {
                propertyType = SerializedPropertyType.Enum;
                return true;
            }

            return false;
        }

        public Object GetValue()
        {
            return this.getter.Invoke(this.instance, null);
        }

        public void SetValue(Object value)
        {
            this.setter.Invoke(this.instance, new[] { value });
        }

        #endregion
    }
}