using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GraphColoring
{
    public class GraphLoader
    {
        public enum GraphFormat
        {
            /// <summary>
            /// First line is number of vertices.
            /// All other lines are in provided format: "{first endpoint} {second endpoint}"
            /// </summary>
            Minimal
        }

        public Graph Load(string filepath, GraphFormat format, out int? vertexCount)
        {
            vertexCount = null;

            if (!File.Exists(filepath))
                throw new FileNotFoundException();

            switch (format)
            {
                case GraphFormat.Minimal:
                    return LoadMinimalFormat(File.ReadLines(filepath), out vertexCount);
                default:
                    break;
            }

            return null;
        }

        private Graph LoadMinimalFormat(IEnumerable<string> lines, out int? vertexCount)
        {
            vertexCount = null;

            var vertices = new List<GraphVertex>();
            var graph = new Graph(vertices);

            var helperDictionary = new Dictionary<string, GraphVertex>();

            int lineCounter = 0;
            foreach (var line in lines)
            {
                if (lineCounter == 0)
                {
                    if (int.TryParse(line, out var parsed))
                    {
                        vertexCount = parsed;

                        for (int i = 1; i <= vertexCount; i++)
                        {
                            var vert = new GraphVertex($"{i:00000}");
                            vert.Graph = graph;
                            graph.Vertices.Add(vert);
                            helperDictionary[vert.Identifier] = vert;
                        }
                    }

                    lineCounter++;
                    continue;
                }

                var words = line.Split(" ");
                if (words.Length >= 2 && int.TryParse(words[0], out var firstEndpoint) && int.TryParse(words[1], out var secondEndpoint))
                {
                    var firstId = $"{firstEndpoint:00000}";
                    var secondId = $"{secondEndpoint:00000}";

                    helperDictionary.TryGetValue(firstId, out var firstEndpointVertex);
                    helperDictionary.TryGetValue(secondId, out var secondEndpointVertex);
                    //var firstEndpointVertex = graph.Vertices.Find(v => v.Identifier == firstId);
                    //var secondEndpointVertex = graph.Vertices.Find(v => v.Identifier == secondId);

                    if (firstEndpointVertex == null)
                    {
                        firstEndpointVertex = new GraphVertex(firstId);
                        firstEndpointVertex.Graph = graph;
                        graph.Vertices.Add(firstEndpointVertex);
                        helperDictionary[firstId] = firstEndpointVertex;
                    }

                    if (secondEndpointVertex == null)
                    {
                        secondEndpointVertex = new GraphVertex(secondId);
                        secondEndpointVertex.Graph = graph;
                        graph.Vertices.Add(secondEndpointVertex);
                        helperDictionary[secondId] = secondEndpointVertex;
                    }

                    firstEndpointVertex.Connect(secondEndpointVertex);
                }

                lineCounter++;
            }

            graph.Vertices = graph.Vertices.OrderBy(v => v.Identifier).ToList();

            return graph;
        }
    }
}
