#nullable enable
using System.Collections.Generic;

namespace GraphMLParser
{
    public class Node
    {
        public string ID { get; set; }
        public Dictionary<string, List<ushort>> ConnectedNodes { get; set; } = new Dictionary<string, List<ushort>>();// 1 non directed, 2 incoming, 3 outgoing, 0 non valido
        public Dictionary<string, string?> NodeProprieties { get; set; } =  new Dictionary<string, string?>();//propertyid: defaultvalue #nullable

        public Node(string id)
        {
            this.ID = id;
        }

    

    }
}