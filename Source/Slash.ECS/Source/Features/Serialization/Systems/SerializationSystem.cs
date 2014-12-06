// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationSystem.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Features.Serialization.Systems
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Events;
    using Slash.ECS.Features.Serialization.Events;
    using Slash.ECS.Systems;

    [GameSystem]
    public class SerializationSystem : GameSystem
    {
        #region Public Methods and Operators

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
                this.EntityManager.InitEntity(savedEntity.EntityId, savedEntity.Blueprint, null, null);
            }
        }

        private void OnSave(GameEvent e)
        {
            string path = (string)e.EventData;
            List<SerializedEntity> savedEntities =
                this.EntityManager.Entities.Select(
                    entityId =>
                    new SerializedEntity { EntityId = entityId, Blueprint = this.EntityManager.Save(entityId) })
                    .ToList();

            Savegame savegame = new Savegame { SavedEntities = savedEntities };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Savegame));
            Stream stream = File.Open(path, FileMode.Truncate);
            xmlSerializer.Serialize(stream, savegame);
        }

        #endregion

        public class Savegame
        {
            #region Public Properties

            public List<SerializedEntity> SavedEntities { get; set; }

            #endregion
        }

        public class SerializedEntity
        {
            #region Public Properties

            public Blueprint Blueprint { get; set; }

            public int EntityId { get; set; }

            #endregion
        }
    }
}