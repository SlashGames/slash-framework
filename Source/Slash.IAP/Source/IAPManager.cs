// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAPManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.IAP
{
    using System;

#if WINDOWS_STORE
    using Windows.ApplicationModel.Store;
#endif

    public static class IAPManager
    {
#if WINDOWS_STORE
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

        public static Action<string> OnBuyFeature;

        public static Action<string> OnSimulateBuyFeature;

        public static Action<string> OnPurchaseSucceeded;

        public static Action<string> OnPurchaseFailed;


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
                if (Debug)
                {
                    var handler = OnSimulateBuyFeature;
                    if (handler != null)
                    {
                        handler(key);
                    }
                }
                else
                {
                    var handler = OnBuyFeature;
                    if (handler != null)
                    {
                        handler(key);
                    }
                }


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
#if DEBUG
            licenseInformation = Debug ? CurrentAppSimulator.LicenseInformation : CurrentApp.LicenseInformation;
#else
            licenseInformation = CurrentApp.LicenseInformation;
#endif
        }

        #endregion
#else
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
#endif
    }
}