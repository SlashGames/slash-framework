// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AStar.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.Pathfinding
{
    using System;
    using System.Collections.Generic;

    using Slash.Collections.Graphs;
    using Slash.Collections.PriorityQueues;

    /// <summary>
    ///   Implemenation of the A* algorithm for calculating the most efficient path
    ///   between two specified pathfinding nodes.
    /// </summary>
    public class AStar
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Computes the most efficient path in the specified graph between the
        ///   specified pathfinding nodes using the A* algorithm.
        /// </summary>
        /// <param name="graph">Pathfinding graph to look at.</param>
        /// <param name="start">Starting node.</param>
        /// <param name="finish">Finish node.</param>
        /// <returns>
        ///   List of pathfinding nodes representing the shortest path, if there is one,
        ///   and null otherwise.
        /// </returns>
        /// <typeparam name="T">Type of the pathfinding nodes.</typeparam>
        /// <exception cref="ArgumentNullException">
        ///   Passed graph or start or finish node is <c>null</c>.
        /// </exception>
        public static List<T> FindPath<T>(IWeightedGraph<T> graph, T start, T finish) where T : IAStarNode
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph", "The passed pathfinding graph mustn't be null.");
            }

            if (start == null)
            {
                throw new ArgumentNullException("start", "The passed start node mustn't be null.");
            }

            if (finish == null)
            {
                throw new ArgumentNullException("finish", "The passed finish mustn't be null.");
            }

            // --- Initialization ---
            // Initialize variables to check for the algorithm to terminate.
            bool algorithmComplete = false;
            bool algorithmAborted = false;

            // Initialize list to choose the next node of the path from.
            IPriorityQueue<T> openList = new FibonacciHeap<T>();
            IPriorityQueueItem<T>[] fibHeapItems = new FibonacciHeapItem<T>[graph.VertexCount];
            HashSet<T> dirtyNodes = new HashSet<T>();

            // Initialize queue to hold the nodes along the path to the finish.
            Queue<T> closedList = new Queue<T>();

            // Declare current node to work on in order to calculate the path.

            // Declare list to hold the neighbors of the current node.

            // Add starting node to open list.
            fibHeapItems[start.Index] = openList.Insert(start, 0);
            dirtyNodes.Add(start);
            start.Discovered = true;

            // --- A* Pathfinding Algorithm ---
            while ((!algorithmComplete) && (!algorithmAborted))
            {
                // Get the node with the lowest F score in the open list.
                T currentNode = openList.DeleteMin().Item;

                // Drop that node from the open list and add it to the closed list.
                closedList.Enqueue(currentNode);
                currentNode.Visited = true;

                // We're done if the target node is added to the closed list.
                if (currentNode.Equals(finish))
                {
                    algorithmComplete = true;
                    break;
                }

                // Otherwise, get all adjacent nodes.
                List<T> neighbors = graph.ListOfAdjacentVertices(currentNode);

                // Add all nodes that aren't already on the open or closed list to the open list.
                foreach (T node in neighbors)
                {
                    // Ignore nodes in the closed list.
                    if (!node.Visited)
                    {
                        if (!node.Discovered)
                        {
                            // The parent node is the previous node on the path to the finish.
                            node.ParentNode = currentNode;

                            // The G score of the node is calculated by adding the G score
                            // of the parent node and the movement cost of the path between
                            // the node and the current node.
                            // In other words: The G score of the node is the total cost of the
                            // path between the starting node and this one.
                            node.G = node.ParentNode.G + graph.GetEdgeWeight(node, (T)node.ParentNode);

                            // The H score of the node is calculated by heuristically
                            // estimating the movement cost from the node to the finish.
                            // In other words: The H score of the node is the total remaining
                            // cost of the path between this node and the finish.
                            node.H = node.EstimateHeuristicMovementCost(finish);

                            // The F score of the node is calculated by adding the G and H scores.
                            // In other words: The F score is an indicator that tells whether this
                            // node should be crossed on the path to the finish, or not.
                            node.F = node.G + node.H;

                            // Add to open list.
                            fibHeapItems[node.Index] = openList.Insert(node, node.F);
                            dirtyNodes.Add(node);
                            node.Discovered = true;
                        }
                        else
                        {
                            // Node is already in open list!
                            // Check if the new path to this node is a better one.
                            if (currentNode.G + graph.GetEdgeWeight(node, currentNode) < node.G)
                            {
                                // G cost of new path is lower!
                                // Change parent node to current node.
                                node.ParentNode = currentNode;

                                // Recalculate F and G costs.
                                node.G = node.ParentNode.G + graph.GetEdgeWeight(node, (T)node.ParentNode);
                                node.F = node.G + node.H;

                                openList.DecreaseKeyTo(fibHeapItems[node.Index], node.F);
                            }
                        }
                    }
                }

                // We've failed to find a path if the open list is empty.
                if (openList.IsEmpty())
                {
                    algorithmAborted = true;
                }
            }

            // Return the path to the finish, if there is one.
            if (algorithmComplete)
            {
                // Generate path through parent pointers using a stack.
                Stack<T> s = new Stack<T>();
                T node = finish;
                while (!node.Equals(start))
                {
                    s.Push(node);
                    node = (T)node.ParentNode;
                }

                // Generate path in right order.
                List<T> path = new List<T>();
                while (s.Count > 0)
                {
                    path.Add(s.Pop());
                }

                // Cleanup pathing.
                foreach (T dirtyNode in dirtyNodes)
                {
                    dirtyNode.Reset();
                }

                return path;
            }
            else
            {
                // Cleanup pathing.
                foreach (T dirtyNode in dirtyNodes)
                {
                    dirtyNode.Reset();
                }

                return null;
            }
        }

        #endregion
    }
}