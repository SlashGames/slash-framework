// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.Reflection
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    ///   Utility functions for property reflection.
    /// </summary>
    /// <typeparam name="TType"> Class type. </typeparam>
    public static class PropertyUtils<TType>
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Can be used to get the property info of a known class property.
        ///   <para> Usage: </para>
        ///   <para> PropertyInfo propertyInfo = PropertyUtils{ClassType}.GetPropertyInfo(x => x.Property); </para>
        /// </summary>
        /// <typeparam name="TValue"> Property type. </typeparam>
        /// <param name="selector"> Lambda expression in this form: x => x.PropertyName. </param>
        /// <returns> Property info of property with property name "PropertyName" of specified class. </returns>
        public static PropertyInfo GetPropertyInfo<TValue>(Expression<Func<TType, TValue>> selector)
        {
            Expression body = selector.Body;
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion
    }
}