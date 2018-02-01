namespace Slash.Unity.Common.UI
{
    using UnityEngine.UI;

    /// <summary>
    ///     A concrete subclass of the Unity UI `Graphic` class that just skips drawing.
    ///     Useful for providing a raycast target without actually drawing anything.
    ///     From https://answers.unity.com/questions/1091618/ui-panel-without-image-component-as-raycast-target.html
    /// </summary>
    public class NonDrawingGraphic : Graphic
    {
        /// <inheritdoc />
        public override void SetMaterialDirty()
        {
        }

        /// <inheritdoc />
        public override void SetVerticesDirty()
        {
        }

        /// <inheritdoc />
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            // Probably not necessary since the chain of calls `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` won't happen; 
            // so here really just as a fail-safe.
            vh.Clear();
        }
    }
}