namespace Slash.Unity.Common.UI
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(RectTransform))]
    public class RangeSlider : MonoBehaviour
    {
        public RectTransform ActiveAreaIndicator;

        public GameObject MaxHandle;

        public GameObject MinHandle;

        /// <summary>
        ///     Initial position for maximum handle (0-1).
        /// </summary>
        private float initialValueMax = 1;

        /// <summary>
        ///     Initial position for minimum handle (0-1).
        /// </summary>
        private float initialValueMin;

        private RangeSliderHandle rangeHandleMax;

        private RangeSliderHandle rangeHandleMin;

        private RectTransform rectTransform;

        /// <summary>
        ///     Maximum value (as ratio from 0 to 1).
        /// </summary>
        public float MaxValue
        {
            get { return this.rangeHandleMax != null ? this.PositionToRatio(this.rangeHandleMax.Position) : 1; }
            set
            {
                if (this.rangeHandleMax != null)
                {
                    this.rangeHandleMax.Position = this.RatioToPosition(value);
                }
                else
                {
                    this.initialValueMax = value;
                }
            }
        }

        /// <summary>
        ///     Minimum value (as ratio from 0 to 1).
        /// </summary>
        public float MinValue
        {
            get { return this.rangeHandleMin != null ? this.PositionToRatio(this.rangeHandleMin.Position) : 0; }
            set
            {
                if (this.rangeHandleMin != null)
                {
                    this.rangeHandleMin.Position = this.RatioToPosition(value);
                }
                else
                {
                    this.initialValueMin = value;
                }
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

        private float Width
        {
            get { return this.RectTransform.rect.width; }
        }

        public event Action MaxValueChanged;

        public event Action MinValueChanged;

        protected void OnDisable()
        {
            // Store values to restore when enabled again.
            this.initialValueMin = this.MinValue;
            this.initialValueMax = this.MaxValue;

            this.UnregisterHandle(ref this.rangeHandleMin, this.OnMinValueChanged);
            this.UnregisterHandle(ref this.rangeHandleMax, this.OnMaxValueChanged);
        }

        protected void OnEnable()
        {
            // Setup handles.
            this.rangeHandleMin = this.RegisterHandle(this.MinHandle, this.OnMinValueChanged);
            this.rangeHandleMax = this.RegisterHandle(this.MaxHandle, this.OnMaxValueChanged);

            this.rangeHandleMin.MinPosition = this.RatioToPosition(0);
            this.rangeHandleMax.MaxPosition = this.RatioToPosition(1);

            this.MinValue = this.initialValueMin;
            this.MaxValue = this.initialValueMax;

            // Setup active area.
            if (this.ActiveAreaIndicator != null)
            {
                this.ActiveAreaIndicator.anchorMin = new Vector2(0, 0.5f);
                this.ActiveAreaIndicator.anchorMax = new Vector2(1, 0.5f);
            }

            this.UpdateHandleRanges();
        }

        protected virtual void OnMaxValueChanged()
        {
            this.UpdateActiveArea();
            this.UpdateHandleRanges();

            var handler = this.MaxValueChanged;
            if (handler != null)
            {
                handler();
            }
        }

        protected virtual void OnMinValueChanged()
        {
            this.UpdateActiveArea();
            this.UpdateHandleRanges();

            var handler = this.MinValueChanged;
            if (handler != null)
            {
                handler();
            }
        }

        private float PositionToRatio(int position)
        {
            return 0.5f + position / this.Width;
        }

        private int RatioToPosition(float ratio)
        {
            return Mathf.RoundToInt((ratio - 0.5f) * this.Width);
        }

        private RangeSliderHandle RegisterHandle(GameObject handleGameObject, Action valueChangedCallback)
        {
            var handle = handleGameObject.GetComponent<RangeSliderHandle>();
            if (handle == null)
            {
                handle = handleGameObject.AddComponent<RangeSliderHandle>();
            }

            var handleRectTransform = handleGameObject.GetComponent<RectTransform>();
            if (handleRectTransform == null)
            {
                handleRectTransform = handleGameObject.AddComponent<RectTransform>();
            }

            // Setup anchoring of handle.
            handleRectTransform.anchorMin = handleRectTransform.anchorMax = new Vector2(0.5f, 0.5f);

            handle.PositionChanged += valueChangedCallback;

            return handle;
        }

        private void UnregisterHandle(ref RangeSliderHandle rangeSliderHandle, Action valueChangedCallback)
        {
            if (rangeSliderHandle != null)
            {
                rangeSliderHandle.PositionChanged -= valueChangedCallback;
                rangeSliderHandle = null;
            }
        }

        private void UpdateActiveArea()
        {
            if (this.ActiveAreaIndicator == null)
            {
                return;
            }

            var offsetMin = this.ActiveAreaIndicator.offsetMin;
            offsetMin.x = this.rangeHandleMin.Position + this.Width * 0.5f;
            this.ActiveAreaIndicator.offsetMin = offsetMin;

            var offsetMax = this.ActiveAreaIndicator.offsetMax;
            offsetMax.x = this.rangeHandleMax.Position - this.Width * 0.5f;
            this.ActiveAreaIndicator.offsetMax = offsetMax;
        }

        private void UpdateHandleRanges()
        {
            this.rangeHandleMax.MinPosition = this.rangeHandleMin.Position;
            this.rangeHandleMin.MaxPosition = this.rangeHandleMax.Position;
        }
    }

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