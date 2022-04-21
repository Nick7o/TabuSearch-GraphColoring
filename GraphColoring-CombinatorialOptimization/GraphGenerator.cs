using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    public class GraphGenerator
    {
        public Graph Generate(int vertexCount, float fillFactor)
        {
            if (vertexCount <= 0)
                return new Graph();

            fillFactor = Math.Clamp(fillFactor, 0f, 1f);

            var vertices = new List<GraphVertex>(vertexCount);

            // Creating empty vertices
            for (int i = 0; i < vertexCount; i++)
                vertices.Add(new GraphVertex());

            for (int i = 0; i < vertexCount; i++)
            {
                // Simple connecting of vertices to guarantee a connected graph
                var nextVertexIndex = i % (vertexCount - 1);
                vertices[i].Connect(vertices[nextVertexIndex]);

                // Filling graph with more edges
                var randomGenerator = new Random();
                var additionalEdges = fillFactor > 0 ? (int)(vertexCount * fillFactor) : 0;
                for (int j = 0; j < additionalEdges; j++)
                {
                    var additionalVertexIndex = randomGenerator.Next(0, vertexCount);
                    if (additionalVertexIndex == i)
                        continue;

                    vertices[i].Connect(vertices[additionalVertexIndex]);
                }
            }

            return new Graph(vertices);
        }
    }
}
