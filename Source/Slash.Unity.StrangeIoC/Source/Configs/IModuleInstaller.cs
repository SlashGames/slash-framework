namespace Slash.Unity.StrangeIoC.Configs
{
    using System;
    using System.Collections.Generic;
    using strange.extensions.command.api;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;

    public interface IModuleInstaller
    {
        /// <summary>
        ///     Bridges required by this module.
        /// </summary>
        IEnumerable<Type> Bridges { get; }

        /// <summary>
        ///     Scene to load for module.
        /// </summary>
        string SceneName { get; }

        /// <summary>
        ///     Sub modules required by this module.
        /// </summary>
        IEnumerable<IModuleInstaller> SubModules { get; }

        /// <summary>
        ///     Sub modules required by this module.
        /// </summary>
        IEnumerable<Type> SubModuleTypes { get; }

        /// <summary>
        ///     Maps bindings to the injection binder.
        /// </summary>
        /// <param name="injectionBinder">Binder to map to.</param>
        void MapBindings(IInjectionBinder injectionBinder);

        /// <summary>
        ///     Maps bindings to the command binder.
        /// </summary>
        /// <param name="commandBinder">Binder to map to.</param>
        void MapBindings(ICommandBinder commandBinder);

        /// <summary>
        ///     Maps bindings to the mediation binder.
        /// </summary>
        /// <param name="mediationBinder">Binder to map to.</param>
        void MapBindings(IMediationBinder mediationBinder);

        /// <summary>
        ///     Unmaps bindings to the injection binder.
        ///     Only cross context bindings have to be removed, all others are just deleted.
        /// </summary>
        /// <param name="injectionBinder">Binder to modify.</param>
        void UnmapCrossContextBindings(IInjectionBinder injectionBinder);
    }
}