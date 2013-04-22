// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskParameterLocation.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.AI.BehaviorTrees.Data
{
    /// <summary>
    ///   Locations to take a task parameter value from.
    /// </summary>
    public enum TaskParameterLocation
    {
        /// <summary>
        ///   The invalid.
        /// </summary>
        Invalid,

        /// <summary>
        ///   The user value.
        /// </summary>
        UserValue,

        /// <summary>
        ///   The blackboard.
        /// </summary>
        Blackboard,
    }
}