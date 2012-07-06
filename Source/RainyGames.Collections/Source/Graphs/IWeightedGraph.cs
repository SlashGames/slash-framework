// -----------------------------------------------------------------------
// <copyright file="IWeightedGraph.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.Collections.Graphs
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface of the abstract datatype weighted graph.
    /// </summary>
    /// <typeparam name="T">Type of the vertices of this graph.</typeparam>
    public interface IWeightedGraph<T>
        where T : IGraphVertex
    {
        /// <summary>
        /// Number of vertices of this graph.
        /// </summary>
        int VertexCount { get; }

        /// <summary>
        /// Number of edges between the vertices of this graph.
        /// </summary>
        int EdgeCount { get; }

        /// <summary>
        /// Returns the number of adjacent vertices of the given vertex.
        /// </summary>
        /// <param name="node">Vertex to get the degree of.</param>
        /// <returns>Degree of the vertex.</returns>
        int Degree(T node);

        /// <summary>
        /// Checks if there is an edge between two vertices of this graph.
        /// </summary>
        /// <param name="firstNode">First vertex to check.</param>
        /// <param name="secondNode">Second vertex to check.</param>
        /// <returns>True if there is an edge, and false otherwise.</returns>
        bool HasEdge(T firstNode, T secondNode);

        /// <summary>
        /// Gets the weight of the edge between the specified vertices.
        /// </summary>
        /// <param name="firstNode">First vertex to check.</param>
        /// <param name="secondNode">Second vertex to check.</param>
        /// <returns>
        /// Edge weight of the edge between the two vertices, if there is one,
        /// and -1 otherwise.
        /// </returns>
        int GetEdgeWeight(T firstNode, T secondNode);

        /// <summary>
        /// Returns a list containing the adjacent vertices of a given vertex in this graph.
        /// </summary>
        /// <param name="node">Vertex to get the neighbors of.</param>
        /// <returns>List with the neighbors of the given vertex.</returns>
        List<T> ListOfAdjacentVertices(T node);
    }
}
