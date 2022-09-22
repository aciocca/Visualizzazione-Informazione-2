using System.Collections.Generic;

namespace GraphMLParser
{
    public class Edge
    {
        public string SourceNode { get; set; }
        public string DestinationNode { get; set; }
        public bool IsDirected { get; set; }
        public string EdgeID { get; set; } = "";

        public Dictionary<string, string> EdgeProprieties { get; set; } =  new Dictionary<string, string>();
    

        public Edge(string sourceNode, string destinationNode, bool isDirected)
        {
            this.SourceNode = sourceNode;
            this.DestinationNode = destinationNode;
            this.IsDirected = isDirected;
        }
    
    }
}