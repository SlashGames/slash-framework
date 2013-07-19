// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AStarNode.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.Pathfinding
{
    /// <summary>
    ///   Default implementation of an A* node.
    /// </summary>
    public abstract class AStarNode : IAStarNode
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new A* node with the specified unique index.
        /// </summary>
        /// <param name="index">Unique index of the new vertex in the pathfinding graph.</param>
        public AStarNode(int index)
        {
            this.Index = index;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Whether this node has already been discovered and added to the
        ///   open list, or not.
        /// </summary>
        public bool Discovered { get; set; }

        /// <summary>
        ///   F score of this node, computed by adding G and H.
        /// </summary>
        public int F { get; set; }

        /// <summary>
        ///   G score of this node, telling the movement cost needed
        ///   for travelling from the starting node to this one.
        /// </summary>
        public int G { get; set; }

        /// <summary>
        ///   H score of this node, telling the estimated movement cost
        ///   needed for travelling from this node to the finish.
        /// </summary>
        public int H { get; set; }

        /// <summary>
        ///   The unique index of this vertex in the pathfinding graph.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///   Previous node on the path to the finish.
        /// </summary>
        public IAStarNode ParentNode { get; set; }

        /// <summary>
        ///   Whether this node has already been visited and moved to the
        ///   closed list, or not.
        /// </summary>
        public bool Visited { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the estimated, heuristic movement cost needed to get
        ///   from this node to the specified other one.
        /// </summary>
        /// <param name="target">Target node.</param>
        /// <returns>Estimated movement cost.</returns>
        public abstract int EstimateHeuristicMovementCost(IAStarNode target);

        /// <summary>
        ///   Resets this node, clearing its parent and visited status.
        /// </summary>
        public void Reset()
        {
            this.Discovered = false;
            this.F = 0;
            this.G = 0;
            this.H = 0;
            this.ParentNode = null;
            this.Visited = false;
        }

        #endregion
    }
}