// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyField.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors
{
    using System;
    using System.Reflection;

    using Slash.Math.Geometry.Rectangles;
    using Slash.Unity.Common.Math;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    ///   Property wrapper for mono behaviour properties exposed in the Unity inspector.
    /// </summary>
    public class PropertyField
    {
        #region Fields

        private readonly MethodInfo getter;

        private readonly object instance;

        private readonly PropertyInfo property;

        private readonly MethodInfo setter;

        private readonly SerializedPropertyType type;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Creates a new wrapper for mono behaviour properties exposed in the Unity inspector.
        /// </summary>
        /// <param name="instance">Object to wrap the property of.</param>
        /// <param name="property">Property to wrap.</param>
        /// <param name="type">Type of the property to wrap.</param>
        public PropertyField(object instance, PropertyInfo property, SerializedPropertyType type)
        {
            this.instance = instance;
            this.property = property;
            this.type = type;

            this.getter = this.property.GetGetMethod();
            this.setter = this.property.GetSetMethod();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Conversion function when getting the value to show in the inspector.
        /// </summary>
        public Func<object, object> GetConversionFunc { get; set; }

        /// <summary>
        ///   Name of the wrapped property to be shown in the inspector.
        /// </summary>
        public string Name
        {
            get
            {
                return ObjectNames.NicifyVariableName(this.property.Name);
            }
        }

        /// <summary>
        ///   Conversion function when writing the value from the inspector back to the property.
        /// </summary>
        public Func<object, object> SetConversionFunc { get; set; }

        /// <summary>
        ///   Serialized type of the wrapped property.
        /// </summary>
        public SerializedPropertyType Type
        {
            get
            {
                return this.type;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the property type and the conversion functions to use when showing the value in the inspector and when
        ///   the value is written back to the property.
        /// </summary>
        /// <param name="info"> Property info. </param>
        /// <param name="propertyType"> Defines which inspector control to use to show the value. </param>
        /// <param name="getConversionFunc"> Conversion function when getting the value to show in the inspector. </param>
        /// <param name="setConversionFunc"> Conversion function when writing the value from the inspector back to the property. </param>
        /// <returns> True if the property contains a value which can be visualized in the inspector; otherwise, false. </returns>
        public static bool GetPropertyType(
            PropertyInfo info,
            out SerializedPropertyType propertyType,
            out Func<object, object> getConversionFunc,
            out Func<object, object> setConversionFunc)
        {
            propertyType = SerializedPropertyType.Generic;
            getConversionFunc = null;
            setConversionFunc = null;

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

            if (type == typeof(Rect))
            {
                propertyType = SerializedPropertyType.Rect;
                return true;
            }

            if (type == typeof(RectangleI))
            {
                propertyType = SerializedPropertyType.Rect;
                getConversionFunc = o => ((RectangleI)o).ToRect();
                setConversionFunc = o => ((Rect)o).ToRectangleI();
                return true;
            }

            if (type == typeof(RectangleF))
            {
                propertyType = SerializedPropertyType.Rect;
                getConversionFunc = o => ((RectangleF)o).ToRect();
                setConversionFunc = o => ((Rect)o).ToRectangleF();
                return true;
            }

            if (type.IsEnum)
            {
                propertyType = SerializedPropertyType.Enum;
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Returns the converted current property value.
        /// </summary>
        /// <returns>Current property value.</returns>
        /// <see cref="GetConversionFunc" />
        public object GetValue()
        {
            return this.getter.Invoke(this.instance, null);
        }

        /// <summary>
        ///   Converts and sets the current property value.
        /// </summary>
        /// <param name="value">New property value.</param>
        /// <see cref="SetConversionFunc" />
        public void SetValue(object value)
        {
            if (this.setter != null)
            {
                this.setter.Invoke(this.instance, new[] { value });
            }
        }

        #endregion
    }
}