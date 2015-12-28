// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityGameObjectsBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using System.Collections.Generic;
    using System.Linq;

    using Slash.ECS;
    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.Reflection.Utils;

    using UnityEngine;

    /// <summary>
    ///   Manages the game objects of all game entities after they are created.
    /// </summary>
    public class EntityGameObjectsBehaviour : MonoBehaviour
    {
        #region Constants

        /// <summary>
        ///   Singleton instance of this behaviour.
        /// </summary>
        private static EntityGameObjectsBehaviour instance;

        #endregion

        #region Fields

        /// <summary>
        ///   Connections between logical events and the visual entity game objects.
        /// </summary>
        private readonly IList<LogicToVisualDelegate> logicToVisualDelegates = new List<LogicToVisualDelegate>();

        /// <summary>
        ///   All available mappings from logic components to visual behaviour.
        /// </summary>
        private readonly List<LogicToVisualMapping> logicVisualMappings = new List<LogicToVisualMapping>();

        /// <summary>
        ///   Pool of unused entity objects which are ready for using.
        /// </summary>
        public EntityObjectPool EntityObjectPool;

        /// <summary>
        ///   Game behaviour which manages the current game.
        /// </summary>
        public GameBehaviour GameBehaviour;

        /// <summary>
        ///   Maps entity ids to Unity game objects.
        /// </summary>
        private Dictionary<int, GameObject> entities;

        /// <summary>
        ///   Node to place entity game objects under.
        /// </summary>
        private GameObject entitiesRoot;

        private Game game;

        #endregion

        #region Properties

        /// <summary>
        ///   Entity game objects.
        /// </summary>
        public IEnumerable<GameObject> EntityObjects
        {
            get
            {
                return this.entities != null ? this.entities.Values : null;
            }
        }

        /// <summary>
        ///   Singleton instance of this behaviour.
        /// </summary>
        public static EntityGameObjectsBehaviour Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Clears the entity-gameobject map and destroys all entity objects.
        /// </summary>
        public void ClearEntites()
        {
            // Push entities back to object pool.
            foreach (KeyValuePair<int, GameObject> entityPair in this.entities)
            {
                GameObject entityObject = entityPair.Value;
                if (this.EntityObjectPool != null)
                {
                    // Reset entity object.
                    this.ResetEntityObject(entityObject);

                    // Move back to pool.
                    this.EntityObjectPool.Free(entityObject);
                }
                else if (Application.isEditor)
                {
                    DestroyImmediate(entityObject);
                }
                else
                {
                    Destroy(entityObject);
                }
            }

            this.entities = null;

            if (this.entitiesRoot != null)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(this.entitiesRoot);
                }
                else
                {
                    Destroy(this.entitiesRoot);
                }
                this.entitiesRoot = null;
            }
        }

        /// <summary>
        ///   Prepares for creating entity game objects.
        /// </summary>
        /// <param name="game">Game to create entity game objects for.</param>
        public void SetupForGame(Game game)
        {
            this.game = game;

            if (this.game != null)
            {
                // Create entityId/GameObject mapping.
                this.entities = new Dictionary<int, GameObject>();

#if UNITY_EDITOR
                // Create entity root.
                this.entitiesRoot = new GameObject("Entities");
                this.entitiesRoot.transform.parent = this.transform;
#endif

                // Register for events.
                game.EventManager.RegisterListener(FrameworkEvent.EntityCreated, this.OnEntityCreated);
                game.EventManager.RegisterListener(FrameworkEvent.ComponentAdded, this.OnComponentAdded);
                game.EventManager.RegisterListener(FrameworkEvent.ComponentRemoved, this.OnComponentRemoved);
                game.EventManager.RegisterListener(FrameworkEvent.EntityRemoved, this.OnEntityRemoved);

                // Register for events for visual behaviours.
                foreach (LogicToVisualDelegate logicToVisualDelegate in this.logicToVisualDelegates)
                {
                    game.EventManager.RegisterListener(logicToVisualDelegate.Event, this.OnVisualEvent);
                }
            }
        }

        /// <summary>
        ///   Tries to find the game object of the entity with the specified id.
        /// </summary>
        /// <param name="entityId">Id of the entity to get.</param>
        /// <param name="entityObject">
        ///   Game object of the entity with the specified id, if found, and <c>null</c> otherwise.
        /// </param>
        /// <returns>
        ///   <c>true</c>, if the game object of the entity was found, and <c>false</c> otherwise.
        /// </returns>
        public bool TryGetEntityObject(int entityId, out GameObject entityObject)
        {
            return this.entities.TryGetValue(entityId, out entityObject);
        }

        #endregion

        #region Methods

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            instance = this;

            if (this.EntityObjectPool == null)
            {
                this.EntityObjectPool = (EntityObjectPool)FindObjectOfType(typeof(EntityObjectPool));
            }

            if (this.GameBehaviour != null)
            {
                this.GameBehaviour.GameChanged += this.OnGameChanged;
            }
        }

        private void DelegateVisualEvent(int entityId, GameEvent e)
        {
            // Get entity object.
            GameObject entityObject;
            this.TryGetEntityObject(entityId, out entityObject);
            if (entityObject == null)
            {
                Debug.LogError(
                    string.Format(
                        "Received event {0} for visual behaviour, but no entity object with id {1} was found.",
                        e.EventType,
                        entityId));
                return;
            }

            // Check which method to call.
            LogicToVisualDelegate logicToVisualDelegate =
                this.logicToVisualDelegates.FirstOrDefault(
                    existingDelegate => existingDelegate.Event.Equals(e.EventType));
            if (logicToVisualDelegate != null)
            {
                foreach (string callbackName in logicToVisualDelegate.CallbackNames)
                {
                    entityObject.SendMessage(callbackName, e, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        /// <summary>
        ///   Adds the matching Unity component to the game object that represents
        ///   the entity with the passed id.
        /// </summary>
        /// <param name="e"> Game event that has occurred. </param>
        private void OnComponentAdded(GameEvent e)
        {
            Profiler.BeginSample("Component added");

            EntityComponentData eventArgs = (EntityComponentData)e.EventData;
            int entityId = eventArgs.EntityId;
            IEntityComponent component = eventArgs.Component;
            GameObject entityObject = this.entities[entityId];

            // Check if a behaviour has to be attached which visualizes the logic state.
            foreach (LogicToVisualMapping logicToVisualMapping in
                this.logicVisualMappings.Where(mapping => mapping.LogicType == component.GetType()))
            {
                // NOTE: The component may already exist because we recycle existing entity objects and the old components
                // just get removed after the current update loop (see http://docs.unity3d.com/Documentation/ScriptReference/Object.Destroy.html).
                entityObject.AddComponent(logicToVisualMapping.VisualType);
            }

            Profiler.EndSample();
        }

        private void OnComponentRemoved(GameEvent e)
        {
            EntityComponentData eventArgs = (EntityComponentData)e.EventData;
            int entityId = eventArgs.EntityId;
            IEntityComponent component = eventArgs.Component;
            GameObject entityObject = this.entities[entityId];

            // Check if a behaviour has to be removed which visualizes the logic state.
            foreach (LogicToVisualMapping logicToVisualMapping in this.logicVisualMappings)
            {
                if (component.GetType() != logicToVisualMapping.LogicType)
                {
                    continue;
                }

                Component visualComponent = entityObject.GetComponent(logicToVisualMapping.VisualType);
                if (visualComponent != null)
                {
                    Destroy(visualComponent);
                }
                break;
            }
        }

        private void OnDestroy()
        {
            if (this.GameBehaviour != null)
            {
                this.GameBehaviour.GameChanged -= this.OnGameChanged;
            }

            if (this.entities == null)
            {
                return;
            }

            foreach (KeyValuePair<int, GameObject> entityPair in this.entities)
            {
                GameObject entityObject = entityPair.Value;

                // Clear entity object.
                foreach (LogicToVisualMapping logicToVisualMapping in this.logicVisualMappings)
                {
                    Component component = entityObject.GetComponent(logicToVisualMapping.VisualType);
                    if (component != null)
                    {
                        Destroy(component);
                    }
                }

                if (this.EntityObjectPool != null)
                {
                    this.EntityObjectPool.Free(entityObject);
                }
            }
            this.entities.Clear();

            if (instance == this)
            {
                instance = null;
            }
        }

        /// <summary>
        ///   Creates a new game object whenever a new entity has been created.
        /// </summary>
        /// <param name="e"> Game event that has occurred. </param>
        private void OnEntityCreated(GameEvent e)
        {
            int entityId = (int)e.EventData;

            // Check if entity is still alive, may already be removed due to different update frames.
            if (!this.game.EntityManager.EntityIsAlive(entityId))
            {
                return;
            }

            if (this.entities.ContainsKey(entityId))
            {
                Debug.LogError(string.Format("Entity object for entity with id '{0}' already exists.", entityId));
                return;
            }

            Profiler.BeginSample("Create entity");

            GameObject entityObject;
            if (this.EntityObjectPool != null)
            {
                entityObject = this.EntityObjectPool.Alloc();
            }
            else
            {
                Debug.LogWarning("No entity object pool available, creating dummy entity object.");
                entityObject = new GameObject();
            }

#if UNITY_EDITOR
            Profiler.BeginSample("Change parent transform");
            entityObject.transform.parent = this.entitiesRoot.transform;
            Profiler.EndSample();
#endif

            // Check for entity behaviour to set entity id.
            EntityBehaviour entityBehaviour = entityObject.GetComponent<EntityBehaviour>();
            if (entityBehaviour != null)
            {
                entityBehaviour.EntityId = entityId;
                entityBehaviour.Game = this.game;
            }

            this.entities.Add(entityId, entityObject);

            Profiler.EndSample();
        }
        
        /// <summary>
        ///   Destroys the game object representing the entity with the specified id.
        /// </summary>
        /// <param name="e"> Game event that has occurred. </param>
        private void OnEntityRemoved(GameEvent e)
        {
            int entityId = (int)e.EventData;

            GameObject entityObject;
            if (!this.entities.TryGetValue(entityId, out entityObject))
            {
                Debug.LogError(string.Format("Entity object for entity with id '{0}' wasn't found.", entityId));
                return;
            }

            Profiler.BeginSample("Remove entity");

            // Reset entity object.
            this.ResetEntityObject(entityObject);

            if (this.EntityObjectPool != null)
            {
                this.EntityObjectPool.Free(entityObject);
            }

            this.entities.Remove(entityId);

            Profiler.EndSample();
        }

        private void OnGameChanged(Game newGame, Game oldGame)
        {
            this.SetupForGame(newGame);
        }

        private void OnVisualEvent(GameEvent e)
        {
            // Check for which entity the event is for.
            EntityEventData entityEventData = e.EventData as EntityEventData;
            if (entityEventData != null)
            {
                this.DelegateVisualEvent(entityEventData.EntityId, e);
                return;
            }

            Entity2Data entity2EventData = e.EventData as Entity2Data;
            if (entity2EventData != null)
            {
                this.DelegateVisualEvent(entity2EventData.First, e);
                this.DelegateVisualEvent(entity2EventData.Second, e);
                return;
            }

            Debug.LogError(
                string.Format(
                    "Received event {0} for visual behaviour, but event data {1} wasn't derived from EntityEventData or Entity2Data to get entity id(s).",
                    e.EventType,
                    e.EventData.GetType()));
        }

        /// <summary>
        ///   Resets the specified game object which represented an entity.
        ///   This is required before reusing the object to make sure there are no remaining behaviours on the
        ///   game object.
        /// </summary>
        /// <param name="entityObject">Game object to clean up.</param>
        private void ResetEntityObject(GameObject entityObject)
        {
            foreach (Component component in entityObject.GetComponents(typeof(MonoBehaviour)))
            {
                if (this.logicVisualMappings.Any(mapping => mapping.VisualType == component.GetType()))
                {
                    Destroy(component);
                }
            }
        }

        private void Start()
        {
            // Search all visual behaviour which map to logic components.
            ReflectionUtils.HandleTypesWithAttribute<LogicToVisualMappingAttribute>(
                (visualType, mappingAttribute) =>
                    this.logicVisualMappings.Add(LogicToVisualMapping.create(mappingAttribute.LogicType, visualType)));

            // Get delegates of visual behaviours.
            foreach (LogicToVisualMapping logicToVisualMapping in this.logicVisualMappings)
            {
                LogicToVisualDelegateAttribute[] delegateAttributes =
                    (LogicToVisualDelegateAttribute[])
                        ReflectionUtils.GetAttributes(
                            logicToVisualMapping.VisualType,
                            typeof(LogicToVisualDelegateAttribute),
                            true);
                if (delegateAttributes.Length == 0)
                {
                    continue;
                }

                foreach (LogicToVisualDelegateAttribute delegateAttribute in delegateAttributes)
                {
                    // Make sure that the same event with the same callback name is not already registered, otherwise
                    // the method would be called multiple times when the event occurs.
                    LogicToVisualDelegate logicToVisualDelegate =
                        this.logicToVisualDelegates.FirstOrDefault(
                            existingDelegate => existingDelegate.Event.Equals(delegateAttribute.Event));
                    if (logicToVisualDelegate == null)
                    {
                        logicToVisualDelegate = LogicToVisualDelegate.create(delegateAttribute);
                        this.logicToVisualDelegates.Add(logicToVisualDelegate);
                    }
                    else
                    {
                        logicToVisualDelegate.CallbackNames.Add(delegateAttribute.CallbackName);
                    }
                }
            }
        }

        #endregion
    }
}