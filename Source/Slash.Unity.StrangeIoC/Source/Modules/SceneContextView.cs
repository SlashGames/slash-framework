namespace Slash.Unity.StrangeIoC.Modules
{
    using strange.extensions.context.impl;

    /// <summary>
    ///   Put on root game objects of a scene to give them a reference to a context.
    /// </summary>
    public class SceneContextView : ContextView
    {
        /// <inheritdoc />
        protected override void OnDestroy()
        {
        }
    }
}