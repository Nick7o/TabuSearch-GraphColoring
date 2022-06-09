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
                var le450_5a = new GraphLoader().Load("le450_5a.txt", GraphLoader.GraphFormat.Minimal, out _);
                var gc500 = new GraphLoader().Load("gc500.txt", GraphLoader.GraphFormat.Minimal, out _);

                //ProcessGraph(le450_5a, "le450_5a", 10000, 200, 4); // 6
                ProcessGraph(gc500, "gc500", 100, 100, 250);

                //new Thread(() => ProcessGraph(gc500, "gc500", 5000, 200, 4)).Start();
                //new Thread(() => ProcessGraph(gc500, "gc500", 5000, 200, 16)).Start();
                //new Thread(() => ProcessGraph(gc500, "gc500", 5000, 200, 64)).Start();
                //new Thread(() => ProcessGraph(gc500, "gc500 fast", 1000, 50, 96)).Start();
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("Error occured while loading a file with a graph.");
                Console.WriteLine(exception);
            }
        }

        private static void ProcessGraph(Graph graph, string graphName, int maxIteration, int rep, int tabuSize, float maxTime = -1)
        {
            Console.WriteLine($" === GRAPH: {graphName} ===");

            Graph tabuGraph, greedyGraph;

            lock (graph)
            {
                tabuGraph = graph.Clone();
                greedyGraph = graph.Clone();
            }

            var greedyColoring = new GreedyColoring();
            var greedyResult = greedyColoring.Color(greedyGraph);

            var tabuSearchColoring = new TabuSearchColoring(maxIteration, rep, tabuSize, maxTime);
            int tabuSearchResult = tabuSearchColoring.Color(tabuGraph);

            Console.WriteLine($"GREEDY NUMBER OF COLORS: {greedyResult}");
            Console.WriteLine($"TABU NUMBER OF COLORS: {tabuSearchResult}");
            Console.WriteLine($" =========================== ");
        }
    }
}
