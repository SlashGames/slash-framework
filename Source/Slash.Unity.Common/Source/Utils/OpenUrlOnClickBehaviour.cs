// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenUrlOnClickBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FreudBot.Unity.Ads
{
    using UnityEngine;

    public class OpenUrlOnClickBehaviour : MonoBehaviour
    {
        #region Fields

        public string Url;

        #endregion

        #region Methods

        private void OnClick()
        {
            Debug.Log("Opening URL: " + this.Url);
            Application.OpenURL(this.Url);
        }

        #endregion
    }
}