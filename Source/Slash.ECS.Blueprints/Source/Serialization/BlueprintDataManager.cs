namespace Slash.ECS.Blueprints.Serialization
{
    using System.Collections.Generic;
    using Slash.Serialization;
    using Slash.Serialization.Xml;

    public class BlueprintDataManager
    {
        /// <summary>
        ///   All registered blueprints.
        /// </summary>
        [SerializeMember]
        private readonly SerializableDictionary<string, BlueprintData> blueprints =
            new SerializableDictionary<string, BlueprintData>
            {
                ItemElementName = "Entry",
                KeyElementName = "Id",
                ValueElementName = "Blueprint"
            };

        /// <summary>
        ///   All registered blueprints.
        /// </summary>
        public IEnumerable<KeyValuePair<string, BlueprintData>> Blueprints
        {
            get
            {
                return this.blueprints;
            }
        }

        public void AddBlueprint(BlueprintData blueprint)
        {
            this.blueprints.Add(blueprint.Id, blueprint);
        }

        public void RemoveBlueprint(string blueprintId)
        {
            this.blueprints.Remove(blueprintId);
        }
    }
}