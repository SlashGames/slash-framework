namespace Slash.Unity.Common.UI.Layouting
{
    using UnityEngine;

    public class AvoidOverlappingRectangle : MonoBehaviour
    {
        /// <summary>
        ///     Preferred position of rectangle during layouting (in pixels).
        /// </summary>
        [Tooltip("Preferred position of item during layouting")]
        public Vector2 PreferredPosition;
    }
}