namespace Slash.Unity.Common.Platforms
{
    using UnityEngine;
    using UnityEngine.Events;

    public class InvokeOutsideEditor : MonoBehaviour
    {
        public UnityEvent Actions;

        private void Start()
        {
#if !UNITY_EDITOR
            this.Actions.Invoke();
#endif
        }
    }
}