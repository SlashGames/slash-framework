// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskParameterLocation.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.AI.BehaviorTrees.Data
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