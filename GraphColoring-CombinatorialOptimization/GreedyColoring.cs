using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    public class GreedyColoring : IGraphColoring
    {
        public int MaxColorId { get; set; } = -1;

        public GreedyColoring(int maxColorId = -1)
        {
            MaxColorId = maxColorId;
        }

        public int Color(Graph graph)
        {
            int maxColorId = 0;

            foreach (var vertex in graph.Vertices)
            {
                var neighborColors = GetNeighborColors(vertex);
                var color = GetFirstFreeColor(neighborColors);
                vertex.ColorId = color;
                maxColorId = Math.Max(color, maxColorId);

                if (maxColorId == MaxColorId)
                    break;
            }

            return maxColorId + 1;
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
