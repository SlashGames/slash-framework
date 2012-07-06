// -----------------------------------------------------------------------
// <copyright file="AdjacencyListWeightedDirectedGraph.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.Collections.Graphs
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An implementation of the abstract datatype weighted directed
    /// graph using an adjacency list to encode the set of edges.
    /// </summary>
    /// <typeparam name="T">Type of the vertices of this graph.</typeparam>
    public class AdjacencyListWeightedDirectedGraph<T> : IWeightedGraph<T>
        where T : IGraphVertex
    {
        #region Constants and Fields

        /// <summary>
        /// Vertices of this graph.
        /// </summary>
        private T[] vertices;

        /// <summary>
        /// Edges between the vertices of this graph.
        /// </summary>
        private List<T>[] edges;

        /// <summary>
        /// Weights of the edges between the vertices of this graph.
        /// </summary>
        private List<int>[] edgeWeights;

        /// <summary>
        /// Number of vertices of this graph.
        /// </summary>
        private int vertexCount;

        /// <summary>
        /// Number of edges between the vertices of this graph.
        /// </summary>
        private int edgeCount;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for this implementation of
        /// directed graphs. Takes an array of vertices
        /// as parameter, which should be the set of vertices
        /// of this graph.
        /// Initially there are no edges.
        /// </summary>
        /// <param name="vertices">Vertices of the new graph.</param>
        public AdjacencyListWeightedDirectedGraph(T[] vertices)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException("vertices", "The passed vertex array must not be null.");
            }

            // Take the given array of vertices and compute its length.
            this.vertices = vertices;
            this.vertexCount = vertices.Length;

            // Construct new arrays to hold the lists for the edges and
            // edge weights between the vertices of this graph.
            this.edges = new List<T>[this.vertexCount];
            this.edgeWeights = new List<int>[this.vertexCount];

            for (int i = 0; i < this.vertexCount; i++)
            {
                // Construct new lists for the edges and
                // edge weights between the vertices of this graph.
                this.edges[i] = new List<T>();
                this.edgeWeights[i] = new List<int>();
            }

            // Initially there are no edges.
            this.edgeCount = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Vertices of this graph.
        /// </summary>
        public T[] Vertices
        {
            get { return this.vertices; }
        }

        /// <summary>
        /// Edges between the vertices of this graph.
        /// </summary>
        public List<T>[] Edges
        {
            get { return this.edges; }
        }

        /// <summary>
        /// Weights of the edges between the vertices of this graph.
        /// </summary>
        public List<int>[] EdgeWeights
        {
            get { return this.edgeWeights; }
        }

        /// <summary>
        /// Number of vertices of this graph.
        /// </summary>
        public int VertexCount
        {
            get { return this.vertexCount; }
        }

        /// <summary>
        /// Number of edges between the vertices of this graph.
        /// </summary>
        public int EdgeCount
        {
            get { return this.edgeCount; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the degree of the given vertex,
        /// in other words the number of adjacent vertices, in O(1).
        /// </summary>
        /// <param name="node">Vertex to get the degree of.</param>
        /// <returns>Degree of the vertex.</returns>
        public int Degree(T node)
        {
            return this.edges[node.Index].Count;
        }

        /// <summary>
        /// Checks if there is an edge between two vertices of this
        /// graph in O(n), where n is the number of adjacent vertices
        /// of the first one.
        /// </summary>
        /// <param name="firstNode">First vertex to check.</param>
        /// <param name="secondNode">Second vertex to check.</param>
        /// <returns>True if there is an edge, and false otherwise.</returns>
        public bool HasEdge(T firstNode, T secondNode)
        {
            return this.edges[firstNode.Index].Contains(secondNode);
        }

        /// <summary>
        /// Gets the weight of the edge between the specified vertices in O(n),
        /// where n is the number of adjacent vertices of the first one.
        /// </summary>
        /// <param name="firstNode">First vertex to check.</param>
        /// <param name="secondNode">Second vertex to check.</param>
        /// <returns>
        /// Edge weight of the edge between the two vertices, if there is one,
        /// and -1 otherwise.
        /// </returns>
        public int GetEdgeWeight(T firstNode, T secondNode)
        {
            // Look at the list of neighbors of the first node and get
            // the list index of the second node there.
            int listIndex = this.edges[firstNode.Index].IndexOf(secondNode);

            // If there is no edge between the specified vertices, return -1.
            if (listIndex < 0)
            {
                return -1;
            }

            // Return the weight at the same index in the list of weights.
            return this.edgeWeights[firstNode.Index][listIndex];
        }

        /// <summary>
        /// Adds two edges between two vertices in this graph in O(1),
        /// weighted with the given value.
        /// </summary>
        /// <param name="firstNode">First vertex to add edges to.</param>
        /// <param name="secondNode">Second vertex to add edges to.</param>
        /// <param name="edgeWeight">Weight of the new edges.</param>
        public void AddEdges(T firstNode, T secondNode, int edgeWeight)
        {
            this.edges[firstNode.Index].Add(secondNode);
            this.edgeWeights[firstNode.Index].Add(edgeWeight);

            this.edges[secondNode.Index].Add(firstNode);
            this.edgeWeights[secondNode.Index].Add(edgeWeight);

            this.edgeCount += 2;
        }

        /// <summary>
        /// Adds one edge between two vertices in this graph in O(1),
        /// weighted with the given value.
        /// </summary>
        /// <param name="firstNode">Source vertex of the edge.</param>
        /// <param name="secondNode">Target vertex of the edge.</param>
        /// <param name="edgeWeight">Weight of the new edge.</param>
        public void AddSingleEdge(T firstNode, T secondNode, int edgeWeight)
        {
            this.edges[firstNode.Index].Add(secondNode);
            this.edgeWeights[firstNode.Index].Add(edgeWeight);

            this.edgeCount++;
        }

        /// <summary>
        /// Returns a list containing the adjacent
        /// vertices of a given vertex in this graph in O(1).
        /// </summary>
        /// <param name="node">Vertex to get the neighbors of.</param>
        /// <returns>List with the neighbors of the given vertex.</returns>
        public List<T> ListOfAdjacentVertices(T node)
        {
            return this.edges[node.Index];
        }

        #endregion
    }
}
