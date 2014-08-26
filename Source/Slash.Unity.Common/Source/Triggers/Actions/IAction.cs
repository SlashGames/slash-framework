// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAction.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Triggers.Actions
{
    /// <summary>
    ///   Generic action that can be triggered if a set of conditions is fulfilled.
    /// </summary>
    public interface IAction
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Executes this action.
        /// </summary>
        /// <param name="actionArgs">Action parameters.</param>
        void Execute(object actionArgs);

        #endregion
    }
}