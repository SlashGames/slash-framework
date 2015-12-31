// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskDescription.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Slash.AI.BehaviorTrees.Attributes;
    using Slash.AI.BehaviorTrees.Implementations;
    using Slash.AI.BehaviorTrees.Interfaces;
    using Slash.Collections.Utils;

    /// <summary>
    ///   Contains information about a decider which should be communicated to the level editor.
    /// </summary>
    [Serializable]
    public class TaskDescription
    {
        #region Properties

        /// <summary>
        ///   Class name of the task.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///   Task description.
        /// </summary>
        public string Description { get; set; }

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

        /// <summary>
        ///   Task type. Returns null if the type can't be loaded because it's not available in the current loaded assemblies.
        /// </summary>
        protected Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Creates a task instance from this description. If the task type can't be found, a ReferenceTask instance is created
        ///   which capsules this task description.
        /// </summary>
        /// <returns> Task instance. </returns>
        public ITask CreateInstance()
        {
            // Find task type.
            Type taskType = this.Type ?? typeof(ReferenceTask);

            // Create instance.
            ITask task = (ITask)Activator.CreateInstance(taskType);
            task.Name = this.Name;

            return task;
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:TaskDescription" /> is equal to the current
        ///   <see cref="T:TaskDescription" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:TaskDescription" /> is equal to the current <see cref="T:TaskDescription" />;
        ///   otherwise, false.
        /// </returns>
        /// <param name="other">The <see cref="T:TaskDescription" /> to compare with the current <see cref="T:TaskDescription" />. </param>
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
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///   <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
        ///   otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        /// <filterpriority>2</filterpriority>
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
        ///   Generates a task description for the specified task type.
        /// </summary>
        /// <typeparam name="T"> Task type. </typeparam>
        /// <returns> Task description. </returns>
        public static TaskDescription Generate<T>() where T : ITask
        {
            return Generate(typeof(T));
        }

        /// <summary>
        ///   Generates a task description for the specified task type.
        /// </summary>
        /// <param name="taskType"> Task type. </param>
        /// <returns> Task description. </returns>
        public static TaskDescription Generate(Type taskType)
        {
            // Check for task attribute.
            TaskAttribute[] taskAttributes =
                taskType.GetCustomAttributes(typeof(TaskAttribute), true) as TaskAttribute[];
            if (taskAttributes == null || taskAttributes.Length == 0)
            {
                throw new ArgumentException(
                    "Type {0} doesn't have a task attribute which specifies the class as a behavior tree task.",
                    taskType.Name);
            }

            TaskAttribute taskAttribute = taskAttributes[0];

            TaskDescription description = new TaskDescription
            {
                Name = taskAttribute.Name,
                Description = taskAttribute.Description,
                IsDecorator = taskAttribute.IsDecorator,
                ClassName = taskType.Name,
                TypeName = taskType.AssemblyQualifiedName,
                ParameterDescriptions = new List<TaskParameterDescription>()
            };

            MemberInfo[] parameterMembers = taskType.GetMembers();
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
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
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