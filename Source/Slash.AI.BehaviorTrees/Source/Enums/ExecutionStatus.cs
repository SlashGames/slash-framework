// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionStatus.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Enums
{
    /// <summary>
    ///   The execution status.
    /// </summary>
    public enum ExecutionStatus
    {
        /// <summary>
        ///   Undefined/Invalid status.
        /// </summary>
        None,

        /// <summary>
        ///   Task is currently running.
        /// </summary>
        Running,

        /// <summary>
        ///   Task execution succeeded.
        /// </summary>
        Success,

        /// <summary>
        ///   Task execution failed.
        /// </summary>
        Failed,
    }
}