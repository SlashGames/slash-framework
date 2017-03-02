// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrangeConfig.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.StrangeIoC.Configs
{
    using strange.extensions.command.api;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;

    public class StrangeConfig
    {
        public virtual void MapBindings(IInjectionBinder injectionBinder)
        {
        }

        public virtual void MapBindings(ICommandBinder commandBinder)
        {
        }

        public virtual void MapBindings(IMediationBinder mediationBinder)
        {
        }
    }
}