// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tests.ECS
{
    using NUnit.Framework;

    using Slash.GameBase;
    using Slash.GameBase.Systems;

    public class SystemTest<T>
        where T : ISystem, new()
    {
        #region Fields

        protected Game TestGame;

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public virtual void SetUp()
        {
            this.TestGame = new Game();

            this.TestGame.AddSystem<T>();

            this.TestGame.InitGame();
            this.TestGame.StartGame(null);
        }

        #endregion
    }
}