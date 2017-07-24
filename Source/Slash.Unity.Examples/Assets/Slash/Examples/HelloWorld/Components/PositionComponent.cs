namespace Slash.Examples.HelloWorld.Components
{
    using Slash.ECS.Components;
    using Slash.ECS.Inspector.Attributes;
    using Slash.Math.Algebra.Vectors;

    [InspectorType]
    public class PositionComponent : EntityComponent
    {
        /// <summary>
        ///   Attribute: Current position.
        /// </summary>
        public const string AttributePosition = "PositionComponent.Position";

        /// <summary>
        ///   Current position.
        /// </summary>
        [InspectorVector(Name = AttributePosition, Description = "Current position.")]
        public Vector2F Position { get; set; }
    }
}