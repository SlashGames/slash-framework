namespace Slash.Unity.Common.Input.Handlers
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public interface IPinchHandler : IEventSystemHandler
    {
        #region Public Methods and Operators

        void OnPinch(PinchEventData eventData);

        #endregion
    }

    public class PinchEventData : BaseEventData
    {
        #region Constructors and Destructors

        public PinchEventData(EventSystem eventSystem)
            : base(eventSystem)
        {
        }

        #endregion

        #region Properties

        public float Delta { get; set; }

        public GameObject PinchHandler { get; set; }

        public Vector2 TargetPosition { get; set; }

        #endregion
    }
}