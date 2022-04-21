using System;

namespace GraphColoring
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Generates graph of 8 vertices, up to 5 (+1 mandatory edge) random edges per vertex,
            // and last parameter is probability of creating an additional edge
            var graph = new GraphGenerator().Generate(8, 5, 0.3);
            Console.WriteLine(graph.ToString());
        }
    }
}
