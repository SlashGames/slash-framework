// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewEventDelegate.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ViewModels
{
    using System;

#if WINDOWS_STORE
    using Slash.Reflection.Extensions;
#endif

    using UnityEngine;

    /// <summary>
    ///   Allows registering as listener for a specific view event of a MonoBehaviour.
    /// </summary>
    /// <seealso cref="ViewEvent" />
    [Serializable]
    public class ViewEventDelegate
    {
        #region Fields

        [SerializeField]
        private string field;

        [SerializeField]
        private MonoBehaviour source;

        #endregion

        #region Public Properties

        /// <summary>
        ///   View event to register as listener for.
        /// </summary>
        public string Field
        {
            get
            {
                return this.field;
            }
            set
            {
                this.field = value;
            }
        }

        /// <summary>
        ///   Mono behaviour instance to register as listener at.
        /// </summary>
        public MonoBehaviour Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Registers the passed callback for this view event.
        /// </summary>
        /// <param name="callback">Method to call when the specified view event occurs on the target behaviour.</param>
        /// <seealso cref="Field" />
        /// <seealso cref="Source" />
        public void Register(ViewEvent.EventDelegate callback)
        {
            if (this.source == null || string.IsNullOrEmpty(this.field))
            {
                Debug.LogWarning("Can't register, source of field not set.");
                return;
            }

            // Get event property.
            ViewEvent viewEvent = (ViewEvent)this.source.GetType().GetField(this.field).GetValue(this.source);
            viewEvent.Event += callback;
        }

        #endregion
    }
}