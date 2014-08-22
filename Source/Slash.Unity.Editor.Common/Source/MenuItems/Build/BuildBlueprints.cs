// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildBlueprints.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Build
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    using Slash.ECS.Blueprints;
    using Slash.Serialization.Binary;

    using UnityEditor;

    using UnityEngine;

    public class BuildBlueprints
    {
        #region Constants

        public const string BlueprintFileExtensionBinary = ".bytes";

        public const string BlueprintFileExtensionTemp = ".tmp";

        public const string BlueprintFileExtensionXml = ".xml";

        public const string RelativeBlueprintsFolderPath = "Resources/Blueprints";

        #endregion

        #region Public Properties

        public static string AbsoluteBlueprintsFolderPath
        {
            get
            {
                return String.Format("{0}/{1}", Application.dataPath, RelativeBlueprintsFolderPath);
            }
        }

        #endregion

        #region Public Methods and Operators

        [MenuItem("Slash Games/Blueprints/Restore Xml Blueprints")]
        public static void RestoreXmlBlueprints()
        {
            // Delete binary blueprint files.
            foreach (var filename in Directory.GetFiles(AbsoluteBlueprintsFolderPath))
            {
                var file = new FileInfo(filename);

                if (file.Extension.Equals(BlueprintFileExtensionBinary))
                {
                    Debug.Log(String.Format("Deleting binary file {0}", file.FullName));
                    FileUtil.DeleteFileOrDirectory(file.FullName);

                    Debug.Log(String.Format("Deleting binary meta file {0}", file.FullName + ".meta"));
                    FileUtil.DeleteFileOrDirectory(file.FullName + ".meta");
                }
            }

            // Restore XML blueprint files.
            foreach (var filename in Directory.GetFiles(AbsoluteBlueprintsFolderPath))
            {
                var file = new FileInfo(filename);

                if (file.Extension.Equals(BlueprintFileExtensionTemp))
                {
                    var filenameWithoutTempExtension = Path.GetDirectoryName(file.FullName) + "/"
                                                       + Path.GetFileNameWithoutExtension(file.FullName);

                    Debug.Log(String.Format("Moving {0} to {1}", file.FullName, filenameWithoutTempExtension));
                    FileUtil.MoveFileOrDirectory(file.FullName, filenameWithoutTempExtension);
                }
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Slash Games/Blueprints/Switch To Binary Blueprints")]
        public static void SwitchToBinaryBlueprints()
        {
            // Remove old backup files if xml file also exists.
            // Keeping it if not, as this may be the second time the "SwitchToBinaryBlueprints" method was called.
            foreach (var filename in Directory.GetFiles(AbsoluteBlueprintsFolderPath))
            {
                var file = new FileInfo(filename);
                if (!file.Extension.Equals(BlueprintFileExtensionTemp))
                {
                    continue;
                }

                if (!File.Exists(Path.ChangeExtension(filename, null)))
                {
                    continue;
                }

                Debug.Log(String.Format("Deleting temp file {0}", file));
                FileUtil.DeleteFileOrDirectory(file.FullName);
            }

            // Collect XML blueprint files.
            foreach (var filename in Directory.GetFiles(AbsoluteBlueprintsFolderPath))
            {
                var file = new FileInfo(filename);

                if (file.Extension.Equals(BlueprintFileExtensionXml))
                {
                    BlueprintManager blueprintManager;

                    // Deserialize blueprints from XML.
                    using (var fileStream = file.OpenRead())
                    {
                        Debug.Log(String.Format("Deserializing blueprints from {0}", file.FullName));
                        var blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));
                        blueprintManager = (BlueprintManager)blueprintManagerSerializer.Deserialize(fileStream);
                    }

                    // Rename to temporary files.
                    Debug.Log(
                        String.Format("Moving {0} to {1}", file.FullName, file.FullName + BlueprintFileExtensionTemp));
                    FileUtil.MoveFileOrDirectory(file.FullName, file.FullName + BlueprintFileExtensionTemp);

                    Debug.Log(
                        String.Format(
                            "Moving {0} to {1}",
                            file.FullName + ".meta",
                            file.FullName + ".meta" + BlueprintFileExtensionTemp));
                    FileUtil.MoveFileOrDirectory(
                        file.FullName + ".meta", file.FullName + ".meta" + BlueprintFileExtensionTemp);

                    // Serialize blueprints to binary.
                    var binaryBlueprintFile =
                        new FileInfo(Path.ChangeExtension(file.FullName, BlueprintFileExtensionBinary));

                    using (var fileStream = binaryBlueprintFile.Create())
                    {
                        Debug.Log(String.Format("Serializing blueprints to {0}", binaryBlueprintFile.FullName));
                        var binarySerializer = new BinarySerializer(fileStream);
                        binarySerializer.Serialize(blueprintManager);
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        #endregion
    }
}