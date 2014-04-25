// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tests.ECS
{
    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase;
    using Slash.GameBase.Systems;

    public class SystemTest<T>
        where T : ISystem, new()
    {
        #region Fields

        protected AttributeTable GameConfiguration;

        protected Game TestGame;

        #endregion
        
        #region Public Methods and Operators

        [SetUp]
        public virtual void SetUp()
        {
            this.GameConfiguration = new AttributeTable();
            this.TestGame = new Game();
            this.TestGame.AddSystem<T>();
        }

        #endregion

        #region Methods

        protected void StartGame()
        {
            this.TestGame.StartGame(this.GameConfiguration);
        }

        #endregion
    }
}