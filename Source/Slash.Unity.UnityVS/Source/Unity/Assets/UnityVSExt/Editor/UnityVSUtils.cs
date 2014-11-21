namespace Slash.Unity.UnityVSExt.Editor
{
    using SyntaxTree.VisualStudio.Unity.Bridge;

    public class UnityVSUtils
    {
        #region Public Methods and Operators

        public static FileGenerationHandler AdjustRootNamespace(string rootNamespace)
        {
            return
                (name, content) =>
                content.Replace(
                    "<RootNamespace></RootNamespace>",
                    string.Format("<RootNamespace>{0}</RootNamespace>", rootNamespace));
        }

        #endregion
    }
}