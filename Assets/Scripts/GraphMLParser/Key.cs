using System;
using System.Collections.Generic;

namespace GraphMLParser
{
    public class Key
    {
        // <key id="d0" for="node" attr.name="color" attr.type="string">
        // <default>yellow</default>
        // </key>
        private string _keyID; // <key id="d0">
        public string? KeyDefault; //<default>yellow</yellow>
        public string KeyFor; //<key for = "node">
        public string? KeyValue; //<data key="d0">Value</data>
    
        private readonly List<string> _allowedKeyFor =
            new List<string>()
                {"all", "graph", "node", "edge", "hyperedge", "port", "endpoint", "none"};
        //all, graph, node, edge, hyperedge, port and endpoint. 
        public Key(string keyID, string keyFor = "all", string? keyDefault = null, string? keyValue = null)
        {
            if (!_allowedKeyFor.Contains(keyFor))
            {
                throw new Exception($"Value for in key ({keyFor}) not valid.");
            }
            this._keyID = keyID;
            this.KeyDefault = keyDefault;
            this.KeyFor = keyFor;
            this.KeyValue = keyValue;
        }
    }
}