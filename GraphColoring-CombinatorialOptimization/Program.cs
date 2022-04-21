using System;

namespace GraphColoring
{
    class Program
    {
        private static void Main(string[] args)
        {
            var graph = new GraphGenerator().Generate(5, 0.2f);
            Console.WriteLine(graph.ToString());
        }
    }
}
