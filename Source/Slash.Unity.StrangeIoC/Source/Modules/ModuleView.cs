namespace Slash.Unity.StrangeIoC.Modules
{
    using strange.extensions.context.impl;

    /// <summary>
    ///   Put on root game object of module.
    /// </summary>
    public class ModuleView : ContextView
    {
        /// <inheritdoc />
        protected override void OnDestroy()
        {
        }
    }
}