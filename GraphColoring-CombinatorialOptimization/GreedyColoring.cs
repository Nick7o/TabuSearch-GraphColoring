using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    public class GreedyColoring : IGraphColoring
    {
        public void Color(Graph graph)
        {
            foreach (var vertex in graph.Vertices)
            {
                var neighborColors = GetNeighborColors(vertex);
                var color = GetFirstFreeColor(neighborColors);
                vertex.ColorId = color;
            }
        }

        private List<int> GetNeighborColors(GraphVertex vertex)
        {
            var neighborColors = new List<int>();

            foreach (var neighbor in vertex.Neighbors)
            {
                if (neighbor.ColorId.HasValue)
                    neighborColors.Add(neighbor.ColorId.Value);
            }

            return neighborColors;
        }

        private int GetFirstFreeColor(List<int> usedColors)
        {
            int freeColor = 0;
            while (usedColors.Contains(freeColor))
                freeColor++;

            return freeColor;
        }
    }
}
