using System.Collections.Generic;

namespace GraphMLParser
{
    public class Edge
    {
        // <edge id="e1" source="n0" target="n1">
        // <data key="d1">1.0</data>
        // </edge>
        public string SourceNode { get; set; }
        public string DestinationNode { get; set; }
        public bool IsDirected { get; set; }
        public string EdgeID { get; set; }

        public Dictionary<string, Key> EdgeKeyValues { get; set; } = new Dictionary<string, Key>();
    

        public Edge(string sourceNode, string destinationNode, bool isDirected, string edgeID = "")
        {
            //TODO port support
            this.SourceNode = sourceNode;
            this.DestinationNode = destinationNode;
            this.IsDirected = isDirected;
            this.EdgeID = edgeID;
        }
    
    }
}