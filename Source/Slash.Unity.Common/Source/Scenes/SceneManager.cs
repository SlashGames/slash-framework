namespace Slash.Unity.Common.Scenes
{
    using UnityEngine;

    public class SceneManager : MonoBehaviour
    {
        public object InitParams { get; set; }

        public void LoadScene(string scene, object initParams)
        {
            this.InitParams = initParams;

            Application.LoadLevel(scene);
        }
    }
}