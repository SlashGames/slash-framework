namespace Slash.Unity.StrangeIoC.Modules.Commands
{
    using strange.extensions.command.impl;
    using strange.extensions.context.api;
    using Slash.Unity.StrangeIoC.Configs;

    public class LoadConfigCommand : Command
    {
        [Inject(ContextKeys.CONTEXT)]
        public ModuleContext Context { get; set; }

        [Inject]
        public StrangeConfig ModuleConfig { get; set; }

        /// <inheritdoc />
        public override void Execute()
        {
            this.Context.AddSubModule(this.ModuleConfig);
        }
    }
}