using System;

namespace GraphColoring
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var graph = new GraphLoader().Load("queen6.txt", GraphLoader.GraphFormat.Minimal, out _);

                int maxColorId = new GreedyColoring().Color(graph);
                var tabuGraph = graph.Clone();

                var tabuSearchColoring = new TabuSearchColoring(maxColorId);
                int maxColorTabu = tabuSearchColoring.Color(graph);

                Console.WriteLine($"GREEDY NUMBER OF COLORS: {maxColorId}");
                Console.WriteLine($"TABU NUMBER OF COLORS: {maxColorTabu}");
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("Error occured while loading a file with a graph.");
                Console.WriteLine(exception);
            }
        }
    }
}
