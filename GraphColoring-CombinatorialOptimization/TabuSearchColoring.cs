using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphColoring
{
    class TabuSearchColoring : IGraphColoring
    {
        public int MaxIterations { get; set; } = 1000;
        public int Rep { get; set; }
        public int TabuSize { get; set; }
        public float MaxTime { get; set; }

        private Random _random;

        public TabuSearchColoring(int maxIterations, int rep, int tabuSize, float maxTime = -1)
        {
            _random = new Random();
            SetParams(maxIterations, rep, tabuSize, maxTime);
        }

        public void SetParams(int maxIterations, int rep, int tabuSize, float maxTime = -1)
        {
            MaxIterations = maxIterations;
            Rep = rep;
            TabuSize = tabuSize;
            MaxTime = maxTime;
        }

        public int Color(Graph graph)
        {
            // Initializing
            var tabu = new List<(GraphVertex Vertex, int Color)>(TabuSize);
            var aspiration = new Dictionary<int, int>();

            var greedyColoring = new GreedyColoring();
            int numberOfColors = greedyColoring.Color(graph) - 1;
            var lastValidGraph = graph.Clone();

            ClampColors(graph, numberOfColors - 1);

            int iteration = 0;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while (iteration < MaxIterations)
            {
                if (MaxTime > 0 && sw.ElapsedMilliseconds > MaxTime * 1000)
                    break;

                var conflictingVertices = GetConflictingVertices(graph);//.ToList();
                var conflictsInGraph = conflictingVertices.Count;

                GraphVertex vert = null;
                int newColor;

                for (int r = 0; r < Rep; r++)
                {
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
                        int aspirationValue;
                        if (!aspiration.TryGetValue(conflictsInGraph, out aspirationValue))
                        {
                            aspirationValue = conflictsInGraph - 1;
                            aspiration.Add(conflictsInGraph, conflictsInGraph - 1);
                        }

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

                    if (r < Rep - 1)
                        vert.ColorId = oldColor;
                }

                tabu.Add((vert, vert.ColorId.Value));
                if (tabu.Count > TabuSize)
                    tabu.RemoveAt(0);

                if (CountConflicts(graph) == 0)
                {
                    lastValidGraph = graph.Clone();
                    numberOfColors--;
                    iteration = 0;
                    ClampColors(graph, numberOfColors - 1);

                    Console.WriteLine($"{graph.Name} - No conflicts, new number of colors: {numberOfColors + 1}");
                }
                else
                    iteration++;

                if (iteration % 250 == 0)
                    Console.WriteLine($"{graph.Name} - ELAPSED TIME: {sw.ElapsedMilliseconds} ms");
            }
            sw.Stop();
            Console.WriteLine($"TOTAL TIME: {sw.ElapsedMilliseconds} ms");

            return lastValidGraph.GetColorCount();

            bool IsInTabu(GraphVertex vertex, int color) => tabu.Contains((vertex, color));
        }

        private int CountConflicts(Graph graph) => GetConflictingVertices(graph).Count / 2;

        private void GetConflictingNeighbors(GraphVertex vertex, HashSet<GraphVertex> collection)
        {
            foreach (var neighborId in vertex.NeighborIdentifiers)
            {
                var neighbor = vertex.Graph.Get(neighborId);
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