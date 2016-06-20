namespace Slash.Unity.Common.Input.Handlers
{
    using System;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        public HoverUnityEvent EnterEvent = new HoverUnityEvent();

        public HoverUnityEvent ExitEvent = new HoverUnityEvent();

        #endregion

        #region Public Methods and Operators

        public void OnPointerEnter(PointerEventData eventData)
        {
            // TODO(co): Check if main pointer with InputUtils.IsMainPointer when merged.
            this.EnterEvent.Invoke(this.gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // TODO(co): Check if main pointer with InputUtils.IsMainPointer when merged.
            this.ExitEvent.Invoke(this.gameObject);
        }

        #endregion

        [Serializable]
        public class HoverUnityEvent : UnityEvent<GameObject>
        {
        }
    }
}