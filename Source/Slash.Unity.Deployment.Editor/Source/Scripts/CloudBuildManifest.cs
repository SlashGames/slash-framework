using System.Collections.Generic;

namespace Slash.Unity.Deployment.Editor
{
    // usings go INSIDE the namespace so they don't conflict with the rest

#if UNITY_CLOUD_BUILD
    using UnityEngine.CloudBuild;
#endif

    /// <summary>
    ///     API against Unity's build manifest object. Works locally (on a mock dictionary) and in the cloud.
    /// </summary>
    public class CloudBuildManifest
    {
#if UNITY_CLOUD_BUILD
        private BuildManifestObject buildManifestObject;
#else
        private readonly Dictionary<string, string> contents;
#endif

#if UNITY_CLOUD_BUILD
        public CloudBuildManifest(BuildManifestObject buildManifestObject)
        {
            this.buildManifestObject = buildManifestObject;
        }
#else

        public CloudBuildManifest()
        {
            this.contents = new Dictionary<string, string>();
        }

#endif

        public string ScmCommitId
        {
            get { return this.Get("scmCommitId", null); }
            set { this.Set("scmCommitId", value); }
        }

        public string ScmBranch
        {
            get { return this.Get("scmBranch", null); }
            set { this.Set("scmBranch", value); }
        }

        public string BuildNumber
        {
            get { return this.Get("buildNumber", null); }
            set { this.Set("buildNumber", value); }
        }

        public string BuildStartTime
        {
            get { return this.Get("buildStartTime", null); }
            set { this.Set("buildStartTime", value); }
        }

        public string ProjectId
        {
            get { return this.Get("projectId", null); }
            set { this.Set("projectId", value); }
        }

        public string BundleId
        {
            get { return this.Get("bundleId", null); }
            set { this.Set("bundleId", value); }
        }

        public string UnityVersion
        {
            get { return this.Get("unityVersion", null); }
            set { this.Set("unityVersion", value); }
        }

        public string XCodeVersion
        {
            get { return this.Get("xCodeVersion", null); }
            set { this.Set("xCodeVersion", value); }
        }

        public string CloudBuildTargetName
        {
            get { return this.Get("cloudBuildTargetName", null); }
            set { this.Set("cloudBuildTargetName", value); }
        }

        public string Get(string key, string defaultValue)
        {
#if UNITY_CLOUD_BUILD
            return buildManifestObject.GetValue(key, defaultValue);
#else
            string result;
            if (!this.contents.TryGetValue(key, out result))
            {
                return defaultValue;
            }
            return result;
#endif
        }

        public void Set(string key, string value)
        {
#if UNITY_CLOUD_BUILD
            buildManifestObject.SetValue(key, value);
#else
            this.contents[key] = value;
#endif
        }
    }
}