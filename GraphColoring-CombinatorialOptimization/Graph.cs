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
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Graph - Vertex Count: {Vertices?.Count}");

            foreach (var vertex in Vertices)
                builder.AppendLine(vertex.ToString());

            return builder.ToString();
        }
    }
}
