namespace RainyGames.AI.BehaviorTrees.Editor
{
    using System;
    using System.Reflection;
    using System.Xml.Serialization;

    using RainyGames.AI.BehaviorTrees.Attributes;
    using RainyGames.AI.BehaviorTrees.Data;
    using RainyGames.Xml;

    /// <summary>
    ///   Description of a task parameter.
    /// </summary>
    [Serializable]
    public class TaskParameterDescription
    {
        #region Public Properties

        /// <summary>
        ///   Default value.
        /// </summary>
        [XmlIgnore]
        public object Default { get; set; }

        /// <summary>
        ///   Default value.
        /// </summary>
        [XmlElement("Default")]
        public XmlAnything<object> DefaultSerialized
        {
            get
            {
                return new XmlAnything<object>(this.Default);
            }

            set
            {
                this.Default = value.Value;
            }
        }

        /// <summary>
        ///   Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Returns the final parameter type. If the parameter type is a nullable type, the underlying type is returned, otherwise the parameter type itself is returned.
        /// </summary>
        [XmlIgnore]
        public Type FinalParameterType
        {
            get
            {
                // Get type.
                Type type = this.ParameterType;
                if (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return Nullable.GetUnderlyingType(type);
                }

                return type;
            }
        }

        /// <summary>
        ///   Meta type (used for parameters which are initialized by sub configurations).
        /// </summary>
        [XmlIgnore]
        public Type MetaType
        {
            get
            {
                return this.MetaTypeString != null ? System.Type.GetType(this.MetaTypeString) : null;
            }
        }

        /// <summary>
        ///   String of meta type (used for parameters which are initialized by sub configurations).
        /// </summary>
        public string MetaTypeString { get; set; }

        /// <summary>
        ///   Readable name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Parameter name.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        ///   Parameter type.
        /// </summary>
        [XmlIgnore]
        public Type ParameterType
        {
            get
            {
                return System.Type.GetType(this.Type);
            }
        }

        /// <summary>
        ///   Parameter type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///   Defines how the task parameter should be visualized.
        /// </summary>
        public VisualizationType VisualizationType { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The generate.
        /// </summary>
        /// <param name="parameterMember"> The parameter member. </param>
        /// <returns> The RainyGames.AI.BehaviorTrees.Editor.TaskParameterDescription. </returns>
        /// <exception cref="InvalidCastException"></exception>
        public static TaskParameterDescription Generate(MemberInfo parameterMember)
        {
            // Skip members which are no properties.
            if (parameterMember.MemberType != MemberTypes.Property)
            {
                return null;
            }

            PropertyInfo propertyInfo = parameterMember as PropertyInfo;
            if (propertyInfo == null)
            {
                return null;
            }

            // Check if property has task parameter attribute.
            TaskParameterAttribute[] parameterAttributes =
                parameterMember.GetCustomAttributes(typeof(TaskParameterAttribute), true) as TaskParameterAttribute[];
            if (parameterAttributes == null || parameterAttributes.Length == 0)
            {
                return null;
            }

            TaskParameterAttribute parameterAttribute = parameterAttributes[0];

            Type parameterType = propertyInfo.PropertyType;
            string parameterName = string.IsNullOrEmpty(parameterAttribute.Name)
                                       ? parameterMember.Name
                                       : parameterAttribute.Name;

            // Check if default value is of parameter type, otherwise cast it or throw exception.
            if (parameterAttribute.Default != null)
            {
                Type valueType = parameterType;
                if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(TaskParameter<>))
                {
                    valueType = parameterType.GetGenericArguments()[0];
                }

                if (parameterAttribute.Default.GetType() != valueType)
                {
                    throw new InvalidCastException(
                        string.Format(
                            "Default value of parameter '{0}' (type: '{1}') couldn't be casted to parameter type '{2}'.",
                            parameterName,
                            parameterAttribute.Default.GetType(),
                            parameterType));
                }
            }

            // Determine meta type name.
            Type metaType = null;
            if (parameterAttribute.MetaType != null)
            {
                metaType = parameterAttribute.MetaType;
                if (metaType.IsGenericType)
                {
                    metaType = metaType.GetGenericTypeDefinition();
                }
            }

            // Create description.
            TaskParameterDescription parameterDescription = new TaskParameterDescription
                {
                    ParameterName = parameterMember.Name,
                    Name = parameterName,
                    Description = parameterAttribute.Description,
                    Default = parameterAttribute.Default,
                    MetaTypeString = metaType != null ? metaType.AssemblyQualifiedName : null,
                    Type = parameterType.AssemblyQualifiedName,
                    VisualizationType = parameterAttribute.VisualizationType
                };

            return parameterDescription;
        }

        /// <summary>
        ///   Creates a default value of this task parameter.
        /// </summary>
        /// <returns> Created default value. </returns>
        public object CreateDefaultValue()
        {
            // Handle special TaskParameter class.
            Type type = this.ParameterType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(TaskParameter<>))
            {
                TaskParameter taskParameter = (TaskParameter)Activator.CreateInstance(this.ParameterType);
                if (this.Default != null)
                {
                    taskParameter.UserValue = this.Default;
                }

                return taskParameter;
            }

            return this.Default ?? Activator.CreateInstance(type);
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

            if (obj.GetType() != typeof(TaskParameterDescription))
            {
                return false;
            }

            return this.Equals((TaskParameterDescription)obj);
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(TaskParameterDescription other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.Description, this.Description) && Equals(other.Name, this.Name)
                   && Equals(other.ParameterName, this.ParameterName) && Equals(other.Type, this.Type)
                   && Equals(other.Default, this.Default);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Description != null ? this.Description.GetHashCode() : 0;
                result = (result * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                result = (result * 397) ^ (this.ParameterName != null ? this.ParameterName.GetHashCode() : 0);
                result = (result * 397) ^ (this.Type != null ? this.Type.GetHashCode() : 0);
                result = (result * 397) ^ (this.Default != null ? this.Default.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }
}