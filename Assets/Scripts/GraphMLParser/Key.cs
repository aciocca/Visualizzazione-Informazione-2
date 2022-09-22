using System;
using System.Collections.Generic;

namespace GraphMLParser
{
    public class Key
    {
        public string keyID;
        public string keyDefault;
        public string keyFor;

        public string keyValue;

        private readonly List<string> allowedKeyFor =
            new List<string>()
                {"all", "graph", "node", "edge", "hyperedge", "port", "endpoint"};
        //all, graph, node, edge, hyperedge, port and endpoint. 
        public Key(string keyID, string keyDefault, string keyFor, string keyValue)
        {
            if (!allowedKeyFor.Contains(keyFor))
            {
                throw new Exception($"Value for in key ({keyFor}) not valid.");
            }
            this.keyID = keyID;
            this.keyDefault = keyDefault;
            this.keyFor = keyFor;
            this.keyValue = keyValue;
        }
    }
}