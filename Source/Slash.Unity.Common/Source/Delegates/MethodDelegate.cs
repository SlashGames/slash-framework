namespace Slash.Unity.Common.Delegates
{
    using System;

    using Slash.Reflection.Utils;

    using UnityEngine;

    [Serializable]
    public class MethodDelegate
    {
        #region Fields

        public string Method;

        public MonoBehaviour Target;

        private Delegate action;

        #endregion

        #region Public Methods and Operators

        public void Invoke(params object[] args)
        {
            if (this.action == null)
            {
                var method = ReflectionUtils.GetMethod(this.Target.GetType(), this.Method);
                this.action = ReflectionUtils.CreateDelegate(typeof(Action), this.Target, method);
            }

            this.action.DynamicInvoke(args);
        }

        #endregion
    }
}