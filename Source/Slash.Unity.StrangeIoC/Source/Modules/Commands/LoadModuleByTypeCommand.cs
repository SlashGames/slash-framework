namespace Slash.Unity.StrangeIoC.Modules.Commands
{
    using System;
    using strange.extensions.command.impl;
    using strange.extensions.context.api;

    public class LoadModuleByTypeCommand : Command
    {
        [Inject(ContextKeys.CONTEXT)]
        public ModuleContext Context { get; set; }

        [Inject]
        public Type ModuleConfigType { get; set; }

        /// <inheritdoc />
        public override void Execute()
        {
            this.Context.AddSubModule(this.ModuleConfigType);
        }
    }
}