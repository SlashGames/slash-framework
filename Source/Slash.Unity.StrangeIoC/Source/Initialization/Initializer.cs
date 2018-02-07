// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Initializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.StrangeIoC.Initialization
{
    using Slash.Unity.StrangeIoC.Modules;
    public class Initializer : ModuleView
    {
        private void Awake()
        {
            // Create and start application context.
            var applicationContext = new ApplicationContext();
            applicationContext.Init(this);
            applicationContext.Start();
            applicationContext.SetContextView(this);

            this.context = applicationContext;
        }
    }
}