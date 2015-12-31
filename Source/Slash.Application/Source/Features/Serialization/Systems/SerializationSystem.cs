// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationSystem.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Application.Features.Serialization.Systems
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using Slash.Application.Features.Serialization.Events;
    using Slash.Application.Systems;
    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Blueprints.Extensions;
    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.ECS.Inspector.Data;

    /// <summary>
    ///   Loads and saves the game in a xml format.
    /// </summary>
    [GameSystem]
    public class SerializationSystem : GameSystem
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this system with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="configuration">System configuration data.</param>
        public override void Init(IAttributeTable configuration)
        {
            base.Init(configuration);

            this.EventManager.RegisterListener(SerializationAction.Save, this.OnSave);
            this.EventManager.RegisterListener(SerializationAction.Load, this.OnLoad);
        }

        #endregion

        #region Methods

        private void OnLoad(GameEvent e)
        {
            string path = (string)e.EventData;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Savegame));
            Stream stream = File.Open(path, FileMode.Open);
            Savegame savegame = (Savegame)xmlSerializer.Deserialize(stream);

            // Clear entity manager before load.
            this.EntityManager.RemoveEntities();
            this.EntityManager.CleanUpEntities();

            foreach (var savedEntity in savegame.SavedEntities)
            {
                this.EntityManager.CreateEntity(savedEntity.EntityId);
            }
            foreach (var savedEntity in savegame.SavedEntities)
            {
                this.EntityManager.InitEntity(savedEntity.EntityId, savedEntity.Blueprint);
            }
        }

        private void OnSave(GameEvent e)
        {
            string path = (string)e.EventData;
            List<SerializedEntity> savedEntities = this.EntityManager.Entities.Select(
                entityId =>
                {
                    AttributeTable attributeTable;
                    List<Type> componentTypes;
                    this.Save(entityId, out attributeTable, out componentTypes);

                    var blueprint = new Blueprint { AttributeTable = attributeTable, ComponentTypes = componentTypes };

                    return new SerializedEntity { EntityId = entityId, Blueprint = blueprint };
                }).ToList();

            Savegame savegame = new Savegame { SavedEntities = savedEntities };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Savegame));
            Stream stream = File.Open(path, FileMode.Truncate);
            xmlSerializer.Serialize(stream, savegame);
        }

        private void Save(int entityId, out AttributeTable attributeTable, out List<Type> componentTypes)
        {
            attributeTable = new AttributeTable();
            componentTypes = new List<Type>();

            // Get all components.
            var entityComponents = this.EntityManager.GetComponents(entityId);
            foreach (var entityComponent in entityComponents)
            {
                componentTypes.Add(entityComponent.GetType());
                SaveToAttributeTable(this.EntityManager, entityComponent, attributeTable);
            }
        }

        private static void SaveToAttributeTable(EntityManager entityManager, object obj, AttributeTable attributeTable)
        {
            InspectorType inspectorType = InspectorType.GetInspectorType(obj.GetType());
            if (inspectorType == null)
            {
                throw new ArgumentException("No inspector type for object " + obj.GetType());
            }

            SaveToAttributeTable(entityManager, inspectorType, obj, attributeTable);
        }

        private static void SaveToAttributeTable(
            EntityManager entityManager,
            InspectorType inspectorType,
            object obj,
            AttributeTable attributeTable)
        {
            // Set values for all properties.
            foreach (var inspectorProperty in inspectorType.Properties)
            {
                // Get value from object.
                object propertyValue = inspectorProperty.GetPropertyValue(entityManager, obj);
                attributeTable.SetValue(inspectorProperty.Name, propertyValue);
            }
        }

        #endregion

        private class Savegame
        {
            #region Properties

            public List<SerializedEntity> SavedEntities { get; set; }

            #endregion
        }

        private class SerializedEntity
        {
            #region Properties

            public Blueprint Blueprint { get; set; }

            public int EntityId { get; set; }

            #endregion
        }
    }
}