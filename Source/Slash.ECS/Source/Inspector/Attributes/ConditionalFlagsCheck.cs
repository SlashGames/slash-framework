// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionalFlagsCheck.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Attributes
{
    /// <summary>
    ///   Indicates how the flags are checked.
    /// </summary>
    public enum ConditionalFlagsCheck
    {
        /// <summary>
        ///   All flags of the RequiredConditionValue enum have to be set.
        /// </summary>
        AllSet,

        /// <summary>
        ///   At least one flag of the RequiredConditionValue enum have to be set.
        /// </summary>
        OneSet,
    }
}