namespace Slash.AI.BehaviorTrees.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Editor;
    using Slash.AI.BehaviorTrees.Implementations;
    using Slash.AI.BehaviorTrees.Implementations.Actions;
    using Slash.AI.BehaviorTrees.Interfaces;
    using Slash.AI.BehaviorTrees.Tree;
    using Slash.Collections.Utils;

    /// <summary>
    ///   Library which stores behavior trees by key.
    /// </summary>
    [Serializable]
    public class BehaviorTreeLibrary
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="BehaviorTreeLibrary" /> class.
        /// </summary>
        public BehaviorTreeLibrary()
        {
            this.Entries = new List<Entry>();
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   Called when the library was changed by the editor.
        /// </summary>
        /// <param name="library"> Library which changed. </param>
        public delegate void LibraryChangedDelegate(BehaviorTreeLibrary library);

        #endregion

        #region Public Events

        /// <summary>
        ///   Called when the library was changed by the editor.
        /// </summary>
        [field: NonSerialized]
        public event LibraryChangedDelegate LibraryChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Returns the number of available behavior trees.
        /// </summary>
        [XmlIgnore]
        public int Count
        {
            get
            {
                return this.Entries.Count;
            }
        }

        /// <summary>
        ///   Returns all available trees as a read-only list.
        /// </summary>
        public List<Entry> Entries { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds a behavior tree to the library and uses the passed name as its key.
        /// </summary>
        /// <param name="behaviorTreeName"> Name of the behavior tree. </param>
        /// <param name="behaviorTree"> Behavior tree. </param>
        public void Add(string behaviorTreeName, ITask behaviorTree)
        {
            this.Entries.Add(new Entry { Identifier = behaviorTreeName, Tree = behaviorTree });

            // Invoke event.
            this.InvokeLibraryChanged();
        }

        /// <summary>
        ///   Determines whether the library contains a behavior tree with the passed name.
        /// </summary>
        /// <param name="behaviorTreeName"> Behavior tree name. </param>
        /// <returns> True if the library contains a behavior tree with the passed name; otherwise, false. </returns>
        public bool Contains(string behaviorTreeName)
        {
            return this.Entries.Exists(entry => entry.Identifier == behaviorTreeName);
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(BehaviorTreeLibrary other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return CollectionUtils.SequenceEqual(other.Entries, this.Entries);
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

            if (obj.GetType() != typeof(BehaviorTreeLibrary))
            {
                return false;
            }

            return this.Equals((BehaviorTreeLibrary)obj);
        }

        /// <summary>
        ///   Returns the behavior tree with the passed name from this library.
        /// </summary>
        /// <param name="behaviorTreeName"> Behavior tree name. </param>
        /// <returns> Behavior tree which was stored under the passed name. </returns>
        public ITask Get(string behaviorTreeName)
        {
            Entry foundEntry = this.Entries.First(entry => entry.Identifier == behaviorTreeName);
            return foundEntry == null ? null : foundEntry.Tree;
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            return this.Entries != null ? this.Entries.GetHashCode() : 0;
        }

        /// <summary>
        ///   Removes the behavior tree with the passed name from the library.
        /// </summary>
        /// <param name="behaviorTreeName"> Name of the behavior tree. </param>
        /// <returns> Returns if the behavior tree was successfully removed. </returns>
        public bool Remove(string behaviorTreeName)
        {
            bool entryRemoved = this.Entries.RemoveAll(entry => entry.Identifier == behaviorTreeName) > 0;
            if (!entryRemoved)
            {
                return false;
            }

            // Invoke event.
            this.InvokeLibraryChanged();

            return true;
        }

        /// <summary>
        ///   Removes the behavior tree from the library.
        /// </summary>
        /// <param name="behaviorTree"> Behavior tree. </param>
        /// <returns> Returns if the behavior tree was successfully removed. </returns>
        public bool Remove(ITask behaviorTree)
        {
            bool entryRemoved = this.Entries.RemoveAll(entry => entry.Tree == behaviorTree) > 0;
            if (!entryRemoved)
            {
                return false;
            }

            // Invoke event.
            this.InvokeLibraryChanged();

            return true;
        }

        /// <summary>
        ///   Sets a behavior tree to the library and uses the passed name as its key. Overwrites a behavior tree if the library already contains a behavior tree with the passed name.
        /// </summary>
        /// <param name="behaviorTreeName"> Name of the behavior tree. </param>
        /// <param name="behaviorTree"> Behavior tree. </param>
        public void Set(string behaviorTreeName, ITask behaviorTree)
        {
            this.Entries.RemoveAll(entry => entry.Identifier == behaviorTreeName);
            this.Entries.Add(new Entry { Identifier = behaviorTreeName, Tree = behaviorTree });

            // Invoke event.
            this.InvokeLibraryChanged();
        }

        /// <summary>
        ///   Solves all sub tree reference tasks and replaces them by real references to the sub tree.
        /// </summary>
        public void SolveReferences()
        {
            foreach (Entry entry in this.Entries)
            {
                entry.Tree = this.SolveReferences(entry.Tree);
            }
        }

        /// <summary>
        ///   Solves all sub tree reference tasks and replaces them by real references to the sub tree.
        /// </summary>
        /// <param name="tree"> Tree in which to solve references. </param>
        public void SolveReferences(IBehaviorTree tree)
        {
            tree.Root = this.SolveReferences(tree.Root);
        }

        /// <summary>
        ///   Tries to get the behavior tree with the passed name from this library.
        /// </summary>
        /// <param name="behaviorTreeName"> Behavior tree name. </param>
        /// <param name="behaviorTree"> Behavior tree. </param>
        /// <returns> True if a behavior tree was found, else false. </returns>
        public bool TryGet(string behaviorTreeName, out ITask behaviorTree)
        {
            Entry foundEntry = this.Entries.Find(entry => entry.Identifier == behaviorTreeName);
            if (foundEntry == null)
            {
                behaviorTree = null;
                return false;
            }

            behaviorTree = foundEntry.Tree;
            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The invoke library changed.
        /// </summary>
        private void InvokeLibraryChanged()
        {
            LibraryChangedDelegate handler = this.LibraryChanged;
            if (handler != null)
            {
                handler(this);
            }
        }

        /// <summary>
        ///   Solves a sub tree reference by searching for the tree with the name specified in the reference. Also encapsulates the tree if necessary (e.g. because a blackboard is provided).
        /// </summary>
        /// <param name="subTreeReference"> Reference to resolve. </param>
        /// <returns> Task to use instead of the reference. </returns>
        private ITask SolveReference(SubTreeReference subTreeReference)
        {
            // Find tree reference.
            ITask treeReference = this.Get(subTreeReference.SubTreeName);
            if (treeReference == null && !string.IsNullOrEmpty(subTreeReference.SubTreeName))
            {
                throw new Exception(
                    string.Format(
                        "No tree reference found for tree named '{0}' in sub tree reference task.",
                        subTreeReference.SubTreeName));
            }

            // Encapsulate with CreateBlackboard task if blackboard is set.
            if (subTreeReference.Blackboard != null)
            {
                treeReference = new CreateBlackboard
                    { Task = treeReference, Blackboard = subTreeReference.Blackboard };
            }

            return treeReference;
        }

        /// <summary>
        ///   Solves all sub tree reference tasks and replaces them by real references to the sub tree.
        /// </summary>
        /// <param name="tree"> Tree in which to solve references. </param>
        /// <returns> Task to use instead of the passed one. </returns>
        private ITask SolveReferences(ITask tree)
        {
            if (tree == null)
            {
                return tree;
            }

            // Check if root is reference.
            if (tree is SubTreeReference)
            {
                SubTreeReference subTreeReference = tree as SubTreeReference;

                // Resolve reference.
                ITask treeReference = this.SolveReference(subTreeReference);

                return treeReference;
            }

            // Find sub tree reference tasks in tree.
            ICollection<TaskNode> referenceNodes = new List<TaskNode>();
            TaskNode rootNode = new TaskNode { Task = tree };
            tree.FindTasks(rootNode, task => task is SubTreeReference, ref referenceNodes);

            // Replace tasks in found nodes by referenced tree.
            foreach (TaskNode referenceNode in referenceNodes)
            {
                SubTreeReference subTreeReference = referenceNode.Task as SubTreeReference;
                if (subTreeReference == null)
                {
                    throw new Exception(
                        "Searched for SubTreeReference nodes, but found a task which is no sub tree reference node.");
                }

                IComposite parentComposite = referenceNode.ParentTask as IComposite;
                if (parentComposite == null)
                {
                    throw new Exception("Found task which has no parent composite.");
                }

                // Resolve reference.
                ITask treeReference = this.SolveReference(subTreeReference);

                // Remove from parent.
                parentComposite.RemoveChild(referenceNode.Task);

                // Add tree reference to parent at same position.
                parentComposite.InsertChild(referenceNode.Index, treeReference);
            }

            return tree;
        }

        #endregion

        /// <summary>
        ///   Entry in library.
        /// </summary>
        [Serializable]
        public class Entry
        {
            #region Public Properties

            /// <summary>
            ///   Identifier.
            /// </summary>
            public string Identifier { get; set; }

            /// <summary>
            ///   Tree.
            /// </summary>
            [XmlIgnore]
            public ITask Tree { get; set; }

            /// <summary>
            ///   Tree.
            /// </summary>
            [XmlElement("Tree")]
            public XmlWrapper TreeSerialized
            {
                get
                {
                    return new XmlWrapper(this.Tree);
                }

                set
                {
                    this.Tree = value.Task;
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///   The equals.
            /// </summary>
            /// <param name="other"> The other. </param>
            /// <returns> The System.Boolean. </returns>
            public bool Equals(Entry other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return Equals(other.Identifier, this.Identifier) && Equals(other.Tree, this.Tree);
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

                if (obj.GetType() != typeof(Entry))
                {
                    return false;
                }

                return this.Equals((Entry)obj);
            }

            /// <summary>
            ///   The get hash code.
            /// </summary>
            /// <returns> The System.Int32. </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return ((this.Identifier != null ? this.Identifier.GetHashCode() : 0) * 397)
                           ^ (this.Tree != null ? this.Tree.GetHashCode() : 0);
                }
            }

            #endregion
        }
    }
}