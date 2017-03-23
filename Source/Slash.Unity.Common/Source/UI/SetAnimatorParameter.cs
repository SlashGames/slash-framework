namespace Slash.Unity.Common.UI
{
    using UnityEngine;

    public abstract class SetAnimatorParameter<TValue> : MonoBehaviour
    {
        public Animator Animator;

        public string Parameter;

        public void SetValue(TValue value)
        {
            if (this.Animator == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.Parameter))
            {
                return;
            }

            this.InternalSetValue(value);
        }

        protected abstract void InternalSetValue(TValue value);
    }
}