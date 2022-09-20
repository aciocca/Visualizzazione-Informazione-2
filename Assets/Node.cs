using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

  GameObject epf;
  GameObject depf;
  List<GameObject>  edges  = new List<GameObject> ();
  [SerializeField] List<Node> tonodes = new List<Node>();
  [SerializeField] List<Node> comingnode = new List<Node>();
  List<SpringJoint> joints = new List<SpringJoint>();
  [SerializeField] List<Node> connected = new List<Node>();
  [SerializeField] public List<string> father = new List<string>();
  [SerializeField] public List<string> mother = new List<string>();
  [SerializeField] public List<string> killedby = new List<string>();
  [SerializeField] public List<string> siblings = new List<string>();
  [SerializeField] public List<string> childrens = new List<string>();
  [SerializeField] public List<string> killerof = new List<string>();
  public UnityEngine.Color familycolor = new UnityEngine.Color32();  
  
  void Start(){
    transform.GetChild(0).GetComponent<TextMesh>().text = name;
    transform.GetChild(0).GetComponent<TextMesh>().color = familycolor;    
  }
  public List<GameObject> GetEdges()
  {
    return this.edges;
  }
  public List<SpringJoint> GetJoints()
  {
    return this.joints;
  }
  public List<Node> GetConnectedNodes()
  {
    return this.connected;
  }
  public List<Node> GetOutgoingNodes()
  {
    return this.tonodes;
  }
  public List<Node> GetIncomingNodes()
  {
    return this.comingnode;
  }
  public string GetName()
  {
    return this.name;
  }
  void Update(){    
    int i = 0;
    foreach (GameObject edge in edges){
        edge.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        SpringJoint sj = joints[i];
        GameObject target = sj.connectedBody.gameObject;
        edge.transform.LookAt(target.transform);
        Vector3 ls = edge.transform.localScale;
        ls.z = Vector3.Distance(transform.position, target.transform.position);
        edge.transform.localScale = ls;
        edge.transform.position = new Vector3((transform.position.x+target.transform.position.x)/2,
                                              (transform.position.y+target.transform.position.y)/2,
                                              (transform.position.z+target.transform.position.z)/2);
      i++;
    }
  }

  public void SetEdgePrefab(GameObject epf){
    this.epf = epf; 
  }

  public void SetDirEdgePrefab(GameObject depf){
    this.depf = depf; 
  }

  public void AddIncomingNode(Node n)
  {
    this.comingnode.Add(n);
  }

  public void AddConnectedNode(Node n)
  {
    this.connected.Add(n);
  }

  public void AddOutgoingNode(Node n)
  {
    this.tonodes.Add(n);
  }

  public void AssignRelationship(Node targetnode, string relationship){
    if(relationship == "father")
    {
      targetnode.father.Add(this.GetName());
      this.childrens.Add(targetnode.name);
    }
    else if(relationship == "killed")
    {
      targetnode.killedby.Add(this.GetName());
      this.killerof.Add(targetnode.name);
    }
    else if(relationship == "mother")
    {
      targetnode.mother.Add(this.GetName());
      this.childrens.Add(targetnode.name);
    }
    else if(relationship == "sibling")
    {
      targetnode.siblings.Add(this.GetName());
      this.siblings.Add(targetnode.name);
    }
  }

  public void AddEdge(Node n, bool isdirected, string relationship){
    SpringJoint sj = gameObject.AddComponent<SpringJoint> ();
    sj.spring = 1; 
    sj.autoConfigureConnectedAnchor = false;
    sj.anchor = new Vector3(0,0.5f,0);
    sj.connectedAnchor = new Vector3(0,0,0);
    sj.enableCollision = true;
    sj.connectedBody = n.GetComponent<Rigidbody>();
    GameObject edge;
    if(isdirected){
      edge = Instantiate(this.depf, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
      if(n.GetOutgoingNodes().Contains(this))
      {
        edge.transform.GetChild(0).Rotate(0,0,180);
      }
      this.AddOutgoingNode(n);
      n.AddIncomingNode(this);
    }
    else{
      edge = Instantiate(this.epf, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
      this.connected.Add(n);
      n.AddConnectedNode(this);
    }
    AssignRelationship(n, relationship);
    edges.Add(edge);
    joints.Add(sj);

  }
}
