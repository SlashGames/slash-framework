namespace Slash.Unity.Common.Input.Handlers
{
    using System;

    using UnityEngine;
    using UnityEngine.Events;

    public class PinchHandler : MonoBehaviour, IPinchHandler
    {
        #region Fields

        public PinchEvent Pinch;

        #endregion

        #region Public Methods and Operators

        public void OnPinch(PinchEventData eventData)
        {
            this.Pinch.Invoke(eventData);
        }

        #endregion

        [Serializable]
        public class PinchEvent : UnityEvent<PinchEventData>
        {
        }
    }
}