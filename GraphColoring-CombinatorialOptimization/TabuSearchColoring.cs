using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    class TabuSearchColoring : IGraphColoring
    {
        public static int MaxIterations = 100;

        public int StartColors { get; private set; }

        private Random _random;

        public TabuSearchColoring(int startColors)
        {
            _random = new Random();
            StartColors = startColors;
        }

        public int Color(Graph graph)
        {
            int iteration = 0;
            int numberOfColors = StartColors - 1;
            var tabu = new List<(GraphVertex Vertex, int Color)>();
            var aspiration = new Dictionary<int, int>();
            Graph lastValidGraph = graph.Clone();

            ClampColors(graph, numberOfColors - 1);

            while (iteration < MaxIterations)
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                var conflictingVertices = GetConflictingVertices(graph);//.ToList();
                var conflictsInGraph = conflictingVertices.Count;

                GraphVertex vert = null;
                int newColor = 0;
                for (int r = 0; r < 200; r++)
                {
                    //vert = cn[_random.Next(0, cn.Count)];
                    vert = conflictingVertices.ElementAt(_random.Next(0, conflictingVertices.Count));

                    newColor = _random.Next(0, numberOfColors);
                    for (int i = 0; i < 3; i++)
                    {
                        if (vert.ColorId.Value == newColor)
                        {
                            newColor = _random.Next(0, numberOfColors);
                            break;
                        }
                    }

                    var oldColor = vert.ColorId.Value;
                    vert.ColorId = newColor;

                    var newConflicts = GetConflictingVertices(graph);

                    if (newConflicts.Count < conflictsInGraph)
                    {
                        int aspirationValue = conflictsInGraph - 1;
                        if (!aspiration.TryGetValue(conflictsInGraph, out aspirationValue))
                            aspiration.Add(conflictsInGraph, conflictsInGraph - 1);

                        if (newConflicts.Count <= aspirationValue)
                        {
                            aspiration[conflictsInGraph] = newConflicts.Count - 1;

                            if (IsInTabu(vert, newColor))
                                tabu.Remove((vert, newColor));
                        }
                        else if (IsInTabu(vert, newColor))
                        {
                            vert.ColorId = oldColor;
                            continue;
                        }

                        break;
                    }

                    vert.ColorId = oldColor;
                }

                tabu.Add((vert, vert.ColorId.Value));
                if (tabu.Count > 3)
                    tabu.RemoveAt(0);

                vert.ColorId = newColor;

                if (CountConflicts(graph) == 0)
                {
                    lastValidGraph = graph.Clone();
                    numberOfColors--;
                    iteration = 0;
                    ClampColors(graph, numberOfColors - 1);
                    Console.WriteLine($"No conflicts, new number of colors: {numberOfColors + 1}");
                }
                else
                    iteration++;


                sw.Stop();
                Console.WriteLine($"ITERATION TIME: {sw.ElapsedMilliseconds}");
            }

            return lastValidGraph.GetColorCount();

            bool IsInTabu(GraphVertex vertex, int color)
            {
                return tabu.Exists(t => t.Vertex == vertex && t.Color == color);
            }
        }

        private int CountConflicts(Graph graph)
        {
            /*int conflicts = 0;
            foreach (var vertex in graph.Vertices)
                conflicts += CountConflicts(vertex);

            return conflicts;*/
            return GetConflictingVertices(graph).Count / 2;
        }

        private void GetConflictingNeighbors(GraphVertex vertex, HashSet<GraphVertex> collection)
        {
            foreach (var neighbor in vertex.Neighbors)
            {
                if (vertex.ColorId.Value == neighbor.ColorId.Value)
                {
                    collection.Add(vertex);
                    collection.Add(neighbor);
                }
            }
        }

        private HashSet<GraphVertex> GetConflictingVertices(Graph graph)
        {
            var result = new HashSet<GraphVertex>();
            foreach (var vertex in graph.Vertices)
            {
                GetConflictingNeighbors(vertex, result);
            }

            return result;
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