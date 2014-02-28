// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAction.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.Unity.Common.Triggers.Actions
{
    public interface IAction
    {
        void Execute(object actionArgs);
    }
}