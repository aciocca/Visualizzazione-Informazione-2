using System.Collections.Generic;

namespace GraphMLParser
{
    public class Node
    {
        // <node id="17">
        // <data key="status">Alive</data>
        // <data key="house-birth">House Mormont</data>
        // <data key="name">Jorah Mormont</data>
        // </node>
        public string ID { get; set; }
        // Structure: 2 options
        // 1) Dictionary<Node, List<Edge>> -> Like this so I can quickly check for multiple connections between Node A and B
        // 2) Dictionary<string, List<String>> -> {NodeID, [EdgeID1, EdgeID2]}
        // Using 2 right now
        public Dictionary<string, List<string>> ConnectedNodes { get; set; } = new Dictionary<string, List<string>>();
        public Dictionary<string, Key> NodeKeyValues { get; set; } =  new Dictionary<string, Key>(); // propertyid: defaultvalue (or value if exists)
        public Node(string id)
        {
            //TODO port support
            this.ID = id;
        }


    }
}