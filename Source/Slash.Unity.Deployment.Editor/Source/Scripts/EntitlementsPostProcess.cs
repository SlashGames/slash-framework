using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Slash.Unity.Deployment.Editor
{
#if UNITY_IOS
    using UnityEditor.iOS.Xcode;
    using System.IO;
#endif

    public class EntitlementsPostProcess : ScriptableObject
    {
        public DefaultAsset EntitlementsFile;

        [PostProcessBuild]
        public static void OnPostProcess(BuildTarget buildTarget, string buildPath)
        {
            if (buildTarget != BuildTarget.iOS)
            {
                return;
            }

#if UNITY_IOS
            var dummy = ScriptableObject.CreateInstance<EntitlementsPostProcess>();
            var file = dummy.EntitlementsFile;
            ScriptableObject.DestroyImmediate(dummy);
            if (file == null)
            {
                return;
            }

            var proj_path = PBXProject.GetPBXProjectPath(buildPath);
            var proj = new PBXProject();
            proj.ReadFromFile(proj_path);

            // target_name = "Unity-iPhone"
            var target_name = PBXProject.GetUnityTargetName();
            var target_guid = proj.TargetGuidByName(target_name);
            var src = AssetDatabase.GetAssetPath(file);
            var file_name = Path.GetFileName(src);
            var dst = buildPath + "/" + target_name + "/" + file_name;
            FileUtil.CopyFileOrDirectory(src, dst);
            proj.AddFile(target_name + "/" + file_name, file_name);
            proj.AddBuildProperty(target_guid, "CODE_SIGN_ENTITLEMENTS", target_name + "/" + file_name);

            proj.WriteToFile(proj_path);
#endif
        }
    }
}
