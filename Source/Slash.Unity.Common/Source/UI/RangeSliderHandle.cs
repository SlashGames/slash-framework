namespace Slash.Unity.Common.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(RectTransform))]
    public class RangeSliderHandle : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        private Vector2 dragOffset;

        private RectTransform rectTransform;

        /// <summary>
        ///     Maximum position (in pixels).
        /// </summary>
        public int MaxPosition { get; set; }

        /// <summary>
        ///     Minimum position (in pixels).
        /// </summary>
        public int MinPosition { get; set; }

        /// <summary>
        ///     Current position (in pixels).
        /// </summary>
        public int Position
        {
            get { return Mathf.RoundToInt(this.RectTransform.anchoredPosition.x); }
            set
            {
                var newPosition = this.RectTransform.anchoredPosition;
                if (value == Mathf.RoundToInt(newPosition.x))
                {
                    return;
                }

                newPosition.x = value;

                this.RectTransform.anchoredPosition = newPosition;

                this.OnPositionChanged();
            }
        }

        private RectTransform RectTransform
        {
            get
            {
                if (this.rectTransform == null)
                {
                    this.rectTransform = this.GetComponent<RectTransform>();
                    if (this.rectTransform == null)
                    {
                        this.rectTransform = this.gameObject.AddComponent<RectTransform>();
                    }
                }

                return this.rectTransform;
            }
        }

        /// <inheritdoc />
        public void OnBeginDrag(PointerEventData eventData)
        {
            // Compute drag offset.
            var screenPosition = eventData.position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.RectTransform,
                screenPosition, eventData.pressEventCamera, out this.dragOffset);
        }

        /// <inheritdoc />
        public void OnDrag(PointerEventData eventData)
        {
            var parentTransform = this.RectTransform.parent;
            var screenPosition = eventData.position;

            Vector2 localPosition;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) parentTransform,
                screenPosition, eventData.pressEventCamera, out localPosition))
            {
                return;
            }

            // Consider offset.
            localPosition -= this.dragOffset;

            // Clamp to valid range.
            var position = Mathf.Clamp(Mathf.RoundToInt(localPosition.x), this.MinPosition, this.MaxPosition);

            this.Position = position;
        }

        public event Action PositionChanged;

        protected virtual void OnPositionChanged()
        {
            var handler = this.PositionChanged;
            if (handler != null)
            {
                handler();
            }
        }
    }
}