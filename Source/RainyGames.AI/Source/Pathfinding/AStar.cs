// -----------------------------------------------------------------------
// <copyright file="AStar.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.AI.Pathfinding
{
    using System;
    using System.Collections.Generic;
    using RainyGames.Collections.Graphs;
    using RainyGames.Collections.PriorityQueues;

    /// <summary>
    /// Implemenation of the A* algorithm for calculating the most efficient path
    /// between two specified pathfinding nodes.
    /// </summary>
    public class AStar
    {
        /// <summary>
        /// Computes the most efficient path in the specified graph between the
        /// specified pathfinding nodes using the A* algorithm.
        /// </summary>
        /// <param name="graph">Pathfinding graph to look at.</param>
        /// <param name="start">Starting node.</param>
        /// <param name="finish">Finish node.</param>
        /// <returns>
        /// List of pathfinding nodes representing the shortest path, if there is one,
        /// and null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Passed graph or start or finish node is <c>null</c>.
        /// </exception>
        public static List<IAStarNode> FindPath(
            IWeightedGraph<IAStarNode> graph, IAStarNode start, IAStarNode finish)
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
            IPriorityQueue<IAStarNode> openList = new FibonacciHeap<IAStarNode>();
            IPriorityQueueItem<IAStarNode>[] fibHeapItems =
                new FibonacciHeapItem<IAStarNode>[graph.VertexCount];

            // Initialize queue to hold the nodes along the path to the finish.
            Queue<IAStarNode> closedList = new Queue<IAStarNode>();

            // Declare current node to work on in order to calculate the path.
            IAStarNode currentNode;

            // Declare list to hold the neighbors of the current node.
            List<IAStarNode> neighbors;

            // Add starting node to open list.
            fibHeapItems[start.Index] = openList.Insert(start, 0);
            start.Discovered = true;

            // --- A* Pathfinding Algorithm ---
            while ((!algorithmComplete) && (!algorithmAborted))
            {
                // Get the node with the lowest F score in the open list.
                currentNode = openList.DeleteMin().Item;

                // Drop that node from the open list and add it to the closed list.
                closedList.Enqueue(currentNode);
                currentNode.Visited = true;

                // We're done if the target node is added to the closed list.
                if (currentNode == finish)
                {
                    algorithmComplete = true;
                    break;
                }

                // Otherwise, get all adjacent nodes.
                neighbors = graph.ListOfAdjacentVertices(currentNode);

                // Add all nodes that aren't already on the open or closed list to the open list.
                foreach (IAStarNode node in neighbors)
                {
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
                            node.G = node.ParentNode.G + graph.GetEdgeWeight(node, node.ParentNode);

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
                                node.G = node.ParentNode.G + graph.GetEdgeWeight(node, node.ParentNode);
                                node.F = node.G + node.H;

                                openList.DecreaseKeyTo(fibHeapItems[node.Index], node.F);
                            }
                        }
                    }

                    // Ignore nodes in the closed list.
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
                Stack<IAStarNode> s = new Stack<IAStarNode>();
                IAStarNode node = finish;
                while (node != start)
                {
                    s.Push(node);
                    node = node.ParentNode;
                }

                // Push start manually.
                s.Push(node);

                // Generate path in right order.
                List<IAStarNode> path = new List<IAStarNode>();
                while (s.Count > 0)
                {
                    path.Add(s.Pop());
                }

                return path;
            }
            else
            {
                return null;
            }
        }
    }
}
