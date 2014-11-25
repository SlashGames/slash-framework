// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwipeValidDirections.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.FingerGesturesExt.GestureRecognizers
{
    using Slash.Unity.Common.Delegates;

    using UnityEngine;

    public class SwipeValidDirections : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Method to invoke if swipe in valid direction was detected.
        /// </summary>
        public MethodDelegate Delegate;

        /// <summary>
        ///   Swipe recognizer to use. If empty the swipe recognizer on this game object is used.
        /// </summary>
        [Tooltip("Swipe recognizer to use. If empty the swipe recognizer on this game object is used.")]
        public SwipeRecognizer SwipeRecognizer;

        /// <summary>
        ///   Directions to restrict the swipe gesture to
        /// </summary>
        public FingerGestures.SwipeDirection ValidDirections = FingerGestures.SwipeDirection.All;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Return true if the input direction is supported
        /// </summary>
        public bool IsValidDirection(FingerGestures.SwipeDirection dir)
        {
            if (dir == FingerGestures.SwipeDirection.None)
            {
                return false;
            }

            return ((this.ValidDirections & dir) == dir);
        }

        public void OnSwipe(SwipeGesture gesture)
        {
            // Check if direction is valid.
            if (!this.IsValidDirection(gesture.Direction))
            {
                return;
            }

            // Forward event.
            if (this.Delegate != null)
            {
                this.Delegate.Invoke();
            }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            if (this.SwipeRecognizer == null)
            {
                this.SwipeRecognizer = this.GetComponent<SwipeRecognizer>();
            }
        }

        private void OnDisable()
        {
            if (this.SwipeRecognizer != null)
            {
                this.SwipeRecognizer.OnGesture -= this.OnSwipe;
            }
        }

        private void OnEnable()
        {
            if (this.SwipeRecognizer != null)
            {
                this.SwipeRecognizer.OnGesture += this.OnSwipe;
            }
        }

        #endregion
    }
}