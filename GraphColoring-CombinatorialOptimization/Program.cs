using System;
using System.Threading;

namespace GraphColoring
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var queen6 = new GraphLoader().Load("queen6.txt", GraphLoader.GraphFormat.Minimal, out _);
                var miles250 = new GraphLoader().Load("miles250.txt", GraphLoader.GraphFormat.Minimal, out _);
                var le450_5a = new GraphLoader().Load("le450_5a.txt", GraphLoader.GraphFormat.Minimal, out _);
                var gc500 = new GraphLoader().Load("gc500.txt", GraphLoader.GraphFormat.Minimal, out _);
                var gc1000 = new GraphLoader().Load("gc_1000.txt", GraphLoader.GraphFormat.Minimal, out _);

                ProcessGraph(le450_5a, "le450_5a", 10000, 200, 4); // 6
                //ProcessGraph(gc500, "gc500", 100, 100, 250);

                //new Thread(() => ProcessGraph(queen6, "queen6", 500000, 300, 3)).Start();
                //new Thread(() => ProcessGraph(miles250, "miles250", 500000, 300, 3)).Start();

                //new Thread(() => ProcessGraph(gc500, "gc500[3]", 500000, 300, 3)).Start();
                //new Thread(() => ProcessGraph(gc500, "gc500[5]", 500000, 300, 5)).Start();

                //new Thread(() => ProcessGraph(gc1000, "gc1000[3]", 500000, 600, 3)).Start();
                //new Thread(() => ProcessGraph(gc1000, "gc1000[5]", 500000, 600, 5)).Start();
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("Error occured while loading a file with a graph.");
                Console.WriteLine(exception);
            }
        }

        private static void ProcessGraph(Graph graph, string graphName, int maxIteration, int rep, int tabuSize, float maxTime = -1)
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
        }
    }
}
