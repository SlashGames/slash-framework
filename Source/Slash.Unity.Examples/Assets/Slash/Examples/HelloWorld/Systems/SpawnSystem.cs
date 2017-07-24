namespace Slash.Examples.HelloWorld.Systems
{
    using Slash.Application.Systems;
    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Blueprints.Extensions;
    using Slash.ECS.Inspector.Attributes;

    using UnityEngine;

    [GameSystem]
    [InspectorType]
    public class SpawnSystem : GameSystem
    {
        /// <summary>
        ///   Attribute: Blueprint to use.
        /// </summary>
        public const string AttributeSpawnBlueprintId = "SpawnSystem.SpawnBlueprintId";

        /// <summary>
        ///   Attribute: Spawn Interval (in 1/s).
        /// </summary>
        public const string AttributeSpawnInterval = "SpawnSystem.SpawnInterval";

        private float accuTime;

        private Blueprint spawnBlueprint;

        /// <summary>
        ///   Blueprint to use.
        /// </summary>
        [InspectorBlueprint(AttributeSpawnBlueprintId, Description = "Id of blueprint to use.")]
        public string SpawnBlueprintId { get; set; }

        /// <summary>
        ///   Spawn Interval (in 1/s).
        /// </summary>
        [InspectorFloat(AttributeSpawnInterval, Default = 0.25f, Description = "Spawn Interval (in 1/s).")]
        public float SpawnInterval { get; set; }

        /// <inheritdoc />
        public override void Init(IAttributeTable configuration)
        {
            base.Init(configuration);

            this.spawnBlueprint = this.BlueprintManager.GetBlueprint(this.SpawnBlueprintId);
        }

        /// <inheritdoc />
        public override void Update(float dt)
        {
            this.accuTime += dt;

            if (this.SpawnInterval > 0)
            {
                var requiredTime = 1 / this.SpawnInterval;
                if (this.accuTime > requiredTime)
                {
                    this.accuTime -= requiredTime;
                    this.SpawnEntity();
                }
            }
        }

        private void SpawnEntity()
        {
            if (this.spawnBlueprint == null)
            {
                return;
            }

            var entityId = this.EntityManager.CreateEntity(this.spawnBlueprint);

            Debug.LogFormat("Spawned entity {0} with blueprint {1}", entityId, this.SpawnBlueprintId);
        }
    }
}