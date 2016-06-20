namespace Slash.Unity.Common.Input.Handlers
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class BlockAllInput : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler,
        IBeginDragHandler,
        IInitializePotentialDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IScrollHandler,
        IUpdateSelectedHandler,
        ISelectHandler,
        IDeselectHandler,
        IMoveHandler,
        ISubmitHandler,
        ICancelHandler,
        ILongPressHandler,
        IDragOverHandler,
        IPinchHandler
    {
        #region Public Methods and Operators

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnCancel(BaseEventData eventData)
        {
        }

        public void OnDeselect(BaseEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnDragOver(PointerEventData eventData)
        {
        }

        public void OnDrop(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
        }

        public void OnLongPress(PointerEventData eventData)
        {
        }

        public void OnLongPressStart(PointerEventData eventData)
        {
        }

        public void OnLongPressCancel(PointerEventData eventData)
        {
        }

        public void OnMove(AxisEventData eventData)
        {
        }

        public void OnPinch(PinchEventData eventData)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        public void OnScroll(PointerEventData eventData)
        {
        }

        public void OnSelect(BaseEventData eventData)
        {
        }

        public void OnSubmit(BaseEventData eventData)
        {
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
        }

        #endregion
    }
}