namespace Slash.Unity.Common.Input.Handlers
{
    using UnityEngine.EventSystems;

    public interface IDragOverHandler : IEventSystemHandler
    {
        #region Public Methods and Operators

        void OnDragOver(PointerEventData eventData);

        #endregion
    }
}