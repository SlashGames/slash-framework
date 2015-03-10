// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EasedMovementBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Transforms
{
    using UnityEngine;

    /// <summary>
    ///   Eases out and restricts transitions between positions.
    /// </summary>
    public class EasedMovementBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Collider to clamp position to, always.
        /// </summary>
        public BoxCollider BoundsHard;

        /// <summary>
        ///   Collider to clamp position to, when requested.
        /// </summary>
        public BoxCollider BoundsSoft;

        [Range(0, 10)]
        public float EaseFactor = 1.0f;

        public bool IgnoreBoundsX;

        public bool IgnoreBoundsY;

        public bool IgnoreBoundsZ;

        /// <summary>
        ///   Prevents all movement.
        /// </summary>
        public bool Static;

        /// <summary>
        ///   Game object to move.
        /// </summary>
        public GameObject Target;

        /// <summary>
        ///   Ignore next manual current and target position change. Useful for ignoring single Drag events triggered after each Swipe.
        /// </summary>
        private bool ignoreNextPositionChange;

        private Vector3 targetPosition;

        #endregion

        #region Public Properties

        public Vector3 CurrentPosition
        {
            get
            {
                return this.Target.transform.position;
            }

            private set
            {
                this.Target.transform.position = this.ClampPosition(value, this.BoundsHard);
            }
        }

        public Vector3 TargetPosition
        {
            get
            {
                return this.targetPosition;
            }
            private set
            {
                this.targetPosition = this.ClampPosition(value, this.BoundsHard);
            }
        }

        #endregion

        #region Public Methods and Operators

        public void ClampToSoftBounds()
        {
            this.TargetPosition = this.ClampPosition(this.TargetPosition, this.BoundsSoft);
        }

        public void EaseToPosition(Vector3 newPosition)
        {
            if (this.Static)
            {
                return;
            }

            this.TargetPosition = newPosition;
            this.ignoreNextPositionChange = true;
        }

        public void SetPosition(Vector3 newPosition)
        {
            if (this.Static)
            {
                return;
            }

            if (this.ignoreNextPositionChange)
            {
                this.ignoreNextPositionChange = false;
                return;
            }

            this.TargetPosition = newPosition;
            this.CurrentPosition = newPosition;
        }

        #endregion

        #region Methods

        private Vector3 ClampPosition(Vector3 newPosition, BoxCollider bounds)
        {
            if (bounds == null || !bounds.enabled)
            {
                return newPosition;
            }
            return
                new Vector3(
                    this.IgnoreBoundsX
                        ? newPosition.x
                        : Mathf.Clamp(newPosition.x, bounds.bounds.min.x, bounds.bounds.max.x),
                    this.IgnoreBoundsY
                        ? newPosition.y
                        : Mathf.Clamp(newPosition.y, bounds.bounds.min.y, bounds.bounds.max.y),
                    this.IgnoreBoundsZ
                        ? newPosition.z
                        : Mathf.Clamp(newPosition.z, bounds.bounds.min.z, bounds.bounds.max.z));
        }

        private void Start()
        {
            if (this.Target == null)
            {
                this.Target = this.gameObject;
            }

            this.TargetPosition = this.CurrentPosition;
        }

        private void Update()
        {
            if (this.Static)
            {
                return;
            }

            this.CurrentPosition = Vector3.Lerp(
                this.CurrentPosition, this.TargetPosition, Time.deltaTime * this.EaseFactor);
        }

        #endregion
    }
}