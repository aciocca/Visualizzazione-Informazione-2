using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace GraphMLParser
{
    class GraphMLParser
    {
        public Graph LoadGraphFromFile(string filepath)
        {
            return ParseGML(LoadGML(filepath));
        }
        private Graph ParseGML(XDocument xDoc)
        {
            Graph graph = new Graph();
            (graph.NodeAttributes, graph.EdgeAttributes, graph.IsDirected) = ReadHeader(xDoc); // nodeAttributes is a list
            // of all possible attributes
            // for the node,
            // with their default value
            // if it exits
            graph.GraphNodes = GetNodes(xDoc, graph.NodeAttributes);
            graph.GraphEdges = GetEdges(xDoc, ref graph); // I'm passing the reference to the graph here to avoid
            // cycling over the list of edges an extra time
            return graph;
        }
        private static XDocument LoadGML(string filepath)
        {
            XDocument xDoc = new XDocument();
            try
            {
                xDoc = XDocument.Load(filepath);
                if (!IsValidGML(xDoc))
                {
                    throw new XmlException("Not a valid graphml");
                }
            }
            catch(XmlException)
            {
                Console.WriteLine(".xml is not valid");
                Environment.Exit(1);
            }
            RemoveAllNamespaces(ref xDoc); //Removes the namespaces so Descendants() doesn't need the namespace everytime
            return xDoc;
        }
        private static void RemoveAllNamespaces(ref XDocument xDoc)
        {
            // https://stackoverflow.com/questions/40517306/c-sharp-how-to-remove-xmlns-from-xelement
            foreach (XElement node in xDoc.Root!.DescendantsAndSelf())
            {
                node.Name = node.Name.LocalName;
            }
        }
        private static void SetNodeConnections(Node source, Node target, Edge edge)
        {
            try
            {
                source.ConnectedNodes.Add(target.ID, new List<string>() { edge.EdgeID });
            }
            catch (ArgumentException)
            {
                source.ConnectedNodes[target.ID].Add(edge.EdgeID);
            }

            try
            {
                target.ConnectedNodes.Add(source.ID, new List<string>() { edge.EdgeID });
            }
            catch (ArgumentException)
            {
                target.ConnectedNodes[source.ID].Add(edge.EdgeID);
            }
        }

        private static bool IsValidGML(XDocument xDoc)
        {
            return xDoc.Root?.Name.LocalName == "graphml";
        }
    
        private static (Dictionary<string, Key>, Dictionary<string, Key>, bool) ReadHeader(XDocument xDoc)
        {
            Dictionary<string, Key> nodeAttributes = new Dictionary<string, Key>();
            Dictionary<string, Key> edgeAttributes = new Dictionary<string, Key>();
            // XNamespace df = xDoc.Root!.Name.Namespace; purged namespaces
            // IEnumerable<XElement> attributes = xDoc.Descendants(df + "key");
            IEnumerable<XElement> attributes = xDoc.Descendants("key");
            foreach (XElement at in attributes)
            {
            
                if (at.Attribute("id") == null || at.Attribute("id")?.Value == null) continue;
                Key key = new Key(at.Attribute("id")!.Value)
                {
                    KeyDefault = at.Descendants("default").Any() ? at.Descendants("default").First().Value : null
                };
                if (at.Attribute("for") == null)
                {
                    nodeAttributes.Add(at.Attribute("id")!.Value, key);
                    edgeAttributes.Add(at.Attribute("id")!.Value, key);
                }
                switch (at.Attribute("for")?.Value)
                {
                    case "node":
                        // status: defaultstatus, house-birth: defaulthousebirth ecc
                        key.KeyFor = "node";
                        nodeAttributes.Add(at.Attribute("id")!.Value, key);
                        break;
                    case "edge":
                        key.KeyFor = "edge";
                        edgeAttributes.Add(at.Attribute("id")!.Value, key);
                        break;
                }
                //TODO all, graph, hyperedge, port and endpoint. 
            }
            bool isDirected = false;
            IEnumerable <XElement> graphDefaults = xDoc.Descendants("graph");
            foreach (XElement grDef in graphDefaults)
            {
                if(grDef.Attribute("edgedefault") != null && grDef.Attribute("edgedefault")?.Value != null)
                    isDirected = (grDef.Attribute("edgedefault")!.Value == "directed");
            }
        
            return (nodeAttributes, edgeAttributes, isDirected);
        }
    
    
        private static Dictionary<string, Node> GetNodes(XDocument xDoc, Dictionary<string, Key> nodeAttributes)
        {
            Dictionary<string, Node> nodeList = new Dictionary<string, Node>();
            IEnumerable<XElement> nodes = xDoc.Descendants("node");
            foreach (XElement node in nodes)
            {
                // Filtering out wrongly formatted nodes
                if (node.Attribute("id") == null || node.Attribute("id")?.Value == null) continue;
                Node nd = new Node(node.Attribute("id")!.Value)
                {
                    // Setting all the possible node attributes and their default values, if they exists
                    NodeKeyValues = nodeAttributes
                };
                foreach (XElement child in node.Descendants())
                {
                    // Filtering out wrongly formatted nodes
                    if (child.Attribute("key") == null|| child.Attribute("key")?.Value == null) continue;
                    Key key = new Key(child.Attribute("key")!.Value)
                    {
                        KeyValue = child.Value
                    };
                    nd.NodeKeyValues[child.Attribute("key")!.Value] = key; 
                }
                nodeList.Add(node.Attribute("id")!.Value,nd);
            }
            return nodeList;
        }
        private static Dictionary<string, List<Edge>> GetEdges(XDocument xDoc, ref Graph graph)
        {
            IEnumerable<XElement> edges = xDoc.Descendants("edge");
            Dictionary<string, List<Edge>> edgeList = new Dictionary<string, List<Edge>>();
            bool directed = graph.IsDirected;
            foreach(XElement edge in edges)
            {
                //#TODO Flag to decide if I want to ignore wrongly formatted graphs or not
                if(edge.Attribute("source") == null || edge.Attribute("target") == null) continue;
                string source = edge.Attribute("source")!.Value;
                string target = edge.Attribute("target")!.Value;
                directed = edge.Attribute("directed")?.Value switch
                {
                    "directed" => true,
                    "undirected" => false,
                    _ => directed
                };
                Edge ed = new Edge(source, target, directed)
                {
                    EdgeKeyValues = graph.EdgeAttributes
                };
            
                foreach(XElement child in edge.Descendants())
                {
                    // Filtering out wrongly formatted nodes
                    if (child.Attribute("key") == null || child.Attribute("key")?.Value == null) continue;
                    Key key = new Key(child.Attribute("key")!.Value)
                    {
                        KeyValue = child.Value
                    };
                    ed.EdgeKeyValues[child.Attribute("key")!.Value] = key;
                }

                try
                {
                    // Check if it already is on the list
                    edgeList[ed.SourceNode].Add(ed);
                }
                catch (KeyNotFoundException)
                {
                    edgeList.Add(ed.SourceNode, new List<Edge>(){ed});
                }
                // Updating the connected edges here to save cycling through the list an extra time
                try
                {
                    SetNodeConnections(graph.GetGraphNode(source), graph.GetGraphNode(target), ed);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Environment.Exit(1);
                }
            }
            return edgeList;
        }
    }
}