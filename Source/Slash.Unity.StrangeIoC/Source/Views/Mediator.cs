using strange.extensions.mediation.impl;

namespace Slash.Unity.StrangeIoC.Views
{
    public abstract class Mediator<TView> : Mediator where TView : View
    {
        [Inject]
        public TView View { get; set; }
    }
}