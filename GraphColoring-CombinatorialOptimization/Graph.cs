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

        private Dictionary<string, GraphVertex> Cache { get; set; } = new Dictionary<string, GraphVertex>();

        public Graph() { }

        public Graph(List<GraphVertex> vertices)
        {
            Vertices = vertices;
            Cache = new Dictionary<string, GraphVertex>(vertices.Count);
            foreach (var v in vertices)
                Cache[v.Identifier] = v;

            ApplyParent();
        }

        public void ApplyParent()
        {
            foreach (var vertex in Vertices)
            {
                vertex.Graph = this;
            }
        }

        public GraphVertex Get(string identifier)
        {
            if (!Cache.TryGetValue(identifier, out var value) || value.Graph != this)
            {
                if (value != null && value.Graph != this)
                    Cache.Remove(identifier);

                value = Vertices.Find(v => v.Identifier == identifier);
                if (value != null)
                    Cache[identifier] = value;
            }

            return value;
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

        public void ClearColors()
        {
            foreach (var vertex in Vertices)
            {
                vertex.ColorId = null;
            }
        }

        public void RandomizeOrder()
        {
            Random rnd = new Random();

            int n = Vertices.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = Vertices[k];
                Vertices[k] = Vertices[n];
                Vertices[n] = value;
            }
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
