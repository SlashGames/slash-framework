// -----------------------------------------------------------------------
// <copyright file="FibonacciHeapNode.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Slash.Collections.PriorityQueues
{
    /// <summary>
    /// Node of an heap-ordered tree. Contains an item with a key which allows
    /// comparing it to other heap items for order. Provides pointers to its
    /// parent node, to its left and right siblings, and to one of its children.
    /// Can be marked in order to decide whether to make a cascading cut after
    /// the edge to this node's parent has been cut, or not.
    /// </summary>
    /// <typeparam name="T">Type of the item held by this node.</typeparam>
    internal class FibonacciHeapNode<T>
    {
        #region Constructors

        /// <summary>
        /// Constructs a new heap-ordered tree node holding the passed item.
        /// The node initially has no siblings.
        /// </summary>
        /// <param name="item">the container holding this node's item and its key</param>
        public FibonacciHeapNode(FibonacciHeapItem<T> item)
        {
            this.Item = item;
            item.ContainingNode = this;

            this.LeftSibling = this;
            this.RightSibling = this;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The container holding this node's item and its key.
        /// </summary>
        internal FibonacciHeapItem<T> Item { get; set; }

        /// <summary>
        /// The parent of this node.
        /// </summary>
        internal FibonacciHeapNode<T> Parent { get; set; }

        /// <summary>
        /// The left sibling of this node.
        /// </summary>
        internal FibonacciHeapNode<T> LeftSibling { get; set; }

        /// <summary>
        /// One of the children of this node.
        /// </summary>
        internal FibonacciHeapNode<T> SomeChild { get; set; }

        /// <summary>
        /// The right sibling of this node.
        /// </summary>
        internal FibonacciHeapNode<T> RightSibling { get; set; }

        /// <summary>
        /// The number of children of this node.
        /// </summary>
        internal int Rank { get; set; }

        /// <summary>
        /// Whether to perform a cascading cut after the edge to this node's
        /// parent has been cut, or not.
        /// </summary>
        internal bool Marked { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds to passed heap-ordered tree node to the list of this node's
        /// children, increasing the rank of this node.
        /// </summary>
        /// <param name="node">the new child of this node</param>
        public void AddChild(FibonacciHeapNode<T> node)
        {
            // update the rank of this node
            this.Rank++;

            // set the parent of the new node
            node.Parent = this;

            if (this.SomeChild == null)
            {
                // new node is the only child (has no siblings)
                node.LeftSibling = node;
                node.RightSibling = node;
            }
            else
            {
                // append new node to the right
                node.LeftSibling = this.SomeChild;
                node.RightSibling = this.SomeChild.RightSibling;
                node.RightSibling.LeftSibling = node;
                this.SomeChild.RightSibling = node;
            }

            this.SomeChild = node;
        }

        /// <summary>
        /// Cuts the edge to this node's parent, decreasing the rank of its
        /// parent. Performs a cascading cut if necessary.
        /// </summary>
        /// <param name="addToRootList">
        /// whether this node should be added to the list of roots of its
        /// heap, or not
        /// </param>
        /// <param name="heap">
        /// the heap whose root list this node is added to, if
        /// <code>addToRootList</code> is set to true
        /// </param>
        public void CutEdgeToParent(bool addToRootList, FibonacciHeap<T> heap)
        {
            // remove this node from the list of children of its parent
            if (this.LeftSibling != this)
            {
                this.LeftSibling.RightSibling = this.RightSibling;
                this.RightSibling.LeftSibling = this.LeftSibling;

                this.Parent.SomeChild = this.LeftSibling;
            }
            else
            {
                this.Parent.SomeChild = null;
            }

            // decrease the rank of this node's parent
            this.Parent.Rank--;

            if (this.Parent.Parent != null)
            {
                // parent is not a root
                if (!this.Parent.Marked)
                {
                    // mark it if it is unmarked
                    this.Parent.Marked = true;
                }
                else
                {
                    // cut the edge to its parent if it is marked
                    this.Parent.CutEdgeToParent(true, heap);
                }
            }

            this.Parent = null;

            if (addToRootList)
            {
                // add this node to the list of roots of this heap
                heap.MinimumNode.RightSibling.LeftSibling = this;
                this.RightSibling = heap.MinimumNode.RightSibling;

                heap.MinimumNode.RightSibling = this;
                this.LeftSibling = heap.MinimumNode;
            }
        }

        /// <summary>
        /// Combines the heap-ordered tree represented by the passed root node
        /// with the tree represented by this one. Assumes that both trees are
        /// item-disjoint.
        /// </summary>
        /// <param name="otherTreeRoot">the root of the other tree to combine with this one</param>
        /// <returns>the root of the resulting heap-ordered tree</returns>
        public FibonacciHeapNode<T> Link(FibonacciHeapNode<T> otherTreeRoot)
        {
            if (this.Item.Key < otherTreeRoot.Item.Key)
            {
                this.AddChild(otherTreeRoot);
                otherTreeRoot.Marked = false;
                return this;
            }
            else
            {
                otherTreeRoot.AddChild(this);
                this.Marked = false;
                return otherTreeRoot;
            }
        }

        #endregion
    }
}
