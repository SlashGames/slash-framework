using UnityEditor;
using UnityEngine;

namespace Slash.Unity.Deployment.Editor
{
    public class CloudBuildHooks : MonoBehaviour
    {
        /// <summary>
        ///   Executed before a Unity cloud build is started.
        ///   Sets the build number (iOS) and bundle version code (Android) to the Unity cloud build number.
        /// </summary>
#if UNITY_CLOUD_BUILD
        public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest)
#else
        public static void PreExport()
#endif
        {
#if UNITY_CLOUD_BUILD
            var cloudBuildManifest = new CloudBuildManifest(manifest);
#else
            var cloudBuildManifest = new CloudBuildManifest();
#endif
            PlayerSettings.iOS.buildNumber = cloudBuildManifest.BuildNumber;
            int buildNumber;
            int.TryParse(cloudBuildManifest.BuildNumber, out buildNumber);
            PlayerSettings.Android.bundleVersionCode = buildNumber;
        }
    }
}