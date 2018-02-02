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

        private RangeSliderHandle rangeHandleMax;

        private RangeSliderHandle rangeHandleMin;

        private RectTransform rectTransform;

        protected void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
        }

        protected void OnDisable()
        {
            this.UnregisterHandle(ref this.rangeHandleMin, this.OnMinValueChanged);
            this.UnregisterHandle(ref this.rangeHandleMax, this.OnMaxValueChanged);
        }

        protected void OnEnable()
        {
            var size = this.rectTransform.rect.size.x;

            // Setup handles.
            this.rangeHandleMin = this.RegisterHandle(this.MinHandle, this.OnMinValueChanged);
            this.rangeHandleMax = this.RegisterHandle(this.MaxHandle, this.OnMaxValueChanged);
            
            this.rangeHandleMin.MinPosition = -size * 0.5f;
            this.rangeHandleMin.Position = -size * 0.5f;
            this.rangeHandleMax.MaxPosition = size * 0.5f;
            this.rangeHandleMax.Position = size * 0.5f;

            // Setup active area.
            if (this.ActiveAreaIndicator != null)
            {
                this.ActiveAreaIndicator.anchorMin = new Vector2(0, 0.5f);
                this.ActiveAreaIndicator.anchorMax = new Vector2(1, 0.5f);
            }

            this.UpdateHandleRanges();
        }

        private void OnMaxValueChanged()
        {
            this.UpdateActiveArea();
            this.UpdateHandleRanges();
        }

        private void OnMinValueChanged()
        {
            this.UpdateActiveArea();
            this.UpdateHandleRanges();
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
            if (this.ActiveAreaIndicator != null)
            {
                var size = this.rectTransform.rect.size.x;

                var offsetMin = this.ActiveAreaIndicator.offsetMin;
                offsetMin.x = this.rangeHandleMin.Position + size * 0.5f;
                this.ActiveAreaIndicator.offsetMin = offsetMin;

                var offsetMax = this.ActiveAreaIndicator.offsetMax;
                offsetMax.x = this.rangeHandleMax.Position - size * 0.5f;
                this.ActiveAreaIndicator.offsetMax = offsetMax;
            }
        }

        private void UpdateHandleRanges()
        {
            this.rangeHandleMax.MinPosition = this.rangeHandleMin.Position;
            this.rangeHandleMin.MaxPosition = this.rangeHandleMax.Position;
        }
    }

    [RequireComponent(typeof(RectTransform))]
    public class RangeSliderHandle : MonoBehaviour, IDragHandler
    {
        private RectTransform rectTransform;

        public float MaxPosition { get; set; }

        public float MinPosition { get; set; }

        public float Position
        {
            get { return this.rectTransform.anchoredPosition.x; }
            set
            {
                var newPosition = this.rectTransform.anchoredPosition;
                if (value == newPosition.x)
                {
                    return;
                }

                newPosition.x = value;

                this.rectTransform.anchoredPosition = newPosition;

                this.OnPositionChanged();
            }
        }

        /// <inheritdoc />
        public void OnDrag(PointerEventData eventData)
        {
            var parentTransform = this.rectTransform.parent;
            var screenPosition = eventData.position;

            Vector2 localPosition;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) parentTransform,
                screenPosition, eventData.pressEventCamera, out localPosition))
            {
                return;
            }

            // Clamp to valid range.
            localPosition.x = Mathf.Clamp(localPosition.x, this.MinPosition, this.MaxPosition);

            this.Position = localPosition.x;
        }

        public event Action PositionChanged;

        protected void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
        }

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