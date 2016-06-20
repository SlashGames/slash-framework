namespace Slash.Unity.Common.Input.Handlers
{
    using System;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class PressHandler : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPressCanceledHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        #region Fields

        [Tooltip(
            "Event that fires when control was pressed, but press was canceled. E.g. because drag of another control started."
            )]
        public PressUnityEvent CancelEvent;

        [Tooltip("Event that fires when control is initially pressed down")]
        public PressUnityEvent PressEvent;

        [Tooltip("Event that fires when control is released")]
        public PressUnityEvent ReleaseEvent;

        #endregion

        #region Properties

        /// <summary>
        ///   Indicates if game object is currently pressed.
        /// </summary>
        public bool IsPressed { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Implemented to not let drag events fall through this control.
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Implemented to not let drag events fall through this control.
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Implemented to not let drag events fall through this control.
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.OnPress();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.OnRelease();
        }

        public void OnPressCanceled(PointerEventData eventData)
        {
            this.CancelEvent.Invoke(this.gameObject);
            this.IsPressed = false;
        }

        #endregion

        #region Methods

        protected void OnDisable()
        {
            // Check if pressed.
            if (this.IsPressed)
            {
                this.OnRelease();
            }
        }

        private void OnPress()
        {
            this.PressEvent.Invoke(this.gameObject);
            this.IsPressed = true;
        }

        private void OnRelease()
        {
            this.ReleaseEvent.Invoke(this.gameObject);
            this.IsPressed = false;
        }

        #endregion

        [Serializable]
        public class PressUnityEvent : UnityEvent<GameObject>
        {
        }
    }
}