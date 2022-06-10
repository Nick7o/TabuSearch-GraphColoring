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
                var graph = new GraphGenerator().Generate(100, 50, 0.5);
                Console.WriteLine(graph.ToString(false));

                var queen6 = new GraphLoader().Load("queen6.txt", GraphLoader.GraphFormat.Minimal, out _);
                var miles250 = new GraphLoader().Load("miles250.txt", GraphLoader.GraphFormat.Minimal, out _);
                var le450_5a = new GraphLoader().Load("le450_5a.txt", GraphLoader.GraphFormat.Minimal, out _);
                var gc500 = new GraphLoader().Load("gc500.txt", GraphLoader.GraphFormat.Minimal, out _);
                var gc1000 = new GraphLoader().Load("gc_1000.txt", GraphLoader.GraphFormat.Minimal, out _);

                var anna = new GraphLoader().Load("CO-GC-instances/anna.col", GraphLoader.GraphFormat.Minimal, out _); // 11 11
                var games120 = new GraphLoader().Load("CO-GC-instances/games120.col", GraphLoader.GraphFormat.Minimal, out _); // 9 9
                var homer = new GraphLoader().Load("CO-GC-instances/homer.col", GraphLoader.GraphFormat.Minimal, out _); // 13 13
                var le450_15a = new GraphLoader().Load("CO-GC-instances/le450_15a.col", GraphLoader.GraphFormat.Minimal, out _);
                var le450_25a = new GraphLoader().Load("CO-GC-instances/le450_25a.col", GraphLoader.GraphFormat.Minimal, out _); // 25 kk
                var miles750 = new GraphLoader().Load("CO-GC-instances/miles750.col", GraphLoader.GraphFormat.Minimal, out _); // 31 31
                var miles1000 = new GraphLoader().Load("CO-GC-instances/miles1000.col", GraphLoader.GraphFormat.Minimal, out _); // 42 42
                var miles1500 = new GraphLoader().Load("CO-GC-instances/miles1500.col", GraphLoader.GraphFormat.Minimal, out _); // 73 70
                var myciel6 = new GraphLoader().Load("CO-GC-instances/myciel6.col", GraphLoader.GraphFormat.Minimal, out _);  // 7 30
                var queen9 = new GraphLoader().Load("CO-GC-instances/queen9_9.col", GraphLoader.GraphFormat.Minimal, out _); // 10 10
                var queen11 = new GraphLoader().Load("CO-GC-instances/queen11_11.col", GraphLoader.GraphFormat.Minimal, out _); // 11 13
                var queen13 = new GraphLoader().Load("CO-GC-instances/queen13_13.col", GraphLoader.GraphFormat.Minimal, out _); // 13 k
                var zeroini2 = new GraphLoader().Load("CO-GC-instances/zeroin.i.2.col", GraphLoader.GraphFormat.Minimal, out _); // 30 30
                var zeroini1 = new GraphLoader().Load("CO-GC-instances/zeroin.i.1.col", GraphLoader.GraphFormat.Minimal, out _);

                // ProcessGraph(le450_5a, "le450_5a", 10000, 200, 4);

                new Thread(() => ProcessGraph(graph, "test graf", 10000, 300, 7)).Start();
                //new Thread(() => ProcessGraph(le450_15a, "le450_15a", 10000, 300, 7)).Start();
                // new Thread(() => ProcessGraph(miles1000, "miles1000", 10000, 300, 7)).Start();
                // new Thread(() => ProcessGraph(miles1500, "miles1500", 1000, 400, 7)).Start();
                // new Thread(() => ProcessGraph(queen11, "queen11", 10000, 300, 7)).Start();
                // new Thread(() => ProcessGraph(queen13, "queen13", 10000, 300, 7)).Start();

                // new Thread(() => ProcessGraph(zeroini1, "zeroini1", 10000, 300, 7)).Start();
                // new Thread(() => ProcessGraph(zeroini2, "zeroini2", 10000, 300, 7)).Start();
                // new Thread(() => ProcessGraph(games120, "games120",1000, 300, 7)).Start(); //opt
                // new Thread(() => ProcessGraph(myciel6, "myciel6", 10000, 300, 7)).Start(); opt
                // new Thread(() => ProcessGraph(miles750, "miles750", 500000, 300, 7)).Start(); opt
                // new Thread(() => ProcessGraph(queen9, "queen9", 500000, 300, 7)).Start(); opt
                // new Thread(() => ProcessGraph(homer, "homer", 500000, 300, 7)).Start();opt
                // new Thread(() => ProcessGraph(anna, "anna", 500000, 300, 7)).Start(); opt

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
