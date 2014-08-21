// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UICenterChildren.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Interaction
{
    using UnityEngine;

    /// <summary>
    ///   Centers the children of the game object horizontally, vertically or in both directions to the bounds of the game object.
    /// </summary>
    [ExecuteInEditMode]
    public class UICenterChildren : MonoBehaviour
    {
        #region Fields

        public Direction direction = Direction.Both;

        public bool everyFrame = true;

        public bool repositionNow = false;

        /// <summary>
        ///   Grid which may manage the positions of the children.
        /// </summary>
        private UIGrid uiGrid;

        /// <summary>
        ///   Table which may manage the positions of the children.
        /// </summary>
        private UITable uiTable;

        #endregion

        #region Enums

        public enum Direction
        {
            None,

            Horizontal,

            Vertical,

            Both
        }

        #endregion

        #region Methods

        private void Awake()
        {
            this.uiTable = this.GetComponent<UITable>();
            this.uiGrid = this.GetComponent<UIGrid>();
        }

        private void OnDisable()
        {
            if (this.uiTable != null)
            {
                this.uiTable.onReposition -= this.Recenter;
            }
            if (this.uiGrid != null)
            {
                this.uiGrid.onReposition -= this.Recenter;
            }
        }

        private void OnEnable()
        {
            if (this.uiTable != null)
            {
                this.uiTable.onReposition += this.Recenter;
            }
            if (this.uiGrid != null)
            {
                this.uiGrid.onReposition += this.Recenter;
            }
        }

        [ContextMenu("Execute")]
        private void Recenter()
        {
            if (this.direction != Direction.None)
            {
                Vector3 boundsCenter = NGUIMath.CalculateAbsoluteWidgetBounds(this.transform).center;

                if (this.direction == Direction.Horizontal || this.direction == Direction.Both)
                {
                    float differenceHorizontal = this.transform.position.x - boundsCenter.x;

                    foreach (Transform child in this.transform)
                    {
                        child.Translate(differenceHorizontal, 0f, 0f, Space.World);
                    }
                }

                if (this.direction == Direction.Vertical || this.direction == Direction.Both)
                {
                    float differenceVertical = this.transform.position.y - boundsCenter.y;

                    foreach (Transform child in this.transform)
                    {
                        child.Translate(0f, differenceVertical, 0f, Space.World);
                    }
                }
            }
        }

        private void Start()
        {
            this.Recenter();
        }

        private void Update()
        {
            if (this.everyFrame)
            {
                this.Recenter();
            }

            if (this.repositionNow)
            {
                this.repositionNow = false;
                this.Recenter();
            }
        }

        #endregion
    }
}