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
                var graph = new GraphLoader().Load("myciel4.txt", GraphLoader.GraphFormat.Minimal, out _);
                Console.WriteLine(graph.ToString(false));

                Console.WriteLine("\nPERFORMING GRAPH COLORING\n");

                new GreedyColoring().Color(graph);
                Console.WriteLine(graph.ToString(false));
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("Error occured while loading a file with a graph.");
                Console.WriteLine(exception);
            }
        }
    }
}
