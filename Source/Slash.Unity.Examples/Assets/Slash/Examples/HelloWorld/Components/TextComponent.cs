namespace Slash.Examples.HelloWorld.Components
{
    using Slash.ECS.Components;
    using Slash.ECS.Inspector.Attributes;

    [InspectorType]
    public class TextComponent : EntityComponent
    {
        /// <summary>
        ///   Attribute: Text to show.
        /// </summary>
        public const string AttributeText = "TextComponent.Text";

        /// <summary>
        ///   Text to show.
        /// </summary>
        [InspectorString(Name = AttributeText, Description = "Text to show.")]
        public string Text { get; set; }
    }
}