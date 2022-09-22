using System;
using System.Collections.Generic;

namespace GraphMLParser
{
    public class Graph
    {
        public List<Node> GraphNodes { get; set; }
        public List<Edge> GraphEdges { get; set; }
        public Dictionary<string, string?> NodeAttributes { get; set; }
        public Dictionary<string, string?> EdgeAttributes { get; set; }
        public bool IsDirected { get; set; }

        public Node GetGraphNode(string nodeID)
        {
            Node nd = null;
            foreach (Node graphNode in GraphNodes)
            {
                if (nodeID != graphNode.ID) continue;
                nd = graphNode;
                break;
            }

            if (nd == null)
            {
                throw new Exception("Node not found");
            }

            return nd;
        }
    }
}