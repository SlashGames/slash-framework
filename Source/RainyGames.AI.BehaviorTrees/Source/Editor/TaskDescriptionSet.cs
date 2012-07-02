namespace RainyGames.AI.BehaviorTrees.Editor
{
    using System.Collections.Generic;
    using System.Xml;

    using RainyGames.AI.BehaviorTrees.Implementations;
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Set of task descriptions.
    /// </summary>
    public class TaskDescriptionSet
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="TaskDescriptionSet" /> class. Constructor.
        /// </summary>
        public TaskDescriptionSet()
        {
            this.Descriptions = new List<TaskDescription>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   task descriptions.
        /// </summary>
        public List<TaskDescription> Descriptions { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Callback when an unknown task should be read from xml. Uses the task descriptions in this set to wrap the unknown task in a ReferenceTask object.
        /// </summary>
        /// <param name="reader"> Xml reader. </param>
        /// <param name="task"> Task which is read. </param>
        public void OnXmlReadUnknownTask(XmlReader reader, out ITask task)
        {
            try
            {
                task = ReferenceTask.ReadUnknownTask(reader, this);
            }
            catch (XmlException)
            {
                /*
                CLog.Error(
                    string.Format("Reading reference task from xml '{0}' failed: '{1}'.", reader.Name, ex.Message));*/
                task = null;
            }
        }

        #endregion
    }
}