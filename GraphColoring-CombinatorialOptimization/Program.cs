using System;

namespace GraphColoring
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Generates graph of 8 vertices, up to 5 (+1 mandatory edge) random edges per vertex,
            // and last parameter is probability of creating an additional edge
            //var graph = new GraphGenerator().Generate(8, 5, 0.3);
            //Console.WriteLine(graph.ToString());

            try
            {
                var graph = new GraphLoader().Load("myciel4.txt", GraphLoader.GraphFormat.Minimal, out var optimalColorCount);
                Console.WriteLine($"OPTIMAL COLOR COUNT: {optimalColorCount}");
                Console.WriteLine(graph.ToString());
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Console.WriteLine("Error occured while loading a file with a graph.");
                Console.WriteLine(exception);
            }
        }
    }
}
