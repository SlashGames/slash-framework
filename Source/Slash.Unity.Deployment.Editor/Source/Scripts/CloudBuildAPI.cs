using System;
using UnityEngine;

namespace Slash.Unity.Deployment.Editor
{
#if UNITY_CLOUD_BUILD

    using UnityEditor;
    using UnityEngine.CloudBuild;

#endif

    public static class CloudBuildAPI
    {
        public static CloudBuildManifest GetBuildManifest()
        {
#if UNITY_CLOUD_BUILD
            var manifest =
(BuildManifestObject)AssetDatabase.LoadAssetAtPath("Assets/__UnityCloud__/Resources/UnityCloudBuildManifest.scriptable.asset", typeof(BuildManifestObject));
            return new CloudBuildManifest(manifest);
#else
            var result = new CloudBuildManifest();
            // add a few dummy values
            result.ProjectId = "com.yourproject.id[Local Dummy]";
            result.ScmBranch = "master[Local Dummy]";
            result.ScmCommitId = "da39a3ee5e6b4b0d3255bfef95601890afd80709[Local Dummy]";
            result.UnityVersion = Application.unityVersion;
            result.XCodeVersion = "5.5 [Local Dummy]";
            result.BuildNumber = "182 [Local Dummy]";
            result.BuildStartTime = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString();
            result.BundleId = "com.yourbundle.id[Local Dummy]";
            return result;
#endif
        }
    }
}