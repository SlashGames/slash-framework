// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateEntityProcess.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Processes
{
    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Blueprints.Extensions;

    /// <summary>
    ///   Creates an entity with the specified blueprint and configuration.
    /// </summary>
    public class CreateEntityProcess : GameProcess
    {
        #region Fields

        private readonly IAttributeTable attributeTable;

        private readonly Blueprint blueprint;

        #endregion

        #region Constructors and Destructors

        public CreateEntityProcess(Blueprint blueprint)
            : this(blueprint, null)
        {
        }

        public CreateEntityProcess(Blueprint blueprint, IAttributeTable attributeTable)
        {
            this.blueprint = blueprint;
            this.attributeTable = attributeTable;
        }

        #endregion

        #region Properties

        public int CreatedEntity { get; private set; }

        #endregion

        #region Public Methods and Operators

        public override void Update(float dt)
        {
            base.Update(dt);

            this.CreatedEntity = this.EntityManager.CreateEntity(this.blueprint, this.attributeTable);

            this.Kill();
        }

        #endregion
    }
}