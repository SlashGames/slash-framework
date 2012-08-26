// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskParameterGeneric.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.AI.BehaviorTrees.Data
{
    using System;
    using System.Xml.Serialization;

    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Generic task parameter which makes it possible to let the user decide where the parameter value is taken from. Possibilities are: - Set by the user. - Taken from blackboard.
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Serializable]
    public abstract class TaskParameterGeneric<T> : TaskParameter
    {
        #region Fields

        /// <summary>
        ///   Blackboard attribute to take the parameter value from.
        /// </summary>
        private object blackboardAttribute;

        /// <summary>
        ///   Parameter value, set by the user.
        /// </summary>
        private T userValue;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blackboard attribute to take the parameter value from.
        /// </summary>
        [XmlIgnore]
        public override object BlackboardAttribute
        {
            get
            {
                return this.blackboardAttribute;
            }
        }

        /// <summary>
        ///   Parameter value, set by the user.
        /// </summary>
        [XmlIgnore]
        public T ConcreteUserValue
        {
            get
            {
                return this.userValue;
            }

            set
            {
                this.userValue = value;
            }
        }

        /// <summary>
        ///   Returns the type of the task parameter value.
        /// </summary>
        [XmlIgnore]
        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        ///   Parameter value, set by the user.
        /// </summary>
        [XmlIgnore]
        public override object UserValue
        {
            get
            {
                return this.userValue;
            }

            set
            {
                if (!(value is T))
                {
                    throw new InvalidCastException(
                        string.Format(
                            "New value can't be cast to expected type '{0}' (type: '{1}').",
                            typeof(T),
                            value != null ? value.GetType() : null));
                }

                this.userValue = (T)value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(TaskParameterGeneric<T> other)
        {
            return base.Equals(other);
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

            return this.Equals(obj as TaskParameterGeneric<T>);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        ///   Tries to get the task parameter value.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="value"> Parameter value. </param>
        /// <returns> True if value was determined; otherwise, false. </returns>
        public bool TryGetValue(IAgentData agentData, out T value)
        {
            object objectValue;
            bool result = this.TryGetValue(agentData, out objectValue);
            if (result)
            {
                value = (T)objectValue;
                return true;
            }

            value = default(T);
            return false;
        }

        #endregion
    }
}