using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    class TabuSearchColoring : IGraphColoring
    {
        public static int MaxIterations = 10;

        public int StartColors { get; private set; }

        public TabuSearchColoring(int startColors)
        {
            StartColors = startColors;
        }

        public int Color(Graph graph)
        {
            int iteration = 0;
            int numberOfColors = StartColors - 1;
            var tabu = new List<(GraphVertex Vertex, int Color)>();

            //Console.WriteLine(graph.ToString(false));
            Graph lastValidGraph = graph.Clone();

            ClampColors(graph, numberOfColors - 1);
            //Console.WriteLine(graph.ToString(false));

            while (CountConflicts(graph) > 0 && iteration < MaxIterations)
            {
                var conflictsInGraph = CountConflicts(graph);

                foreach (var vertex in graph.Vertices)
                {
                    var numberOfConflicts = CountConflicts(vertex);

                    for (int i = 0; i < numberOfColors; i++)
                    {
                        var conflictsNewColor = CountConflicts(vertex, i);

                        if (IsInTabu(vertex, i))
                            continue;
                        else if (CountConflicts(vertex, i) < numberOfConflicts)
                        {
                            vertex.ColorId = i;
                            break;
                        }
                    }

                    tabu.Add((vertex, vertex.ColorId.Value));
                    if (tabu.Count > 150)
                        tabu.RemoveAt(0);
                    //tabu.Remove(tabu.Find(t => t.Vertex == vertex));

                    conflictsInGraph = Math.Min(conflictsInGraph, CountConflicts(graph));
                }

                if (conflictsInGraph == 0)
                {
                    lastValidGraph = graph.Clone();
                    numberOfColors--;
                    iteration = 0;
                    ClampColors(graph, numberOfColors);
                    Console.WriteLine($"No conflicts, new number of colors: {numberOfColors + 1}");
                }

                iteration++;
            }

            return lastValidGraph.GetColorCount();

            bool IsInTabu(GraphVertex vertex, int color)
            {
                return tabu.Exists(t => t.Vertex == vertex && t.Color == color);
            }
        }

        private List<int> GetNeighborColors(GraphVertex vertex)
        {
            var neighborColors = new List<int>();

            foreach (var neighbor in vertex.Neighbors)
            {
                if (neighbor.ColorId.HasValue)
                    neighborColors.Add(neighbor.ColorId.Value);
            }

            return neighborColors;
        }

        private int CountConflicts(GraphVertex vertex)
        {
            return vertex.Neighbors.Count(n => n.ColorId == vertex.ColorId);
        }

        private int CountConflicts(GraphVertex vertex, int colorId)
        {
            return vertex.Neighbors.Count(n => n.ColorId == colorId);
        }

        private int CountConflicts(Graph graph)
        {
            int conflicts = 0;
            foreach (var vertex in graph.Vertices)
                conflicts += CountConflicts(vertex);

            return conflicts;
        }

        private void ClampColors(Graph graph, int maxColor)
        {
            foreach (var vertex in graph.Vertices)
            {
                vertex.ColorId = Math.Min(vertex.ColorId.Value, maxColor);
            }
        }
    }
}
