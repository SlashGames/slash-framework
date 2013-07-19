// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseSetBlackboardAttributeBoolean.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Implementations.Actions
{
    using Slash.AI.BehaviorTrees.Attributes;

    /// <summary>
    ///   Sets a boolean blackboard attribute.
    /// </summary>
    public abstract class BaseSetBlackboardAttributeBoolean : BaseSetBlackboardAttribute
    {
        #region Public Properties

        /// <summary>
        ///   Value to set.
        /// </summary>
        [TaskParameter(Name = "Value", Description = "Value to set.")]
        public bool AttributeValue { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Value to set.
        /// </summary>
        protected override object BlackboardAttributeValue
        {
            get
            {
                return this.AttributeValue;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(BaseSetBlackboardAttributeBoolean other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && other.AttributeValue.Equals(this.AttributeValue);
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="obj"> The obj. </param>
        /// <returns> The System.Boolean. </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return this.Equals(obj as BaseSetBlackboardAttributeBoolean);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ this.AttributeValue.GetHashCode();
            }
        }

        #endregion
    }
}