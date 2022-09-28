using System;
using System.Collections.Generic;

namespace GraphMLParser
{
    public class Graph
    {
        // Is updated everytime I update a node, holds all the updated information about the nodes
        // string nodeID, Node node
        public Dictionary<string, Node> GraphNodes { get; set; } = new Dictionary<string, Node>();

        // Edges don't require an ID, so I'm storing them as
        // sourceNodeID: List<Edge>
        public Dictionary<string, List<Edge>> GraphEdges { get; set; } = new Dictionary<string, List<Edge>>();
        // I'm using Dictionaries for both even tough the id is also stored in Key.KeyID to:
        // 1) ensure uniqueness of the key 2) ease to access
        public Dictionary<string, Key> NodeAttributes { get; set; } = new Dictionary<string, Key>();
        public Dictionary<string, Key> EdgeAttributes { get; set; } = new Dictionary<string, Key>();
        public bool IsDirected { get; set; }

        public Node GetGraphNode(string nodeID)
        {
            try
            {
                return GraphNodes[nodeID];
            }
            catch (ArgumentException)
            {
                return new Node("invalidNode");
            }
        }
    }
}