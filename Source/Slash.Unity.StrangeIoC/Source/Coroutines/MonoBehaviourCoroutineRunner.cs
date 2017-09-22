namespace Slash.Unity.StrangeIoC.Coroutines
{
    using System.Collections;
    using UnityEngine;

    public class MonoBehaviourCoroutineRunner : ICoroutineRunner
    {
        private readonly MonoBehaviour monoBehaviour;

        public MonoBehaviourCoroutineRunner(MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour;
        }
        
        /// <inheritdoc />
        public void StartCoroutine(IEnumerator routine)
        {
            this.monoBehaviour.StartCoroutine(routine);
        }
    }
}