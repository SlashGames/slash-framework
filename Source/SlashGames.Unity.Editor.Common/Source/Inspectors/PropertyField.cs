// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyField.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.Unity.Editor.Common.Inspectors
{
    using System;
    using System.Reflection;

    using SlashGames.Unity.Common.Math;

    using SlashGames.Math.Geometry.Rectangles;

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
        
        /// <summary>
        ///   Conversion function when getting the value to show in the inspector.
        /// </summary>
        public Func<object, object> GetConversionFunc { get; set; }

        public String Name
        {
            get
            {
                return ObjectNames.NicifyVariableName(this.info.Name);
            }
        }

        /// <summary>
        ///   Conversion function when writing the value from the inspector back to the property. 
        /// </summary>
        public Func<Object, Object> SetConversionFunc { get; set; }

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

        public Object GetValue()
        {
            return this.getter.Invoke(this.instance, null);
        }

        public void SetValue(Object value)
        {
            if (this.setter != null)
            {
                this.setter.Invoke(this.instance, new[] { value });
            }
        }

        #endregion
    }
}