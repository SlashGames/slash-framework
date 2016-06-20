namespace Slash.Unity.Common.Input.Handlers
{
    using System;

    using Slash.Unity.Common.Utils;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Flags]
        public enum Directions
        {
            All = 0,

            Up = 1 << 1,

            Down = 1 << 2,

            Left = 1 << 3,

            Right = 1 << 4,
        }

        #region Fields

        /// <summary>
        ///   Threshold for direction checks (in degrees).
        /// </summary>
        public float DirectionThreshold = 45.0f;

        [EnumFlag]
        public Directions ValidDirections = Directions.All;

        /// <summary>
        ///   Current drag event data.
        /// </summary>
        private PointerEventData dragPointerEventData;

        /// <summary>
        ///   Indicates if drag handler is currently handling a drag.
        /// </summary>
        private bool isDragging;

        #endregion

        #region Unity Events

        public BeginDragEvent BeginDrag;

        public DragEvent Drag;

        public EndDragEvent EndDrag;

        #endregion

        #region Public Methods and Operators

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Check if direction is correct.
            if (!this.CheckDirection(eventData))
            {
                // Reset this one as drag receiver.
                eventData.pointerDrag = null;
                return;
            }

            this.BeginDrag.Invoke(eventData.position);
            this.isDragging = true;
            this.dragPointerEventData = eventData;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!this.isDragging)
            {
                return;
            }
            
            this.Drag.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!this.isDragging)
            {
                return;
            }
            
            this.EndDrag.Invoke(eventData.position);
            this.isDragging = false;
            this.dragPointerEventData = null;
        }

        #endregion

        #region Methods

        protected void OnDisable()
        {
            if (this.isDragging)
            {
                this.OnEndDrag(this.dragPointerEventData);
            }
        }

        private bool CheckDirection(PointerEventData eventData)
        {
            if (this.ValidDirections == Directions.All)
            {
                return true;
            }

            var direction = (eventData.position - eventData.pressPosition).normalized;

            if ((this.ValidDirections & Directions.Up) != 0)
            {
                if (this.CheckDirection(direction, Vector2.up))
                {
                    return true;
                }
            }

            if ((this.ValidDirections & Directions.Down) != 0)
            {
                if (this.CheckDirection(direction, Vector2.down))
                {
                    return true;
                }
            }

            if ((this.ValidDirections & Directions.Left) != 0)
            {
                if (this.CheckDirection(direction, Vector2.left))
                {
                    return true;
                }
            }

            if ((this.ValidDirections & Directions.Right) != 0)
            {
                if (this.CheckDirection(direction, Vector2.right))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckDirection(Vector2 direction, Vector2 desiredDirection)
        {
            var directionAngle = Vector2.Angle(desiredDirection, direction);
            return directionAngle < this.DirectionThreshold;
        }

        #endregion

        [Serializable]
        public class BeginDragEvent : UnityEvent<Vector2>
        {
        }

        [Serializable]
        public class DragEvent : UnityEvent<PointerEventData>
        {
        }

        [Serializable]
        public class EndDragEvent : UnityEvent<Vector2>
        {
        }
    }
}