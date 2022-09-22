using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace GraphMLParser
{
    internal class GraphMLParser
    {
        public Graph LoadGraphFromFile(string filepath)
        {
            return ParseGML(LoadGML(filepath));
        }
        private Graph ParseGML(XDocument xDoc)
        {
            Graph graph = new Graph();
            (graph.NodeAttributes, graph.EdgeAttributes, graph.IsDirected) = ReadHeader(xDoc);
            graph.GraphNodes = GetNodes(xDoc, graph.NodeAttributes);
            List<Edge> edgeList = GetEdges(xDoc, ref graph); // I'm passing the reference to the graph here to avoid
            // cycling over the list of edges an extra time
            graph.GraphEdges = edgeList;
            return graph;
        }
        private XDocument LoadGML(string filepath)
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
        private static void SetNodeConnections(Node source, Node target, bool isDirected)
        {
            if (isDirected)
            {
                try
                {
                    source.ConnectedNodes.Add(target.ID, new List<ushort>() { 3 });
                }
                catch (Exception ex) when(ex is ArgumentException)
                {
                    source.ConnectedNodes[target.ID].Add(3);
                }

                try
                {
                    target.ConnectedNodes.Add(source.ID, new List<ushort>() { 2 });
                }
                catch (ArgumentException)
                {
                    target.ConnectedNodes[source.ID].Add(2);
                }
            
            
            }
            else
            {
                try
                {
                    source.ConnectedNodes.Add(target.ID, new List<ushort>(){1});
                }
                catch (ArgumentException)
                {
                    source.ConnectedNodes[target.ID].Add(1);
                }

                try
                {
                    target.ConnectedNodes.Add(source.ID, new List<ushort>(){1});
                }
                catch (ArgumentException)
                {
                    target.ConnectedNodes[source.ID].Add(1);
                }
            }
        }

        private static bool IsValidGML(XDocument xDoc)
        {
            return xDoc.Root?.Name.LocalName == "graphml";
        }
    
        private (Dictionary<string, string?>, Dictionary<string, string?>, bool) ReadHeader(XDocument xDoc){
            Dictionary<string, string?> nodeAttributes = new Dictionary<string, string?>();
            Dictionary<string, string?> edgeAttributes = new Dictionary<string, string?>();
            XNamespace df = xDoc.Root!.Name.Namespace;
            IEnumerable<XElement> attributes = xDoc.Descendants(df + "key");
            foreach (XElement at in attributes)
            {
                if (at.Attribute("id")?.Value == null) continue;
                if (at.Attribute("for") == null)
                {
                    nodeAttributes.Add(at.Attribute("id")!.Value,
                        at.Descendants("default").Any() ? at.Descendants("default").First().Value : null);
                    edgeAttributes.Add(at.Attribute("id")!.Value, 
                        at.Descendants("default").Any() ? at.Descendants("default").First().Value : null);
                }
                switch (at.Attribute("for")?.Value)
                {
                    case "node":
                        // status: defaultstatus, house-birth: defaulthousebirth ecc 
                        nodeAttributes.Add(at.Attribute("id")!.Value,
                            at.Descendants("default").Any() ? at.Descendants("default").First().Value : null);
                        break;
                    case "edge":
                        edgeAttributes.Add(at.Attribute("id")!.Value, 
                            at.Descendants("default").Any() ? at.Descendants("default").First().Value : null);
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
    
    
        private static List<Node> GetNodes(XDocument xDoc, Dictionary<string, string?> nodeAttributes)
        {
            List<Node> nodeList = new List<Node>();
            IEnumerable<XElement> nodes = xDoc.Descendants("node");
            foreach (XElement node in nodes)
            {
                if (node.Attribute("id") == null || node.Attribute("id")?.Value == null) continue;
                Node nd = new Node(node.Attribute("id")!.Value);
                foreach (XElement child in node.Descendants())
                {
                    if (child.Attribute("key") == null|| child.Attribute("key")?.Value == null) continue;
                    string attributeID = child.Attribute("key")!.Value;
                    foreach (KeyValuePair<string, string?> ndAttr in nodeAttributes)
                    {
                        if (attributeID != ndAttr.Key) continue;
                        if (child.IsEmpty)
                        {
                            nd.NodeProprieties[ndAttr.Key] = ndAttr.Value;
                        }
                        else
                        {
                            nd.NodeProprieties[ndAttr.Key] = child.Value;
                        }
                    }
                }
                nodeList.Add(nd);
            }
            return nodeList;
        }
        private static List<Edge> GetEdges(XDocument xDoc, ref Graph graph)
        {
            IEnumerable<XElement> edges = xDoc.Descendants("edge");
            List<Edge> edgeList = new List<Edge>();
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
                Edge ed = new Edge(source, target, directed);
            
                foreach(XElement child in edge.Descendants())
                {
                    if (child.Attribute("key") == null || child.Attribute("key")?.Value == null) continue;
                    string attributeID = child.Attribute("key")!.Value;
                    foreach (KeyValuePair<string, string?> edgAttr in graph.EdgeAttributes)
                    {
                        if (attributeID != edgAttr.Key) continue;
                        if (child.IsEmpty)
                        {
                            ed.EdgeProprieties[edgAttr.Key] = edgAttr.Value;
                        }
                        else
                        {
                            ed.EdgeProprieties[edgAttr.Key] = child.Value;
                        }
                    }
                }
                edgeList.Add(ed);
                // Updating the connected edges here to save cycling through the list an extra time
                try
                {
                    SetNodeConnections(graph.GetGraphNode(source), graph.GetGraphNode(target), directed);
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