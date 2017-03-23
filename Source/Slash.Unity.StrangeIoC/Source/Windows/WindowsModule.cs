// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowsModule.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using strange.extensions.command.api;
using Slash.Unity.StrangeIoC.Commands;
using Slash.Unity.StrangeIoC.Windows.Signals;

namespace Slash.Unity.StrangeIoC.Windows
{
    public class WindowsModule
    {
        public void Init(ICommandBinder commandBinder)
        {
            commandBinder.Bind<CloseWindowSignal>().To<CloseWindowCommand>();
        }
    }
}