namespace Slash.Unity.Common.Input.Handlers
{
    using UnityEngine.EventSystems;

    public interface ILongPressHandler : IEventSystemHandler
    {
        #region Public Methods and Operators

        void OnLongPress(PointerEventData eventData);

        void OnLongPressStart(PointerEventData eventData);

        void OnLongPressCancel(PointerEventData eventData);

        #endregion
    }
}