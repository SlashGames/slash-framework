namespace RainyGames.AI.BehaviorTrees.Data
{
    using System;
    using System.Xml.Serialization;

    using RainyGames.AI.BehaviorTrees.Editor;
    using RainyGames.AI.BehaviorTrees.Interfaces;
    using RainyGames.Xml;

    /// <summary>
    ///   Locations to take a task parameter value from.
    /// </summary>
    public enum TaskParameterLocation
    {
        /// <summary>
        ///   The invalid.
        /// </summary>
        Invalid,

        /// <summary>
        ///   The user value.
        /// </summary>
        UserValue,

        /// <summary>
        ///   The blackboard.
        /// </summary>
        Blackboard,
    }

    /// <summary>
    ///   Base class for task parameter.
    /// </summary>
    [Serializable]
    public abstract class TaskParameter
    {
        #region Public Properties

        /// <summary>
        ///   Blackboard attribute to take the parameter value from.
        /// </summary>
        [XmlIgnore]
        public abstract object BlackboardAttribute { get; set; }

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
        public bool TryGetValue(IAgentData agentData, ref object value)
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
                        if (agentData.Blackboard != null
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
            return false;
        }

        #endregion
    }

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

            set
            {
                this.blackboardAttribute = value;
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
        public bool TryGetValue(IAgentData agentData, ref T value)
        {
            object objectValue = null;
            bool result = this.TryGetValue(agentData, ref objectValue);
            if (result)
            {
                value = (T)objectValue;
            }

            return result;
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

    /// <summary>
    ///   Task parameter for dynamic types (e.g. base class of derived types).
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Serializable]
    public class TaskParameterDynamic<T> : TaskParameterGeneric<T>
    {
        #region Public Properties

        /// <summary>
        ///   User value xml serialization.
        /// </summary>
        [XmlElement("UserValue")]
        public XmlAnything<T> ConcreteUserValueSerialized
        {
            get
            {
                return new XmlAnything<T>(this.ConcreteUserValue);
            }

            set
            {
                this.ConcreteUserValue = value != null ? value.Value : default(T);
            }
        }

        #endregion
    }

    /// <summary>
    ///   Task parameter for tasks.
    /// </summary>
    [Serializable]
    public class TaskParameterTask : TaskParameterGeneric<ITask>
    {
        #region Public Properties

        /// <summary>
        ///   User value xml serialization.
        /// </summary>
        [XmlElement("UserValue")]
        public XmlWrapper ConcreteUserValueSerialized
        {
            get
            {
                return new XmlWrapper(this.ConcreteUserValue);
            }

            set
            {
                this.ConcreteUserValue = value != null ? value.Task : null;
            }
        }

        #endregion
    }
}