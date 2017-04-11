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

    using UnityEngine.SceneManagement;

    public class StrangeConfig
    {
        /// <summary>
        ///   Scene to load for this module.
        /// </summary>
        protected string SceneName { get; set; }

        /// <summary>
        ///   Maps bindings to the injection binder.
        /// </summary>
        /// <param name="injectionBinder">Binder to map to.</param>
        public virtual void MapBindings(IInjectionBinder injectionBinder)
        {
        }

        /// <summary>
        ///   Maps bindings to the command binder.
        /// </summary>
        /// <param name="commandBinder">Binder to map to.</param>
        public virtual void MapBindings(ICommandBinder commandBinder)
        {
        }

        /// <summary>
        ///   Maps bindings to the mediation binder.
        /// </summary>
        /// <param name="mediationBinder">Binder to map to.</param>
        public virtual void MapBindings(IMediationBinder mediationBinder)
        {
        }

        /// <summary>
        ///   Sets up the view for this module.
        /// </summary>
        public virtual void SetupView()
        {
            if (!string.IsNullOrEmpty(this.SceneName))
            {
                var scene = SceneManager.GetSceneByName(this.SceneName);

                // Only load if not already loaded (e.g. in editor)
                if (!scene.isLoaded)
                {
                    SceneManager.LoadSceneAsync(this.SceneName, LoadSceneMode.Additive);
                }
            }
        }
    }
}