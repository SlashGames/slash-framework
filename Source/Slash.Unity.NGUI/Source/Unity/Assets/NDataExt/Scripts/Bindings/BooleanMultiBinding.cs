// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanMultiBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EZData;

    using UnityEngine;

    /// <summary>
    ///   Binds a collection of boolean values.
    /// </summary>
    public abstract class BooleanMultiBinding : NguiBaseBinding
    {
        #region Fields

        public bool DefaultValue = false;

        public List<BooleanProperty> Properties;

        private readonly Dictionary<BooleanProperty, Dictionary<Type, Property>> propertyGroups =
            new Dictionary<BooleanProperty, Dictionary<Type, Property>>();

        #endregion

        #region Public Properties

        public override IList<string> ReferencedPaths
        {
            get
            {
                return this.Properties.Select(property => property.Path).ToList();
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void Awake()
        {
            base.Awake();

            for (var i = 0; i < this.Properties.Count; ++i)
            {
                this.propertyGroups.Add(this.Properties[i], new Dictionary<Type, Property>());
            }
        }

        #endregion

        #region Methods

        protected abstract void ApplyNewValue(List<bool> values);

        protected override void Bind()
        {
            base.Bind();

            foreach (var propertyGroup in this.propertyGroups)
            {
                this.FillBooleanProperties(propertyGroup.Value, propertyGroup.Key.Path);
                foreach (var p in propertyGroup.Value)
                {
                    p.Value.OnChange += this.OnChange;
                }
            }
        }

        protected void FillBooleanProperties(Dictionary<Type, Property> properties, string path)
        {
            var context = this.GetContext(path);
            if (context == null)
            {
                Debug.LogWarning("FillBooleanProperties - context is null");
                return;
            }

            properties.Add(typeof(bool), context.FindProperty<bool>(path, this));

            this.ClearNullProperties(properties);
        }

        protected List<bool> GetBooleanValue()
        {
            var newValues = new List<bool>();
            foreach (var properties in this.propertyGroups)
            {
                bool value = this.GetBooleanValue(properties.Value);
                if (properties.Key.Invert)
                {
                    value = !value;
                }
                newValues.Add(value);
            }
            return newValues;
        }

        protected override void OnChange()
        {
            base.OnChange();

            var newValues = this.GetBooleanValue();
            this.ApplyNewValue(newValues);
        }

        protected override void Unbind()
        {
            base.Unbind();

            foreach (var propertyGroup in this.propertyGroups)
            {
                foreach (var property in propertyGroup.Value)
                {
                    property.Value.OnChange -= this.OnChange;
                }
                propertyGroup.Value.Clear();
            }
        }

        private bool GetBooleanValue(Dictionary<Type, Property> properties)
        {
            var newValue = this.DefaultValue;
            if (properties[typeof(bool)] != null)
            {
                newValue = ((Property<bool>)properties[typeof(bool)]).GetValue();
            }
            return newValue;
        }

        #endregion

        [Serializable]
        public class BooleanProperty
        {
            #region Fields

            public bool Invert = false;

            public string Path;

            #endregion
        }
    }
}