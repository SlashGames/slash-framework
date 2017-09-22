namespace Slash.Unity.StrangeIoC.Coroutines
{
    using System.Collections;

    public interface ICoroutineRunner
    {
        void StartCoroutine(IEnumerator routine);
    }
}