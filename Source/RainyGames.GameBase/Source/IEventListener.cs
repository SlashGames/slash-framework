// -----------------------------------------------------------------------
// <copyright file="IEventListener.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    /// <summary>
    /// Contract that all event listeners have to fulfill, making them able to
    /// be notified of any events they're interested in.
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Notifies this listener of the passed event, passing along any
        /// additional interesting information.
        /// </summary>
        /// <param name="e">
        /// Event to notify of.
        /// </param>
        void Notify(Event e);
    }
}
