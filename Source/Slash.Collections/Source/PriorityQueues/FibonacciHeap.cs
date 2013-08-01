// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FibonacciHeap.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.PriorityQueues
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///   Very fast priority queue. Provides insertion, finding the minimum,
    ///   melding and decreasing keys in constant amortized time, and deleting
    ///   from an n-item heap in O(log n) amortized time.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     An implementation of a Fibonacci heap (abbreviated F-heap) by
    ///     Michael L. Fredman and Robert Endre Tarjan which represents a
    ///     very fast priority queue.
    ///   </para>
    ///   <para>
    ///     http://www.cl.cam.ac.uk/~sos22/supervise/dsaa/fib_heaps.pdf
    ///   </para>
    /// </remarks>
    /// <typeparam name="T">Type of the items held by this Fibonacci heap.</typeparam>
    public class FibonacciHeap<T> : IPriorityQueue<T>
    {
        #region Fields

        /// <summary>
        ///   Number of elements of this heap.
        /// </summary>
        private int count;

        /// <summary>
        ///   Root containing the item with the minumum key in this heap.
        /// </summary>
        private FibonacciHeapNode<T> minimumNode;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Number of elements of this heap.
        /// </summary>
        public int Count
        {
            get
            {
                return this.count;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Root containing the item with the minumum key in this heap.
        /// </summary>
        internal FibonacciHeapNode<T> MinimumNode
        {
            get
            {
                return this.minimumNode;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Clears this Fibonacci heap, removing all items.
        /// </summary>
        public void Clear()
        {
            this.minimumNode = null;
            this.count = 0;
        }

        /// <summary>
        ///   Decreases the key of the specified item in this heap by subtracting
        ///   the passed non-negative real number <c>delta</c>.
        /// </summary>
        /// <param name="item">Item to decrease the key of.</param>
        /// <param name="delta">Non-negative real number to be subtracted from the item's key.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <c>delta</c> is negative.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///   This heap is empty.
        /// </exception>
        public void DecreaseKey(IPriorityQueueItem<T> item, double delta)
        {
            if (delta < 0)
            {
                throw new ArgumentOutOfRangeException("delta", "delta has to be non-negative.");
            }

            if (this.IsEmpty())
            {
                throw new InvalidOperationException("This heap is empty.");
            }

            if (item is FibonacciHeapItem<T>)
            {
                FibonacciHeapItem<T> fibHeapItem = (FibonacciHeapItem<T>)item;

                // Subtract delta from the key of the passed item.
                fibHeapItem.Key -= delta;

                // Cut the edge joining the containing node x to its parent p.
                FibonacciHeapNode<T> x = fibHeapItem.ContainingNode;

                if (x.Parent != null)
                {
                    /*
                     * If x is not a root, remove it from the list of children of
                     * its parent, decrease its parent's rank, and add it to the list
                     * of roots of this heap in order to preserve the heap order.
                     */
                    x.CutEdgeToParent(true, this);
                }

                // Redefine the minimum node of this heap, if necessary.
                if (fibHeapItem.Key < this.minimumNode.Item.Key)
                {
                    this.minimumNode = x;
                }
            }
            else
            {
                throw new ArgumentException("Can only work on Fibonacci Heap items.", "item");
            }
        }

        /// <summary>
        ///   Decreases the key of the specified item in this heap to the passed
        ///   non-negative real number.
        /// </summary>
        /// <param name="item">Item to decrease the key of.</param>
        /// <param name="newKey">Item's new key.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   The resulting key would be greater than the current one.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///   This heap is empty.
        /// </exception>
        public void DecreaseKeyTo(IPriorityQueueItem<T> item, double newKey)
        {
            this.DecreaseKey(item, item.Key - newKey);
        }

        /// <summary>
        ///   Deletes the specified item from this heap.
        /// </summary>
        /// <param name="item">Item to be deleted.</param>
        /// <exception cref="InvalidOperationException">
        ///   This heap is empty.
        /// </exception>
        public void Delete(IPriorityQueueItem<T> item)
        {
            if (this.IsEmpty())
            {
                throw new InvalidOperationException("This heap is empty.");
            }

            if (item is FibonacciHeapItem<T>)
            {
                FibonacciHeapItem<T> fibHeapItem = (FibonacciHeapItem<T>)item;

                // Cut the edge joining the containing node x to its parent p.
                FibonacciHeapNode<T> x = fibHeapItem.ContainingNode;

                if (x.Parent == null)
                {
                    // x is originally a root - just remove it from the list of roots.
                    if (x == this.minimumNode)
                    {
                        this.DeleteMin();
                        return;
                    }
                    else
                    {
                        x.LeftSibling.RightSibling = x.RightSibling;
                        x.RightSibling.LeftSibling = x.LeftSibling;
                    }
                }
                else
                {
                    /*
                     * As x is not a root, remove it from the list of children of
                     * its parent and decrease its parent's rank.
                     */
                    x.CutEdgeToParent(false, this);

                    /*
                     * Form a new list of roots by concatenating the list of children of
                     * x with the original list of roots.
                     */
                    if (x.SomeChild != null)
                    {
                        this.minimumNode.RightSibling.LeftSibling = x.SomeChild.LeftSibling;
                        x.SomeChild.LeftSibling.RightSibling = this.minimumNode.RightSibling;

                        this.minimumNode.RightSibling = x.SomeChild;
                        x.SomeChild.LeftSibling = this.minimumNode;
                    }
                }

                this.count--;
            }
            else
            {
                throw new ArgumentException("Can work only on Fibonacci Heap items.", "item");
            }
        }

        /// <summary>
        ///   Deletes the item with the minimum key in this heap and returns it.
        /// </summary>
        /// <returns>Item with the minimum key in this heap.</returns>
        /// <exception cref="InvalidOperationException">
        ///   This heap is empty.
        /// </exception>
        public IPriorityQueueItem<T> DeleteMin()
        {
            if (this.IsEmpty())
            {
                throw new InvalidOperationException("This heap is empty.");
            }

            /*
             * Remove the minimum node x from this heap and concatenate the list of
             * children of x with the list of roots of this heap other than x.
             */
            FibonacciHeapNode<T> x = this.minimumNode;
            FibonacciHeapItem<T> minimumItem = x.Item;

            this.count--;

            // Remember some node to start with the linking step later on.
            FibonacciHeapNode<T> someListNode;

            if (x.RightSibling == x)
            {
                if (x.SomeChild == null)
                {
                    // The heap consists of only the root node.
                    this.minimumNode = null;

                    return minimumItem;
                }
                else
                {
                    // Root has no siblings - apply linking step to list of children.
                    someListNode = x.SomeChild;
                }
            }
            else
            {
                if (x.SomeChild == null)
                {
                    // Root has no children - apply linking step to list of siblings.
                    x.LeftSibling.RightSibling = x.RightSibling;
                    x.RightSibling.LeftSibling = x.LeftSibling;

                    someListNode = x.LeftSibling;
                }
                else
                {
                    // Concatenate children and siblings and apply linking step.
                    x.LeftSibling.RightSibling = x.SomeChild;
                    x.SomeChild.LeftSibling.RightSibling = x.RightSibling;
                    x.RightSibling.LeftSibling = x.SomeChild.LeftSibling;
                    x.SomeChild.LeftSibling = x.LeftSibling;

                    someListNode = x.SomeChild;
                }
            }

            // Linking Step.
            /*
             * Create an hashtable indexed by rank, from one up to the maximum
             * possible rank, each entry pointing to a tree root.
             */
            Dictionary<int, FibonacciHeapNode<T>> rankIndexedRoots = new Dictionary<int, FibonacciHeapNode<T>>();

            // Insert the roots one-by-one into the appropriate table positions.
            FibonacciHeapNode<T> nextOldRoot = someListNode;

            do
            {
                FibonacciHeapNode<T> shouldBeInserted = nextOldRoot;
                nextOldRoot = nextOldRoot.RightSibling;

                while (shouldBeInserted != null)
                {
                    /*
                     * If the position is already occupied, perform a linking
                     * step and attempt to insert the root of the new tree into
                     * the next higher position.
                     */
                    if (rankIndexedRoots.ContainsKey(shouldBeInserted.Rank))
                    {
                        FibonacciHeapNode<T> other = rankIndexedRoots[shouldBeInserted.Rank];
                        rankIndexedRoots.Remove(shouldBeInserted.Rank);
                        shouldBeInserted = shouldBeInserted.Link(other);
                    }
                    else
                    {
                        rankIndexedRoots.Add(shouldBeInserted.Rank, shouldBeInserted);
                        shouldBeInserted = null;
                    }
                }
            }
            while (nextOldRoot != someListNode);

            /*
             * Form a list of the remaining roots, in the process finding a root
             * containing an item of minimum key to serve as the minimum node of the
             * modified heap.
             */
            List<FibonacciHeapNode<T>> newRoots = rankIndexedRoots.Values.ToList();

            // Start with the first new root.
            FibonacciHeapNode<T> firstNewRoot = newRoots[0];
            this.minimumNode = firstNewRoot;
            this.minimumNode.Parent = null;

            FibonacciHeapNode<T> currentNewRoot = firstNewRoot;

            for (int i = 1; i < newRoots.Count; i++)
            {
                // Get the next new root.
                FibonacciHeapNode<T> previousNewRoot = currentNewRoot;
                currentNewRoot = newRoots[i];

                // Update pointers.
                previousNewRoot.RightSibling = currentNewRoot;
                currentNewRoot.LeftSibling = previousNewRoot;
                currentNewRoot.Parent = null;

                // Check for new minimum node.
                if (currentNewRoot.Item.Key < this.minimumNode.Item.Key)
                {
                    this.minimumNode = currentNewRoot;
                }
            }

            currentNewRoot.RightSibling = firstNewRoot;
            firstNewRoot.LeftSibling = currentNewRoot;

            return minimumItem;
        }

        /// <summary>
        ///   Returns the item with the minimum key in this heap.
        /// </summary>
        /// <returns>Item with the minimum key in this heap.</returns>
        /// <exception cref="InvalidOperationException">
        ///   This heap is empty.
        /// </exception>
        public IPriorityQueueItem<T> FindMin()
        {
            if (this.IsEmpty())
            {
                throw new InvalidOperationException("This heap is empty.");
            }

            return this.minimumNode.Item;
        }

        /// <summary>
        ///   Inserts the passed item with the specified key into this heap.
        /// </summary>
        /// <param name="item">Item to insert.</param>
        /// <param name="key">Key of the item to insert.</param>
        /// <returns>Container that holds the passed item.</returns>
        public IPriorityQueueItem<T> Insert(T item, double key)
        {
            // Contruct a new container for the passed item.
            FibonacciHeapItem<T> newItem = new FibonacciHeapItem<T>(item, key);

            // Create a new heap consisting of one node containing the passed item.
            FibonacciHeap<T> newHeap = new FibonacciHeap<T>();
            newHeap.minimumNode = new FibonacciHeapNode<T>(newItem);
            newHeap.count = 1;

            // Meld this heap and the new one.
            this.Meld(newHeap);

            return newItem;
        }

        /// <summary>
        ///   Checks whether this heap is empty, or not.
        /// </summary>
        /// <returns>
        ///   <c>true</c>, if this heap is empty, and <c>false</c>
        ///   otherwise.
        /// </returns>
        public bool IsEmpty()
        {
            return this.minimumNode == null;
        }

        /// <summary>
        ///   Takes the union of the passed heap and this one. Assumes that both heaps
        ///   are item-disjoint. This operation destroys the passed heap.
        /// </summary>
        /// <param name="other">Other heap to take the union of.</param>
        public void Meld(FibonacciHeap<T> other)
        {
            // If the other heap is empty, there is nothing to do.
            if (!other.IsEmpty())
            {
                if (this.IsEmpty())
                {
                    // If this heap is empty, return the other heap.
                    this.minimumNode = other.minimumNode;
                }
                else
                {
                    // Combine the root lists of both heaps into a single list.
                    this.minimumNode.RightSibling.LeftSibling = other.minimumNode.LeftSibling;
                    other.minimumNode.LeftSibling.RightSibling = this.minimumNode.RightSibling;

                    this.minimumNode.RightSibling = other.minimumNode;
                    other.minimumNode.LeftSibling = this.minimumNode;

                    // Set the minimum node of the resulting heap.
                    if (this.minimumNode.Item.Key > other.minimumNode.Item.Key)
                    {
                        this.minimumNode = other.minimumNode;
                    }
                }

                this.count += other.count;
            }
        }

        #endregion
    }
}