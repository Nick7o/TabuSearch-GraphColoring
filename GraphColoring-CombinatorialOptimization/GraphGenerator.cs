using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    public class GraphGenerator
    {
        private Random _random;

        public GraphGenerator() => _random = new Random();

        /// <summary>
        /// Creates a instance of a connected graph.
        /// </summary>
        /// <param name="vertexCount">Total number of vertices of the final graph.</param>
        /// <param name="maxAdditionalEdges">Max number of additional edges (not including ones mandatory for simple guarantee of a connected graph)</param>
        /// <param name="additionalEdgeSpawnChance">Chance between 0.0 and 1.0 to create an additional edges.</param>
        /// <returns></returns>
        public Graph Generate(int vertexCount, int maxAdditionalEdges, double additionalEdgeSpawnChance)
        {
            if (vertexCount <= 0)
                return new Graph();

            additionalEdgeSpawnChance = Math.Clamp(additionalEdgeSpawnChance, 0, 1);

            var vertices = new List<GraphVertex>(vertexCount);
            var graph = new Graph(vertices);

            // Creating empty vertices
            for (int i = 0; i < vertexCount; i++)
                vertices.Add(new GraphVertex()
                {
                    Graph = graph,
                    Identifier = $"V{i}"
                });

            // Simple connecting of vertices to guarantee a connected graph
            for (int i = 0; i < vertexCount; i++)
            {
                var nextVertexIndex = (i + 1) % vertexCount;
                vertices[i].Connect(vertices[nextVertexIndex]);
            }

            // Filling graph with more edges
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < maxAdditionalEdges; j++)
                {
                    if (vertices[i].Neighbors.Count() > maxAdditionalEdges || !Chance(additionalEdgeSpawnChance))
                        continue;

                    var additionalVertexIndex = _random.Next(0, vertexCount);
                    if (additionalVertexIndex == i || vertices[additionalVertexIndex].Neighbors.Count() > maxAdditionalEdges)
                        continue;

                    vertices[i].Connect(vertices[additionalVertexIndex]);
                }
            }

            return graph;
        }

        /// <summary>
        /// Performs a random number pick and returns if it falls in the range of probability.
        /// </summary>
        /// <param name="probability">A number between 0.0 and 1.0.</param>
        private bool Chance(double probability)
        {
            if (probability >= 1.0)
                return true;
            else if (probability <= 0.0)
                return false;

            return _random.NextDouble() <= probability;
        }
    }
}
