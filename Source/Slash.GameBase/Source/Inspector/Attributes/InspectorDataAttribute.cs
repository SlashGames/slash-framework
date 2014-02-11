// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorDataAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Attributes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Slash.GameBase.Inspector.Validation;

    public class InspectorDataAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        public InspectorDataAttribute(string name)
            : base(name)
        {
        }
        
        #endregion

        #region Public Methods and Operators

        public override object ConvertStringToValue(string text)
        {
            throw new NotImplementedException();
        }

        public override string ConvertValueToString(object value)
        {
            return value.ToString();
        }

        public override IList GetEmptyList()
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(this.PropertyType));
        }

        public override bool TryConvertStringToValue(string text, out object value)
        {
            value = null;
            return false;
        }

        public override bool TryConvertValueToString(object value, out string text)
        {
            text = value.ToString();
            return true;
        }

        public override ValidationError Validate(object value)
        {
            if (value == null)
            {
                return ValidationError.Null;
            }

            if (value.GetType() != this.PropertyType)
            {
                return ValidationError.WrongType;
            }

            return null;
        }

        #endregion
    }
}