// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Initializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using strange.extensions.context.impl;

namespace Slash.Unity.StrangeIoC.Initialization
{
    public class Initializer : ContextView
    {
        private void Awake()
        {
            // Create and start application context.
            var applicationContext = new ApplicationContext();
            applicationContext.Init();
            applicationContext.Start();
            applicationContext.SetContextView(this);

            this.context = applicationContext;
        }
    }
}