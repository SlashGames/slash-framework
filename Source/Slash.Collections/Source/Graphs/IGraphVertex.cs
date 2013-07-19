// -----------------------------------------------------------------------
// <copyright file="IGraphVertex.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Slash.Collections.Graphs
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
