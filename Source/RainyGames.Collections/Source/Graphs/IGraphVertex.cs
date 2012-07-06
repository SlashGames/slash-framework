// -----------------------------------------------------------------------
// <copyright file="IGraphVertex.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.Collections.Graphs
{
    /// <summary>
    /// Vertex of any graph with a unique index.
    /// </summary>
    public interface IGraphVertex
    {
        /// <summary>
        /// The unique array index of this vertex in the graph.
        /// </summary>
        int Index { get; }
    }
}
