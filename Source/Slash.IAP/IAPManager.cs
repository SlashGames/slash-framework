// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAPManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.IAP
{
    using System;

    public static class IAPManager
    {
        #region Public Properties

        public static bool Debug { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool BuyFeature(string key)
        {
            throw new NotImplementedException();
        }

        public static bool HasFeature(string key)
        {
            throw new NotImplementedException();
        }

        public static void Init()
        {
        }

        #endregion
    }
}