// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Context
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Slash.GameBase.Blueprints;

    public sealed class EditorContext
    {
        #region Fields

        private readonly XmlSerializer blueprintManagerSerializer;

        private BlueprintManager blueprintManager;

        public IEnumerable<Type> EntityComponentTypes { get; set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public EditorContext()
        {
            this.BlueprintManager = new BlueprintManager();
            this.blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));

            // NOTE(co): Available entity components should be set dynamically from application libraries.
            this.EntityComponentTypes = new List<Type>() { typeof(int), typeof(bool) };
        }

        #endregion

        #region Delegates

        public delegate void BlueprintManagerChangedDelegate(
            BlueprintManager newBlueprintManager, BlueprintManager oldBlueprintManager);

        #endregion

        #region Public Events

        /// <summary>
        ///   Raised when the blueprint manager in the context changed.
        /// </summary>
        public event BlueprintManagerChangedDelegate BlueprintManagerChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blueprint manager which is edited.
        /// </summary>
        public BlueprintManager BlueprintManager
        {
            get
            {
                return this.blueprintManager;
            }
            private set
            {
                if (ReferenceEquals(value, this.blueprintManager))
                {
                    return;
                }

                BlueprintManager oldBlueprintManager = this.blueprintManager;
                this.blueprintManager = value;

                this.OnBlueprintManagerChanged(this.blueprintManager, oldBlueprintManager);
            }
        }

        public string SerializationPath { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Load(string path)
        {
            BlueprintManager newBlueprintManager =
                (BlueprintManager)this.blueprintManagerSerializer.Deserialize(new FileStream(path, FileMode.Open));
            if (newBlueprintManager == null)
            {
                throw new SerializationException(
                    string.Format("Couldn't deserialize blueprint manager from '{0}'.", path));
            }
            this.BlueprintManager = newBlueprintManager;
            this.SerializationPath = path;
        }

        public void New()
        {
            this.BlueprintManager = new BlueprintManager();
            this.SerializationPath = null;
        }

        public void Save()
        {
            var fileStream = new FileStream(this.SerializationPath, FileMode.Create);
            this.blueprintManagerSerializer.Serialize(fileStream, this.BlueprintManager);
            fileStream.Close();
        }

        #endregion

        #region Methods

        private void OnBlueprintManagerChanged(
            BlueprintManager newBlueprintManager, BlueprintManager oldBlueprintManager)
        {
            BlueprintManagerChangedDelegate handler = this.BlueprintManagerChanged;
            if (handler != null)
            {
                handler(newBlueprintManager, oldBlueprintManager);
            }
        }

        #endregion
    }
}