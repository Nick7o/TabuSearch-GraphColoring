using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphColoring
{
    public class GraphVertex
    {
        public Graph Graph { get; set; }
        public string Identifier { get; set; }

        public int? ColorId { get; set; }

        public IEnumerable<GraphVertex> Neighbors
        {
            get
            {
                return Graph.Vertices.Where(v => v.Identifier != Identifier && NeighborIdentifiers.Contains(v.Identifier));
            }
        }

        public List<string> NeighborIdentifiers { get; set; }

        public GraphVertex()
        {
            NeighborIdentifiers = new List<string>();
            //Neighbors = new List<GraphVertex>();
        }

        public GraphVertex(string identifier) : this()
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Connects 2 vertices.
        /// </summary>
        /// <returns>True if connection was created successfully.</returns>
        public bool Connect(GraphVertex otherVertex)
        {
            if (otherVertex == null || NeighborIdentifiers.Contains(otherVertex.Identifier)
                || otherVertex.NeighborIdentifiers.Contains(Identifier))
                return false;

            NeighborIdentifiers.Add(otherVertex.Identifier);
            otherVertex.NeighborIdentifiers.Add(Identifier);
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

            NeighborIdentifiers.Remove(otherVertex.Identifier);
            otherVertex.NeighborIdentifiers.Remove(Identifier);
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
            return ToString(true);
        }

        public string ToString(bool printNeighbors)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{GetVertexBaseInfo(this)}");

            if (printNeighbors)
            {
                builder.Append($", Neighbors: \n[");

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
            }

            return builder.ToString();

            string GetVertexBaseInfo(GraphVertex vertex)
            {
                var colorId = vertex?.ColorId == null ? "NONE" : vertex?.ColorId.ToString();
                return $"{vertex?.Identifier} - Color Id: {colorId}, Neighbors Count: {vertex?.Neighbors?.Count()}";
            }
        }

        public GraphVertex Clone()
        {
            var clone = (GraphVertex)MemberwiseClone();
            NeighborIdentifiers = new List<string>(NeighborIdentifiers);
            return clone;
        }
    }
}
