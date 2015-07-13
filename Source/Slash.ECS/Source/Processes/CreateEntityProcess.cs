// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateEntityProcess.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Processes
{
    using Slash.Collections.AttributeTables;

    /// <summary>
    ///   Creates an entity with the specified blueprint and configuration.
    /// </summary>
    public class CreateEntityProcess : GameProcess
    {
        #region Fields

        private readonly IAttributeTable attributeTable;

        private readonly string blueprintId;

        #endregion

        #region Constructors and Destructors

        public CreateEntityProcess(string blueprintId)
            : this(blueprintId, null)
        {
        }

        public CreateEntityProcess(string blueprintId, IAttributeTable attributeTable)
        {
            this.blueprintId = blueprintId;
            this.attributeTable = attributeTable;
        }

        #endregion

        #region Public Methods and Operators

        public override void Update(float dt)
        {
            base.Update(dt);

            this.EntityManager.CreateEntity(this.blueprintId, this.attributeTable);

            this.Kill();
        }

        #endregion
    }
}