using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    public class GraphVertex
    {
        public int? ColorId { get; set; }

        public List<GraphVertex> Neighbors { get; set; }

        public GraphVertex()
        {
            Neighbors = new List<GraphVertex>();
        }

        /// <summary>
        /// Connects 2 vertices.
        /// </summary>
        /// <returns>True if connection was created successfully.</returns>
        public bool Connect(GraphVertex otherVertex)
        {
            if (otherVertex == null || Neighbors.Contains(otherVertex) || otherVertex.Neighbors.Contains(this))
                return false;

            Neighbors.Add(otherVertex);
            otherVertex.Neighbors.Add(this);
            return true;
        }

        /// <summary>
        /// Disconnects 2 vertices.
        /// </summary>
        /// <returns>True if connection was deleted successfully.</returns>
        public bool Disconnect(GraphVertex otherVertex)
        {
            if (otherVertex == null || !Neighbors.Contains(otherVertex) || !otherVertex.Neighbors.Contains(this))
                return false;

            Neighbors.Remove(otherVertex);
            otherVertex.Neighbors.Remove(this);
            return true;
        }

        public bool IsConnectedWith(GraphVertex otherVertex)
        {
            if (otherVertex == null)
                return false;

            return Neighbors.Contains(otherVertex);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{GetVertexBaseInfo(this)}, Neighbors: \n[");

            bool printedFirstNeighbor = false;
            foreach (var neighbor in Neighbors)
            {
                if (printedFirstNeighbor)
                    builder.Append(", ");
                else
                    printedFirstNeighbor = true;
                builder.Append(Environment.NewLine);
                builder.Append("\t");

                builder.Append(GetVertexBaseInfo(neighbor));
            }

            builder.Append("\n]");
            return builder.ToString();

            string GetVertexBaseInfo(GraphVertex vertex)
            {
                var colorId = vertex?.ColorId == null ? "NONE" : vertex?.ColorId.ToString();
                return $"Vertex - Color Id: {colorId}, Neighbors Count: {vertex?.Neighbors?.Count}";
            }
        }
    }
}
