using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    public class Graph
    {
        public List<GraphVertex> Vertices { get; set; } = new List<GraphVertex>();

        public Graph() { }

        public Graph(List<GraphVertex> vertices)
        {
            Vertices = vertices;

            ApplyParent();
        }

        public void ApplyParent()
        {
            foreach (var vertex in Vertices)
            {
                vertex.Graph = this;
            }
        }

        public bool Contains(string identifier)
        {
            return Vertices.Exists(v => v.Identifier == identifier);
        }

        public int GetColorCount()
        {
            int colorCount = 0;
            foreach (var v in Vertices)
            {
                if (v.ColorId != null)
                    colorCount = Math.Max(colorCount, v.ColorId.Value);
            }

            return colorCount + 1;
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool printVertexNeighbors)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Graph - Vertex Count: {Vertices?.Count}");

            foreach (var vertex in Vertices)
                builder.AppendLine(vertex.ToString(printVertexNeighbors));

            return builder.ToString();
        }

        public Graph Clone()
        {
            var clonedVertices = new List<GraphVertex>(Vertices.Count);
            var newGraph = new Graph(clonedVertices);

            for (int i = 0; i < Vertices.Count; i++)
            {
                var clonedVertex = Vertices[i].Clone();
                clonedVertex.Graph = newGraph;
                clonedVertices.Add(clonedVertex);
            }

            return newGraph;
        }
    }
}
