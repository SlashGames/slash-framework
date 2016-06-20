namespace Slash.Unity.Common.Input.Handlers
{
    using System;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.Serialization;

    public class ClickHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        #region Fields

        [FormerlySerializedAs("ClickEvent")]
        public ClickEvent Click = new ClickEvent();

        #endregion

        #region Public Methods and Operators

        public void OnPointerClick(PointerEventData eventData)
        {
            this.Click.Invoke(
                new ClickEventData()
                {
                    GameObject = this.gameObject,
                    ScreenPosition = eventData.position,
                    WorldPosition = eventData.pointerCurrentRaycast.worldPosition
                });
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        #endregion

        public class ClickEventData
        {
            #region Properties

            /// <summary>
            ///   Clicked game object.
            /// </summary>
            public GameObject GameObject { get; set; }

            /// <summary>
            ///   Screen position of click.
            /// </summary>
            public Vector2 ScreenPosition { get; set; }

            /// <summary>
            ///   World position of click.
            /// </summary>
            public Vector3 WorldPosition { get; set; }

            #endregion
        }

        [Serializable]
        public class ClickEvent : UnityEvent<ClickEventData>
        {
        }
    }
}