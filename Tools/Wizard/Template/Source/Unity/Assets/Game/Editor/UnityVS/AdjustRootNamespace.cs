namespace {GAME}.Unity.Editor.UnityVS
{
    using Slash.Unity.UnityVSExt.Editor;

    using SyntaxTree.VisualStudio.Unity.Bridge;

    using UnityEditor;

    [InitializeOnLoad]
    public class AdjustRootNamespace
    {
        private const string Namespace = "{GAME}.Unity";

        static AdjustRootNamespace()
        {
            ProjectFilesGenerator.ProjectFileGeneration += UnityVSUtils.AdjustRootNamespace(Namespace);
        }
    }
}