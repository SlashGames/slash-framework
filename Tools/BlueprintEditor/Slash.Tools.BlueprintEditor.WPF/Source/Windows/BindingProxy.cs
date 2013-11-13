// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindingProxy.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor
{
    using System.Windows;

    public class BindingProxy : Freezable
    {
        #region Static Fields

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        #endregion

        #region Public Properties

        public object Data
        {
            get
            {
                return this.GetValue(DataProperty);
            }
            set
            {
                this.SetValue(DataProperty, value);
            }
        }

        #endregion

        #region Methods

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion
    }
}