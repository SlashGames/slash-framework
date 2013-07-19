// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskNode.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Tree
{
    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   A tree node which provides a way to store a path through a behavior tree.
    /// </summary>
    public class TaskNode
    {
        #region Public Properties

        /// <summary>
        ///   Depth inside tree.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        ///   Index among siblings.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///   Returns the next sibling.
        /// </summary>
        public ITask NextSiblingTask
        {
            get
            {
                IComposite parentComposite = this.ParentTask as IComposite;
                return parentComposite != null && this.Index < parentComposite.Children.Count - 1
                           ? parentComposite.Children[this.Index + 1]
                           : null;
            }
        }

        /// <summary>
        ///   Parent node.
        /// </summary>
        public TaskNode Parent { get; set; }

        /// <summary>
        ///   Returns the parent task.
        /// </summary>
        public ITask ParentTask
        {
            get
            {
                return this.Parent != null ? this.Parent.Task : null;
            }
        }

        /// <summary>
        ///   Returns the previous sibling.
        /// </summary>
        public ITask PreviousSiblingTask
        {
            get
            {
                IComposite parentComposite = this.ParentTask as IComposite;
                return parentComposite != null && this.Index > 0 ? parentComposite.Children[this.Index - 1] : null;
            }
        }

        /// <summary>
        ///   Task in this node.
        /// </summary>
        public ITask Task { get; set; }

        /// <summary>
        ///   Creates a tree id out of the depth and index of the node. For each level 4 bits are reserved, so there should be a maximum of 16 children. The maximum depth is 8 on 32bit and 16 on 64bit.
        /// </summary>
        public int TreeId
        {
            get
            {
                int parentTreeId = this.Parent != null ? this.Parent.TreeId : 0;
                return (parentTreeId << 4) | (this.Index + 1);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Creates a child node of this one.
        /// </summary>
        /// <param name="childTask"> Task in the child node. </param>
        /// <param name="childIndex"> Index of the child node. </param>
        /// <returns> Child node of this one. </returns>
        public TaskNode CreateChildNode(ITask childTask, int childIndex)
        {
            return new TaskNode { Parent = this, Depth = this.Depth + 1, Task = childTask, Index = childIndex };
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(TaskNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Depth == this.Depth && other.Index == this.Index && Equals(other.Parent, this.Parent)
                   && ReferenceEquals(other.Task, this.Task);
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

            if (obj.GetType() != typeof(TaskNode))
            {
                return false;
            }

            return this.Equals((TaskNode)obj);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Depth;
                result = (result * 397) ^ this.Index;
                result = (result * 397) ^ (this.Parent != null ? this.Parent.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }
}