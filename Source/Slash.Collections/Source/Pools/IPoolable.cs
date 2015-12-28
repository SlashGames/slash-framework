// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPoolable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Pools
{
    /// <summary>
    ///   Object that can be restored to its original state for reuse.
    /// </summary>
    public interface IPoolable
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Restores this object to its original state for reuse.
        /// </summary>
        void Reset();

        #endregion
    }
}