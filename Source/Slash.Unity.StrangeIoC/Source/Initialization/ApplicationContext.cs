// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using Slash.Reflection.Utils;
using Slash.Unity.Common.Scenes;
using Slash.Unity.StrangeIoC.Configs;
using Slash.Unity.StrangeIoC.Initialization.Commands;
using Slash.Unity.StrangeIoC.Initialization.Signals;
using Slash.Unity.StrangeIoC.Windows;
using UnityEngine;
using ILogger = Slash.Diagnostics.Logging.ILogger;
using Logger = Slash.Diagnostics.Logging.Logger;

namespace Slash.Unity.StrangeIoC.Initialization
{
    public class ApplicationContext : StrangeContext
    {
        public ApplicationContext(MonoBehaviour view, bool autoMapping)
            : base(view, autoMapping)
        {
        }

        public override void Launch()
        {
            base.Launch();

            var startSignal = this.injectionBinder.GetInstance<ApplicationStartSignal>();
            startSignal.Dispatch();
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();

            // Enable signals.
            this.injectionBinder.Unbind<ICommandBinder>();
            this.injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        protected override void mapBindings()
        {
            // Setup application lifecycle.
            this.injectionBinder.Bind<ApplicationStartSignal>().ToSingleton();
            this.commandBinder.Bind<ApplicationStartSignal>().To<ApplicationStartCommand>().InSequence();

            // Create window manager.
            var windowManager = new WindowManager(this.view);
            this.injectionBinder.Bind<WindowManager>().ToValue(windowManager).ToSingleton();

            // Setup windows module.
            var windowsModule = new WindowsModule();
            windowsModule.Init(this.commandBinder);

            // Setup logging.
            var configAsset = Resources.Load<TextAsset>("log4net");
            if (configAsset != null)
            {
                Logger.Configure(new MemoryStream(configAsset.bytes));
                var logger = new Logger(typeof (ILogger));
                this.injectionBinder.Bind<ILogger>().To(logger);
            }
            
            base.mapBindings();
        }
    }
}