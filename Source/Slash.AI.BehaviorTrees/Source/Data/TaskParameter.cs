// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskParameter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Data
{
    using System;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Base class for task parameter.
    /// </summary>
    [Serializable]
    public abstract class TaskParameter
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        protected TaskParameter()
        {
            this.Location = TaskParameterLocation.UserValue;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blackboard attribute to take the parameter value from.
        /// </summary>
        [XmlIgnore]
        public abstract object BlackboardAttribute { get; }

        /// <summary>
        ///   Location to take parameter value from.
        /// </summary>
        public TaskParameterLocation Location { get; set; }

        /// <summary>
        ///   Returns the type of the task parameter value.
        /// </summary>
        [XmlIgnore]
        public abstract Type Type { get; }

        /// <summary>
        ///   Parameter value, set by the user.
        /// </summary>
        [XmlIgnore]
        public abstract object UserValue { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(TaskParameter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.BlackboardAttribute, this.BlackboardAttribute) && Equals(other.Location, this.Location)
                   && Equals(other.UserValue, this.UserValue);
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

            if (obj.GetType() != typeof(TaskParameter))
            {
                return false;
            }

            return this.Equals((TaskParameter)obj);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.BlackboardAttribute != null ? this.BlackboardAttribute.GetHashCode() : 0;
                result = (result * 397) ^ this.Location.GetHashCode();
                result = (result * 397) ^ (this.UserValue != null ? this.UserValue.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        ///   Tries to get the task parameter value.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="value"> Parameter value. </param>
        /// <returns> True if value was determined; otherwise, false. </returns>
        public bool TryGetValue(IAgentData agentData, out object value)
        {
            switch (this.Location)
            {
                case TaskParameterLocation.UserValue:
                    {
                        // Use user value.
                        value = this.UserValue;
                        return true;
                    }

                case TaskParameterLocation.Blackboard:
                    {
                        // Try to get from blackboard.
                        if (agentData.Blackboard != null && this.BlackboardAttribute != null
                            && agentData.Blackboard.TryGetValue(this.BlackboardAttribute, out value))
                        {
                            return true;
                        }

                        // Use user value as fallback/default.
                        if (this.UserValue != null)
                        {
                            value = this.UserValue;
                            return true;
                        }
                    }

                    break;
            }

            // Not found.
            value = null;
            return false;
        }

        #endregion
    }

    /// <summary>
    ///   Task parameter for static types (e.g. value types and non-derived types).
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Serializable]
    public class TaskParameter<T> : TaskParameterGeneric<T>
    {
        #region Public Properties

        /// <summary>
        ///   User value xml serialization.
        /// </summary>
        [XmlElement("UserValue")]
        public T ConcreteUserValueSerialized
        {
            get
            {
                return this.ConcreteUserValue;
            }

            set
            {
                this.ConcreteUserValue = value;
            }
        }

        #endregion
    }
}