using System;

namespace GraphColoring
{
    class Program
    {
        private static void Main(string[] args)
        {
            //var graph = new GraphGenerator().Generate(10, 10, 1.0);

            try
            {
                var graph = new GraphLoader().Load("queen6.txt", GraphLoader.GraphFormat.Minimal, out _);
                //Console.WriteLine(graph.ToString(false));

                Console.WriteLine("\nPERFORMING GRAPH COLORING\n");

                int maxColorId = new GreedyColoring().Color(graph);
                Console.WriteLine(graph.ToString(false));
                Console.WriteLine($"NUMBER OF COLORS: {maxColorId}");
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("Error occured while loading a file with a graph.");
                Console.WriteLine(exception);
            }
        }
    }
}
