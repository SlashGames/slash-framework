// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChildPrefabBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using EZData;

    using UnityEngine;

    public class ChildPrefabBinding<T> : NguiBinding
        where T : Context
    {
        #region Fields

        /// <summary>
        ///   Prefab to instantiate a new game object from.
        /// </summary>
        public GameObject Template;

        /// <summary>
        ///   Instantiated game object which visualizes child property.
        /// </summary>
        private GameObject childObject;

        private Property<T> childProperty;

        #endregion

        #region Delegates

        public delegate void ChildContextChangeDelegate(T newChild);

        #endregion

        #region Public Events

        public event ChildContextChangeDelegate ChildContextChange;

        #endregion

        #region Public Properties

        public GameObject ChildObject
        {
            get
            {
                return this.childObject;
            }
        }

        #endregion

        #region Methods

        protected override void Bind()
        {
            var context = this.GetContext(this.Path);
            if (context == null)
            {
                Debug.LogWarning("ChildPrefabBinding.Bind - context is null");
                return;
            }

            this.childProperty = context.FindProperty<T>(this.Path, this);
            if (this.childProperty != null)
            {
                this.childProperty.OnChange += this.OnChange;
            }
            else
            {
                Debug.LogWarning("No child property at path " + this.Path);
            }

            this.OnChange();
        }

        protected override void OnChange()
        {
            if (this.childProperty == null)
            {
                return;
            }

            // Instantiate from prefab if no item object available.
            if (this.childObject == null)
            {
                this.childObject = NGUITools.AddChild(this.gameObject, this.Template);
                this.childObject.transform.parent = this.transform;
            }

            T child = this.childProperty.GetValue();
            if (child != null)
            {
                this.childObject.SetActive(true);

                // Set context.
                NguiItemDataContext itemData = this.childObject.GetComponent<NguiItemDataContext>();
                if (itemData == null)
                {
                    itemData = this.childObject.AddComponent<NguiItemDataContext>();
                }
                itemData.SetContext(child);
            }
            else
            {
                this.childObject.SetActive(false);
            }

            this.OnChildContextChange(child);
        }

        private void OnChildContextChange(T newChild)
        {
            var handler = this.ChildContextChange;
            if (handler != null)
            {
                handler(newChild);
            }
        }

        #endregion
    }
}