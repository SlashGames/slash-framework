namespace Slash.Unity.Common.Input.Handlers
{
    using UnityEngine.EventSystems;

    public interface IPressCanceledHandler : IEventSystemHandler
    {
        #region Public Methods and Operators

        void OnPressCanceled(PointerEventData eventData);

        #endregion
    }
}