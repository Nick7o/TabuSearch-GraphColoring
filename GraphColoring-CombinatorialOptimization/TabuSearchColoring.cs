using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            int iteration = 0;
            int numberOfColors = graph.Vertices.Count;
            var tabu = new List<(GraphVertex Vertex, int Color)>(TabuSize);
            var aspiration = new Dictionary<int, int>();
            var lastValidGraph = graph.Clone();

            // Searching for the best start using randomized greedy search
            List<int> colors = new List<int>();
            var greedyColoring = new GreedyColoring();
            for (int i = 0; i < 3; i++)
            {
                graph.ClearColors();
                var numberOfColorsGR = greedyColoring.Color(graph);
                if (colors.Count == 0 || numberOfColorsGR <= colors.Max())
                {
                    Console.WriteLine($"Found greedy solution - colors: {numberOfColorsGR}");
                    colors.Clear();
                    foreach (var vert in graph.Vertices)
                        colors.Add(vert.ColorId.Value);
                }
                graph.RandomizeOrder();
            }

            for (int i = 0; i < graph.Vertices.Count; i++)
                graph.Vertices[i].ColorId = colors[i];

            numberOfColors = colors.Max() + 1;
            Console.WriteLine("NB OF COLORS: "+ numberOfColors);

            ClampColors(graph, numberOfColors - 1);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while (iteration < MaxIterations)
            {
                var conflictingVertices = GetConflictingVertices(graph);//.ToList();
                var conflictsInGraph = conflictingVertices.Count;

                GraphVertex vert = null;
                int newColor = 0;
                for (int r = 0; r < Rep; r++)
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

                    vert.ColorId = oldColor;
                }

                tabu.Add((vert, vert.ColorId.Value));
                if (tabu.Count > TabuSize)
                    tabu.RemoveAt(0);

                vert.ColorId = newColor;

                if (CountConflicts(graph) == 0)
                {
                    lastValidGraph = graph.Clone();
                    numberOfColors--;
                    iteration = 0;
                    ClampColors(graph, numberOfColors - 1);

                    //graph.ClearColors();
                    //greedyColoring.MaxColorId = numberOfColors;
                    //greedyColoring.Color(graph);

                    Console.WriteLine($"No conflicts, new number of colors: {numberOfColors + 1}");
                }
                else
                    iteration++;

                if (iteration % 100 == 0)
                {
                    Console.WriteLine($"{System.Threading.Thread.CurrentThread.ManagedThreadId} - ELAPSED TIME: {sw.ElapsedMilliseconds} ms");
                }
            }
            sw.Stop();
            Console.WriteLine($"TOTAL TIME: {sw.ElapsedMilliseconds} ms");

            return lastValidGraph.GetColorCount();

            bool IsInTabu(GraphVertex vertex, int color)
            {
                return tabu.Contains((vertex, color));//.Exists(t => t.Vertex == vertex && t.Color == color);
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