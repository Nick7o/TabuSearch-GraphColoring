using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    public class LFColoring : IGraphColoring
    {
        private Graph _graph;

        public int Color(Graph graph)
        {
            _graph = graph;

            var sortedVertices = SortVerticesByDegree();
            _graph.Vertices = sortedVertices;

            return new GreedyColoring().Color(graph);
        }

        private List<GraphVertex> SortVerticesByDegree()
        {
            return _graph.Vertices.OrderByDescending(v => v.Neighbors.Count).ToList();
        }
    }
}
