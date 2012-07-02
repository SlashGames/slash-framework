namespace RainyGames.AI.BehaviorTrees.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using RainyGames.AI.BehaviorTrees.Attributes;
    using RainyGames.AI.BehaviorTrees.Interfaces;
    using RainyGames.Collections.Utils;

    /// <summary>
    ///   Contains information about a decider which should be communicated to the level editor.
    /// </summary>
    [Serializable]
    public class TaskDescription
    {
        #region Public Properties

        /// <summary>
        ///   Class name of the task.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///   Indicates if the task is a decorator and thus can have a child task.
        /// </summary>
        public bool IsDecorator { get; set; }

        /// <summary>
        ///   Name of the task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   List of descriptions of the parameters of the task.
        /// </summary>
        public List<TaskParameterDescription> ParameterDescriptions { get; set; }

        /// <summary>
        ///   Full qualified type of the task.
        /// </summary>
        public string TypeName { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Generates a task description for the passed task and its parameters type.
        /// </summary>
        /// <typeparam name="T"> Task type. </typeparam>
        /// <returns> Task description. </returns>
        public static TaskDescription Generate<T>() where T : ITask
        {
            return Generate(typeof(T));
        }

        /// <summary>
        ///   The generate.
        /// </summary>
        /// <param name="typeDecider"> The type decider. </param>
        /// <returns> The RainyGames.AI.BehaviorTrees.Editor.TaskDescription. </returns>
        public static TaskDescription Generate(Type typeDecider)
        {
            // Check for task attribute.
            TaskAttribute[] taskAttributes =
                typeDecider.GetCustomAttributes(typeof(TaskAttribute), true) as TaskAttribute[];
            if (taskAttributes == null || taskAttributes.Length == 0)
            {
                return null;
            }

            TaskAttribute taskAttribute = taskAttributes[0];

            TaskDescription description = new TaskDescription
                {
                    Name = taskAttribute.Name,
                    IsDecorator = taskAttribute.IsDecorator,
                    ClassName = typeDecider.Name,
                    TypeName = typeDecider.AssemblyQualifiedName,
                    ParameterDescriptions = new List<TaskParameterDescription>()
                };

            MemberInfo[] parameterMembers = typeDecider.GetMembers();
            foreach (MemberInfo parameterMember in parameterMembers)
            {
                TaskParameterDescription parameterDescription = TaskParameterDescription.Generate(parameterMember);
                if (parameterDescription == null)
                {
                    continue;
                }

                description.ParameterDescriptions.Add(parameterDescription);
            }

            return description;
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(TaskDescription other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.Name, this.Name)
                   && CollectionUtils.SequenceEqual(other.ParameterDescriptions, this.ParameterDescriptions)
                   && Equals(other.IsDecorator, this.IsDecorator) && Equals(other.TypeName, this.TypeName)
                   && Equals(other.ClassName, this.ClassName);
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

            if (obj.GetType() != typeof(TaskDescription))
            {
                return false;
            }

            return this.Equals((TaskDescription)obj);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Name != null ? this.Name.GetHashCode() : 0;
                result = (result * 397)
                         ^ (this.ParameterDescriptions != null ? this.ParameterDescriptions.GetHashCode() : 0);
                result = (result * 397) ^ this.IsDecorator.GetHashCode();
                result = (result * 397) ^ (this.TypeName != null ? this.TypeName.GetHashCode() : 0);
                result = (result * 397) ^ (this.ClassName != null ? this.ClassName.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }
}