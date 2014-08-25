// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAPManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.IAP
{
    using System;

    using Windows.ApplicationModel.Store;
    using Windows.Foundation;

    public static class IAPManager
    {
        #region Static Fields

        private static LicenseInformation licenseInformation;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Whether to use local debug license information instead of the current user account.
        /// </summary>
        /// <see
        ///   cref="http://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.applicationmodel.store.currentappsimulator.aspx" />
        public static bool Debug { get; set; }

        #endregion

        #region Public Methods and Operators

        public static bool BuyFeature(string key)
        {
            ProductLicense productLicense = licenseInformation.ProductLicenses[key];

            if (productLicense == null)
            {
                throw new ArgumentException("Unknown product: " + key, "key");
            }

            if (!productLicense.IsActive)
            {
                // The customer doesn't own this feature, so show the purchase dialog.
                IAsyncOperation<string> requestProductPurchaseAsync =
                    CurrentAppSimulator.RequestProductPurchaseAsync(key, false);
                requestProductPurchaseAsync.AsTask().Wait();

                // Check the license state to determine if the in-app purchase was successful.
                return HasFeature(key);
            }

            return true;
        }

        public static bool HasFeature(string key)
        {
            if (licenseInformation == null)
            {
                throw new InvalidOperationException("IAP manager not initialized. Call Init() first.");
            }

            ProductLicense productLicense = licenseInformation.ProductLicenses[key];

            if (productLicense == null)
            {
                throw new ArgumentException("Unknown product: " + key, "key");
            }

            return productLicense.IsActive;
        }

        public static void Init()
        {
            // Get the license info.
            licenseInformation = Debug ? CurrentAppSimulator.LicenseInformation : CurrentApp.LicenseInformation;
        }

        #endregion
    }
}