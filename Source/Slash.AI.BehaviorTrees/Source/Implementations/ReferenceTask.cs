// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceTask.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Editor;
    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;
    using Slash.AI.BehaviorTrees.Tree;
    using Slash.Collections.Utils;

    /// <summary>
    ///   Task which references another task. It's required to build a behavior tree in an editor where the concrete tasks are not known, but only their descriptions. Through this class it's possible to write a xml file for the application anyway.
    /// </summary>
    [Serializable]
    public class ReferenceTask : ITask, IXmlSerializable
    {
        #region Fields

        /// <summary>
        ///   Mapping of the value to the paramter.
        /// </summary>
        private readonly Dictionary<TaskParameterDescription, object> parameterValues =
            new Dictionary<TaskParameterDescription, object>();

        /// <summary>
        ///   Description of the task which is referenced.
        /// </summary>
        private TaskDescription taskDescription;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ReferenceTask" /> class. Constructor.
        /// </summary>
        public ReferenceTask()
        {
            this.Name = string.Empty;
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   Called when the value of a task parameter changed.
        /// </summary>
        /// <param name="task"> Task which parameter changed. </param>
        /// <param name="parameterDescription"> Description of parameter which value changed. </param>
        public delegate void ParameterChangedDelegate(ReferenceTask task, TaskParameterDescription parameterDescription);

        /// <summary>
        ///   Called when the task description of the reference task changed.
        /// </summary>
        /// <param name="task"> Reference task which task description changed. </param>
        /// <param name="taskDescription"> New task description. </param>
        public delegate void TaskDescriptionChangedDelegate(ReferenceTask task, TaskDescription taskDescription);

        #endregion

        #region Public Events

        /// <summary>
        ///   Called when task finished successful.
        /// </summary>
        public event OnSuccess OnSuccess
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///   Called when the value of a task parameter changed.
        /// </summary>
        [field: NonSerialized]
        public event ParameterChangedDelegate ParameterChanged;

        /// <summary>
        ///   Called when the task description of the reference task changed.
        /// </summary>
        [field: NonSerialized]
        public event TaskDescriptionChangedDelegate TaskDescriptionChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Capsuled task if referenced task is a decorator.
        /// </summary>
        public ITask DecoratorTask { get; set; }

        /// <summary>
        ///   Debug name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Description of the task which is referenced.
        /// </summary>
        public TaskDescription TaskDescription
        {
            get
            {
                return this.taskDescription;
            }

            set
            {
                if (value == this.taskDescription)
                {
                    return;
                }

                this.taskDescription = value;

                if (this.taskDescription != null && this.taskDescription.ParameterDescriptions != null)
                {
                    // Set default values for parameters.
                    foreach (
                        TaskParameterDescription taskParameterDescription in this.taskDescription.ParameterDescriptions)
                    {
                        if (taskParameterDescription.Default != null
                            && !this.parameterValues.ContainsKey(taskParameterDescription))
                        {
                            this.parameterValues.Add(
                                taskParameterDescription, taskParameterDescription.CreateDefaultValue());
                        }
                    }
                }

                // Invoke event.
                this.InvokeTaskDescriptionChanged(this.taskDescription);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Reads an unknown task from xml and tries to capsule it into a ReferenceTask object. If no task description for the task type was found, null is returned.
        /// </summary>
        /// <param name="reader"> Xml reader. </param>
        /// <param name="availableTaskDescriptions"> Available task descriptions. </param>
        /// <returns> Reference task which capsules the unknown task; null if no task description for the type of the unknown task was found. </returns>
        public static ReferenceTask ReadUnknownTask(XmlReader reader, TaskDescriptionSet availableTaskDescriptions)
        {
            string typeString = reader.GetAttribute("type");

            TaskDescription taskDescription =
                availableTaskDescriptions.Descriptions.Find(description => description.TypeName == typeString);
            if (taskDescription == null)
            {
                reader.Skip();
                return null;
            }

            ReferenceTask referenceTask = new ReferenceTask { TaskDescription = taskDescription };

            if (reader.IsEmptyElement)
            {
                reader.Skip();
                return referenceTask;
            }

            reader.ReadStartElement();
            referenceTask.ReadXml(reader);
            reader.ReadEndElement();

            return referenceTask;
        }

        /// <summary>
        ///   Activation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Execution status after activation. </returns>
        public ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public void Deactivate(IAgentData agentData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the task will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the task will be activated. </returns>
        public float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(ReferenceTask other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return CollectionUtils.SequenceEqual(other.parameterValues, this.parameterValues)
                   && Equals(other.taskDescription, this.taskDescription)
                   && Equals(other.DecoratorTask, this.DecoratorTask) && Equals(other.Name, this.Name);
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

            if (obj.GetType() != typeof(ReferenceTask))
            {
                return false;
            }

            return this.Equals((ReferenceTask)obj);
        }

        /// <summary>
        ///   Searches for tasks which forfill the passed predicate.
        /// </summary>
        /// <param name="taskNode"> Task node of this task. </param>
        /// <param name="predicate"> Predicate to forfill. </param>
        /// <param name="tasks"> List of tasks which forfill the passed predicate. </param>
        public void FindTasks(TaskNode taskNode, Func<ITask, bool> predicate, ref ICollection<TaskNode> tasks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Generates a collection of active task nodes under this task. Used for debugging only.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="taskNode"> Task node of this task. </param>
        /// <param name="activeTasks"> Collection of active task nodes. </param>
        public void GetActiveTasks(IAgentData agentData, TaskNode taskNode, ref ICollection<TaskNode> activeTasks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.parameterValues != null ? this.parameterValues.GetHashCode() : 0;
                result = (result * 397) ^ (this.taskDescription != null ? this.taskDescription.GetHashCode() : 0);
                result = (result * 397) ^ (this.DecoratorTask != null ? this.DecoratorTask.GetHashCode() : 0);
                result = (result * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                return result;
            }
        }

        /// <summary>
        ///   The get schema.
        /// </summary>
        /// <returns> The System.Xml.Schema.XmlSchema. </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        ///   Returns the stored parameter value or, if none is set, the default value.
        /// </summary>
        /// <param name="parameterDescription"> Description of parameter to get value for. </param>
        /// <returns> Parameter value or default one, if none was set before. </returns>
        public object GetValueOrDefault(TaskParameterDescription parameterDescription)
        {
            if (this.taskDescription == null)
            {
                throw new InvalidOperationException("Can't get parameter value, because no task description was set.");
            }

            if (parameterDescription == null)
            {
                throw new NullReferenceException("Passed parameter description was null.");
            }

            if (!this.taskDescription.ParameterDescriptions.Contains(parameterDescription))
            {
                throw new ArgumentException(
                    string.Format(
                        "No parameter with description '{0}' found in task description.", parameterDescription.Name));
            }

            object value;
            if (!this.parameterValues.TryGetValue(parameterDescription, out value))
            {
                value = parameterDescription.CreateDefaultValue();
            }

            return value;
        }

        /// <summary>
        ///   The read xml.
        /// </summary>
        /// <param name="reader"> The reader. </param>
        public void ReadXml(XmlReader reader)
        {
            this.Name = string.Empty;
            this.parameterValues.Clear();

            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                return;
            }

            reader.ReadStartElement();

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    string elementName = reader.Name;
                    if (elementName == "Name")
                    {
                        this.Name = reader.ReadElementString();
                    }
                    else if (elementName == "Child")
                    {
                        XmlWrapper wrapper = new XmlWrapper();
                        wrapper.ReadXml(reader);
                        this.DecoratorTask = wrapper.Task;
                    }
                    else if (this.taskDescription != null)
                    {
                        // Check if parameter.
                        TaskParameterDescription parameterDescription =
                            this.taskDescription.ParameterDescriptions.Find(desc => desc.ParameterName == elementName);
                        if (parameterDescription != null)
                        {
                            Type type = Type.GetType(parameterDescription.Type);

                            reader.ReadStartElement(parameterDescription.ParameterName);

                            XmlSerializer xmlSerializer = new XmlSerializer(type);
                            object value = xmlSerializer.Deserialize(reader);

                            reader.ReadEndElement();

                            this.parameterValues[parameterDescription] = value;
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
                else
                {
                    reader.Skip();
                }
            }

            reader.ReadEndElement();
        }

        /// <summary>
        ///   Sets the value for the parameter with the specified description.
        /// </summary>
        /// <param name="parameterDescription"> Description of parameter to set value for. </param>
        /// <param name="value"> New parameter value. </param>
        public void SetValue(TaskParameterDescription parameterDescription, object value)
        {
            // Check if task description contains parameter.
            if (this.taskDescription == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Reference task '{0}' doesn't have a task description, couldn't set parameter value.", this.Name));
            }

            if (parameterDescription == null)
            {
                throw new NullReferenceException(
                    string.Format(
                        "Parameter description is null, couldn't set a value for it in reference task '{0}'.", this.Name));
            }

            if (!this.taskDescription.ParameterDescriptions.Contains(parameterDescription))
            {
                throw new ArgumentException(
                    string.Format(
                        "Task description of reference task '{0}' doesn't contain a parameter description '{1}'.",
                        this.Name,
                        parameterDescription.Name));
            }

            // Check if value changed.
            object oldValue;
            if (this.parameterValues.TryGetValue(parameterDescription, out oldValue) && value == oldValue)
            {
                return;
            }

            this.parameterValues[parameterDescription] = value;

            // Invoke event.
            this.InvokeParameterChanged(parameterDescription);
        }

        /// <summary>
        ///   Tries to fetch the value of the parameter with the passed description.
        /// </summary>
        /// <param name="parameterDescription"> Parameter description. </param>
        /// <param name="objectValue"> Contains the value of the parameter with the passed description if found; otherwise the default value. </param>
        /// <returns> True if the value for the parameter with the passed description was found; otherwise, false. </returns>
        public bool TryGetValue(TaskParameterDescription parameterDescription, out object objectValue)
        {
            return this.parameterValues.TryGetValue(parameterDescription, out objectValue);
        }

        /// <summary>
        ///   Tries to fetch the value of the parameter with the passed description.
        /// </summary>
        /// <typeparam name="T"> Expected type of value. </typeparam>
        /// <param name="parameterDescription"> Parameter description. </param>
        /// <param name="value"> Contains the value of the parameter with the passed description if found. </param>
        /// <returns> True if the value for the parameter with the passed description was found; otherwise, false. </returns>
        public bool TryGetValue<T>(TaskParameterDescription parameterDescription, ref T value)
        {
            object objectValue;
            bool result = this.parameterValues.TryGetValue(parameterDescription, out objectValue);
            if (result)
            {
                if (!(objectValue is T))
                {
                    return false;
                }

                value = (T)objectValue;
            }

            return result;
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public ExecutionStatus Update(IAgentData agentData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   The write xml.
        /// </summary>
        /// <param name="writer"> The writer. </param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");

            writer.WriteElementString("Name", this.Name ?? string.Empty);

            if (this.TaskDescription != null)
            {
                // Write parameters.
                if (this.TaskDescription.ParameterDescriptions != null)
                {
                    foreach (TaskParameterDescription parameterDescription in this.TaskDescription.ParameterDescriptions
                        )
                    {
                        // Get value.
                        object value;
                        if (!this.parameterValues.TryGetValue(parameterDescription, out value))
                        {
                            continue;
                        }

                        writer.WriteStartElement(parameterDescription.ParameterName);

                        Type type = Type.GetType(parameterDescription.Type);

                        XmlSerializer xmlSerializer = new XmlSerializer(type);
                        xmlSerializer.Serialize(writer, value);

                        writer.WriteEndElement();
                    }
                }

                if (this.TaskDescription.IsDecorator && this.DecoratorTask != null)
                {
                    writer.WriteStartElement("Child");
                    new XmlWrapper(this.DecoratorTask).WriteXml(writer);
                    writer.WriteEndElement();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The invoke parameter changed.
        /// </summary>
        /// <param name="parameterDescription"> The parameter description. </param>
        private void InvokeParameterChanged(TaskParameterDescription parameterDescription)
        {
            ParameterChangedDelegate handler = this.ParameterChanged;
            if (handler != null)
            {
                handler(this, parameterDescription);
            }
        }

        /// <summary>
        ///   The invoke task description changed.
        /// </summary>
        /// <param name="taskDescription"> The task description. </param>
        private void InvokeTaskDescriptionChanged(TaskDescription taskDescription)
        {
            TaskDescriptionChangedDelegate handler = this.TaskDescriptionChanged;
            if (handler != null)
            {
                handler(this, taskDescription);
            }
        }

        #endregion
    }
}