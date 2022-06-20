using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphColoring
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Loading graphs
                var gc1000 = new GraphLoader().Load("gc_1000.txt", GraphLoader.GraphFormat.Minimal, out _);
                var gc500 = new GraphLoader().Load("gc500.txt", GraphLoader.GraphFormat.Minimal, out _);
                var le450_5a = new GraphLoader().Load("le450_5a.txt", GraphLoader.GraphFormat.Minimal, out _);
                var queen6 = new GraphLoader().Load("queen6.txt", GraphLoader.GraphFormat.Minimal, out _);
                var miles250 = new GraphLoader().Load("miles250.txt", GraphLoader.GraphFormat.Minimal, out _);

                //ProcessGraph(le450_5a, "le450_5a", 500000, 50, 7, 300);

                //new Thread(() => ProcessGraph(queen6, "queen6", 5000, 100, 7, 200)).Start();
                //new Thread(() => ProcessGraph(gc500, "gc500-420", 500000, 350, 7, 320)).Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine("### Error occured: ###");
                Console.WriteLine(exception);
            }
        }

        private static (int greedyColors, int tabuColors) ProcessRandomGraph(int samples, int vertices, float maxSaturation, double spawnChance)
        {
            int greedyColors = 0, tabuColors = 0;

            for (int i = 0; i < samples; i++)
            {
                var graph = new GraphGenerator().Generate(vertices, (int)(maxSaturation * vertices), spawnChance);
                graph.Name = " iter: " + i;
                var r = ProcessGraph(graph, graph.Name, 1000, 100, 7);
                greedyColors += r.greedyResult;
                tabuColors += r.tabuResult;
            }

            return (greedyColors / samples, tabuColors / samples);
        }

        private static (int greedyResult, int tabuResult) ProcessGraph(Graph graph, string graphName, int maxIteration, int rep, int tabuSize, float maxTime = -1)
        {
            Console.WriteLine($" === GRAPH: {graphName} - v: {graph.Vertices.Count} ===");

            Graph tabuGraph, greedyGraph;

            lock (graph)
            {
                tabuGraph = graph.Clone();
                greedyGraph = graph.Clone();
            }

            tabuGraph.Name = graphName;
            greedyGraph.Name = graphName;

            var greedyColoring = new GreedyColoring();
            var greedyResult = greedyColoring.Color(greedyGraph);

            var tabuSearchColoring = new TabuSearchColoring(maxIteration, rep, tabuSize, maxTime);
            int tabuSearchResult = tabuSearchColoring.Color(tabuGraph);

            Console.WriteLine($"!!! {graphName} - GREEDY NUMBER OF COLORS: {greedyResult}");
            Console.WriteLine($"!!! {graphName} - TABU NUMBER OF COLORS: {tabuSearchResult}");

            return (greedyResult, tabuSearchResult);
        }
    }
}
