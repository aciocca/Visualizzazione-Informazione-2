using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NewNode : MonoBehaviour
    {
        //string name;
        string housebirth;
        string group;
        string status;
        string housemarriage;
        public UnityEngine.Color32 housecolor = new UnityEngine.Color32(0,0,0,255);
        List<GameObject> goedges = new List<GameObject>();
        List<SpringJoint> springjoints = new List<SpringJoint>();
        [SerializeField] List<NewNode> connected = new List<NewNode>();
        [SerializeField] List<NewNode> outgoing = new List<NewNode>();
        [SerializeField] List<NewNode> incoming = new List<NewNode>();
        GameObject edgepf;
        GameObject diredgepf;
        [SerializeField] public Dictionary<string, string> father = new Dictionary<string, string>();
        [SerializeField] public Dictionary<string, string> mother = new Dictionary<string, string>();
        [SerializeField] public List<string> killedby = new List<string>();
        [SerializeField] public Dictionary<string, string> siblings = new Dictionary<string, string>();
        [SerializeField] public Dictionary<string, string> childrens = new Dictionary<string, string>();
        [SerializeField] public List<string> lover = new List<string>();
        [SerializeField] public List<string> spouse = new List<string>();
        [SerializeField] public Dictionary<string, string> allegiance = new Dictionary<string, string>();
        [SerializeField] public List<string> killerof = new List<string>();
        public UnityEngine.Color32 familycolor = new UnityEngine.Color32(0,0,0,255);  

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetHousebirth(string housebirth)
        {
            this.housebirth = housebirth;
        }

        public void SetGroup(string group)
        {
            this.group = group;
        }
        public void SetStatus(string status)
        {
            this.status = status;
        }
        public void SetMarriage(string housemarriage)
        {
            this.housemarriage = housemarriage;
        }
        public void SetEdgePrefab(GameObject edgepf)
        {
            this.edgepf = edgepf;
        }
        public void SetDirEdgePrefab(GameObject diredgepf)
        {
            this.diredgepf = diredgepf;
        }

        public void SetColor(UnityEngine.Color32 color)
        {
            this.housecolor = color;
        }
        public void AddSpringJoint(SpringJoint sj){
            this.springjoints.Add(sj);
        }
        public void AddIncomingNode(NewNode n)
        {
            this.incoming.Add(n);
        }
        public void SetIncomingNodes(List<NewNode> incoming){
            this.incoming = incoming;
        }
        public void AddConnectedNode(NewNode n)
        {
            this.connected.Add(n);
        }
        public void SetConnectedNodes(List<NewNode> connected){
            this.connected = connected;
        }
        public void AddOutgoingNode(NewNode n)
        {
            this.outgoing.Add(n);
        }
        public void SetOutgoingNodes(List<NewNode> outgoing){
            this.outgoing = outgoing;
        }
        public string GetName()
        {
            return this.name;
        }
        public string GetHouseBirth()
        {
            return this.housebirth;
        }
        public string GetGroup()
        {
            return this.group;
        }
        public string GetStatus()
        {
            return this.status;
        }
        public string GetHouseMarriage()
        {
            return this.housemarriage;
        }
        public GameObject GetEdgePrefab()
        {
            return this.edgepf;
        }
        public GameObject GetDirEdgePrefab()
        {
            return this.diredgepf;
        }
        public UnityEngine.Color32 GetColor()
        {
            return this.housecolor;
        }
        public void AddNodeEdge(GameObject e)
        {
            this.goedges.Add(e);
        }
        public void SetEdges(List<GameObject> edges){
            this.goedges = edges;
        }
        public void SetJoints(List<SpringJoint> sj){
            this.springjoints = sj;
        }
        public List<GameObject> GetEdges(){
            return this.goedges;
        }
        public List<SpringJoint> GetJoints(){
            return this.springjoints;
        }
        public List<NewNode> GetConnectedNodes()
        {
            return this.connected;
        }
        public List<NewNode> GetOutgoingNodes()
        {
            return this.outgoing;
        }
        public List<NewNode> GetIncomingNodes()
        {
            return this.incoming;
        }

        public string GetHexHouseColor(){
            return "#" + ColorUtility.ToHtmlStringRGBA(GetColor());
        }

        public void AssignRelationship(NewNode targetnode, string relationship, string typeofrelationship){
            if(relationship == "father")
            {
            targetnode.father.Add(this.GetName(),typeofrelationship);
            this.childrens.Add(targetnode.name, typeofrelationship);
            }
            else if(relationship == "killed")
            {
            targetnode.killedby.Add(this.GetName());
            this.killerof.Add(targetnode.name);
            }
            else if(relationship == "mother")
            {
            targetnode.mother.Add(this.GetName(), typeofrelationship);
            this.childrens.Add(targetnode.name, typeofrelationship);
            }
            else if(relationship == "sibling")
            {
            targetnode.siblings.Add(this.GetName(),typeofrelationship);
            this.siblings.Add(targetnode.name, typeofrelationship);
            }
            else if(relationship == "lover")
            {
            targetnode.lover.Add(this.GetName());
            this.lover.Add(targetnode.name);
            }
            else if(relationship == "spouse")
            {
                targetnode.spouse.Add(this.GetName());
                this.spouse.Add(targetnode.name);
            }
            else if(relationship == "allegiance")
            {
                targetnode.allegiance.Add(this.GetName(), typeofrelationship);
                this.allegiance.Add(targetnode.name, typeofrelationship);
            }
        }
        void ColorStatus(){
            string word = GetStatus();
            var nodeRenderer = this.gameObject.GetComponent<Renderer>();
            if (word == "Alive"){
                nodeRenderer.material.color = Color.green;
            }
            if (word == "Deceased"){
                nodeRenderer.material.color = Color.red;
            }
            if (word == "Uncertain"){
                nodeRenderer.material.color = Color.gray;
            }
        }
        void Start(){
            //TextMeshProUGUI text = transform.GetComponentInChildren<TextMeshProUGUI>();
            transform.GetComponentInChildren<TextMesh>().color = housecolor;
            //text.color = housecolor;
            //transform.GetChild(0).GetComponent<TextMesh>().color = familycolor;
            ColorStatus();
            transform.GetComponentInChildren<TextMesh>().text = name;
            //text.text = name;
            //transform.GetChild(0).GetComponent<TextMesh>().text = name;
        }
        void Update(){    
            int i = 0;
            foreach (GameObject edge in GetEdges()){
                edge.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                SpringJoint sj = GetJoints()[i];
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
 
    }
