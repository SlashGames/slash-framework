// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewEventDelegate.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ViewModels
{
    using System;

    using UnityEngine;
    
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