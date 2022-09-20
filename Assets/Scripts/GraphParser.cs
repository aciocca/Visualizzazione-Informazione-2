using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System;
using System.Linq;
using Random=UnityEngine.Random;
using System.IO;

class GraphParser : MonoBehaviour
{
    public TextAsset file;
    public TextAsset colorfile;
    public List<Color> colorList;
    public GameObject nodepf;
    public GameObject edgepf;
    public GameObject diredgepf; 
    public float width;
    public float length;
    public float height;
    void Start()
    {
        LoadColorsFromFile(colorfile);
        XDocument xDoc;
        using (StringReader s = new StringReader(file.text))
        {
            xDoc = XDocument.Load(s);
        }
        Dictionary<string,GameObject> nodedictionary = new Dictionary<string, GameObject>();
        nodedictionary = GetNodes(xDoc);
        Dictionary<string, List<NewEdge>> outgoingedges = new Dictionary<string, List<NewEdge>>();
        Dictionary<string, List<NewEdge>> incomingedges = new Dictionary<string, List<NewEdge>>();
        //(outgoingedges, incomingedges) = GetEdges(xDoc, nodedictionary);
        GetEdges(xDoc, nodedictionary);
        ReadHeader(xDoc);
        NewNode starting_node = nodedictionary["0"].GetComponent<NewNode>();
        Rigidbody _rigidbody = starting_node.GetComponent<Rigidbody>();
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;        
    }
    

    void LoadColorsFromFile(TextAsset f){
		string[] lines = f.text.Split('\n');
        foreach(string line in lines){
			string[] colorint = line.Split(',');
			colorList.Add(new Color32(byte.Parse(colorint[0]), byte.Parse(colorint[1]), byte.Parse(colorint[2]), 255));
		}
    }
    int Modulo(int x,int N){
        return (x % N + N) %N;
    }

    public Dictionary<string, string> ReadHeader(XDocument xDoc){
        Dictionary<string, string> nodeattributes = new Dictionary<string, string>();
        Dictionary<string, string> edgeattributes = new Dictionary<string, string>();
        var attributes = xDoc.Descendants("key");
        foreach (var at in attributes)
        {
            if(at.Attribute("for")?.Value == "node"){
                nodeattributes.Add(at.Attribute("attr.name").Value, at.Attribute("attr.type").Value);
            }
            else
            {
                edgeattributes.Add(at.Attribute("attr.name").Value, at.Attribute("attr.type").Value);
            }
            
        }
        return nodeattributes;
    }


    public Dictionary<string, GameObject> GetNodes(XDocument xDoc)
    {
        Dictionary<string,GameObject> nodedictionary = new Dictionary<string,GameObject>();
        var  nodes = xDoc.Descendants("node");
        foreach(var node in nodes)
        {
            NewNode newnode = null;
            GameObject go = Instantiate(nodepf, new Vector3(Random.Range(-width/2, width/2), Random.Range(-length/2, length/2), Random.Range(-height/2, height/2)), Quaternion.identity);
            newnode = go.GetComponent<NewNode>();
            newnode.transform.parent = transform;
            newnode.SetEdgePrefab(edgepf);
            newnode.SetDirEdgePrefab(diredgepf);        
            
            string nodeid = node.Attribute("id").Value;
            //Hack per non avere elementi senza colore
            newnode.SetHousebirth("No housebirth");
            int hash = Modulo("No housebirth".GetHashCode(),colorList.Count-1);
            newnode.SetColor(colorList[hash]);
            foreach(var child in node.Descendants())
            {
                string attributetype = child.Attribute("key").Value;
                if(attributetype == "name"){
                    newnode.SetName(child.Value);
                }
                else if(attributetype == "status"){
                    newnode.SetStatus(child.Value);
                }
                else if(attributetype == "house-birth"){
                    newnode.SetHousebirth(child.Value);
                    hash = Modulo(child.Value.GetHashCode(),colorList.Count-1);
                    newnode.SetColor(colorList[hash]);
                }
                else if(attributetype == "house-marriage"){
                    newnode.SetMarriage(child.Value);
                }
                else if(attributetype == "group"){
                    newnode.SetGroup(child.Value);
                }
            }
            nodedictionary.Add(nodeid, go);
        }
        return nodedictionary;
    }
    
    public void AddEdge(string from, string to, bool isdirected, string relationship, Dictionary<string, GameObject> nodedictionary, string typeofrelationship)
        {
            GameObject gosourcenode = nodedictionary[from];
            GameObject gotargetnode = nodedictionary[to];
            NewNode sourcenode = gosourcenode.GetComponent<NewNode>();
            NewNode targetnode = gotargetnode.GetComponent<NewNode>();
            SpringJoint sj = gosourcenode.AddComponent<SpringJoint>();
            sj.spring = 1; 
            sj.autoConfigureConnectedAnchor = false;
            sj.anchor = new Vector3(0,0.5f,0);
            sj.connectedAnchor = new Vector3(0,0,0);
            sj.enableCollision = true;
            sj.connectedBody = gotargetnode.GetComponent<Rigidbody>();
            GameObject goedge;
            if(isdirected){
                goedge = Instantiate(sourcenode.GetDirEdgePrefab(), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                if(targetnode.GetOutgoingNodes().Contains(sourcenode))
                {
                    goedge.transform.GetChild(0).Rotate(0,0,180); // Ruoto il line renderer
                }
                sourcenode.AddOutgoingNode(targetnode);
                targetnode.AddIncomingNode(sourcenode);
            }
            else{
                goedge = Instantiate(sourcenode.GetEdgePrefab(), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                sourcenode.AddConnectedNode(targetnode);
                targetnode.AddConnectedNode(sourcenode);
            }
            sourcenode.AssignRelationship(targetnode, relationship, typeofrelationship);
            sourcenode.AddNodeEdge(goedge);
            sourcenode.AddSpringJoint(sj);
        }
    
    
    
    public void GetEdges(XDocument xDoc, Dictionary<string, GameObject> nodedictionary)
    {
        var edges = xDoc.Descendants("edge");
        
        foreach(var edge in edges)
        {
            ParsingEdge newedge = new ParsingEdge();
            string source = edge.Attribute("source").Value;
            string target = edge.Attribute("target").Value;
            if(edge.Attribute("directed")?.Value == null)
            {
                newedge.SetDirected(true);
            }
            else
            {
                newedge.SetDirected(false);
            }
            newedge.SetFromID(source);
            newedge.SetToID(target);
            foreach(var data in edge.Descendants())
            {
                string attributetype = data.Attribute("key").Value;
                if(attributetype == "type"){
                    newedge.SetTypeRelationship(data.Value);
                }
                else if(attributetype == "relation"){
                    newedge.SetRelationship(data.Value);
                }
            }
            AddEdge(newedge.GetFromID(), newedge.GetToID(), newedge.IsDirected(), newedge.GetRelationship(), nodedictionary, newedge.GetTypeRelationship());
        }
    }
    
}
