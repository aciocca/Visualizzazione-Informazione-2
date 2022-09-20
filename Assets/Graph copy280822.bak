using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph : MonoBehaviour
{

  public byte auraAlpha = 255;
  public TextAsset file;
  public TextAsset colorFile;
  public List<Color> colorList;
  public GameObject nodepf;
  public GameObject edgepf;
  public GameObject diredgepf; 
  public float width;
  public float length;
  public float height;
  
    void Start()
    {      
      if (file==null){	
	// instantiate A, B, C, D, E
	GameObject A = Instantiate(nodepf, new Vector3(Random.Range(-width/2, width/2), Random.Range(-length/2, length/2), Random.Range(-height/2, height/2)), Quaternion.identity);
	GameObject B = Instantiate(nodepf, new Vector3(Random.Range(-width/2, width/2), Random.Range(-length/2, length/2), Random.Range(-height/2, height/2)), Quaternion.identity);
	GameObject C = Instantiate(nodepf, new Vector3(Random.Range(-width/2, width/2), Random.Range(-length/2, length/2), Random.Range(-height/2, height/2)), Quaternion.identity);
	GameObject D = Instantiate(nodepf, new Vector3(Random.Range(-width/2, width/2), Random.Range(-length/2, length/2), Random.Range(-height/2, height/2)), Quaternion.identity);
	GameObject E = Instantiate(nodepf, new Vector3(Random.Range(-width/2, width/2), Random.Range(-length/2, length/2), Random.Range(-height/2, height/2)), Quaternion.identity);      
	// make nodes children of graph object
	A.transform.parent = transform;
	B.transform.parent = transform;
	C.transform.parent = transform;
	D.transform.parent = transform;
	E.transform.parent = transform;
	// change name
	A.name = "node A"; 
	B.name = "node B";
	C.name = "node C";
	D.name = "node D";
	E.name = "node E";
	// get script instances
	Node AS = A.GetComponent<Node>();
	Node BS = B.GetComponent<Node>();
	Node CS = C.GetComponent<Node>();
	Node DS = D.GetComponent<Node>();
	Node ES = E.GetComponent<Node>();                  
	// add edges      
	AS.SetEdgePrefab(edgepf); AS.AddEdge(BS, false, "");
	AS.AddEdge(CS, false, "");
	CS.SetEdgePrefab(edgepf); CS.AddEdge(DS, false, ""); 
	DS.SetEdgePrefab(edgepf); DS.AddEdge(ES, false, "");
	DS.AddEdge(AS, false, ""); 
      } else {	
	LoadColorsFromFile(colorFile);
	LoadGMLFromFile(file);
      }      
    }

    void Update(){}

	void LoadColorsFromFile(TextAsset f){
		string[] lines = f.text.Split('\n');
		foreach(string line in lines){
			string[] colorint = line.Split(',');
			colorList.Add(new Color32(byte.Parse(colorint[0]), byte.Parse(colorint[1]), byte.Parse(colorint[2]), auraAlpha));
		}
	}
    void LoadGMLFromFile(TextAsset f){
      string[] lines = f.text.Split('\n');
      int currentobject = -1; // 0 = graph, 1 = node, 2 = edge
      int stage = -1; // 0 waiting to open, 1 = waiting for attribute, 2 = waiting for id, 3 = waiting for label, 4 = waiting for source, 5 = waiting for target
      string lasttarget = "0";
	  Node n = null;
	  string currentreleation = "";
      Dictionary<string,Node> nodes = new Dictionary<string,Node>();
      foreach (string line in lines){
		string l = line.Trim();
		string [] words = l.Split(' ');
		string lastwordinline = words.Last();
		foreach(string word in words){
			if (word == "graph" && stage == -1) {
				currentobject = 0;
			}
			if (word == "node" && stage == -1) {
				currentobject = 1;
				stage = 0;	    
			}
			if (word == "edge" && stage == -1) {
				currentobject = 2;
				stage = 0;	    
			}
			if (word == "[" && stage == 0 && currentobject == 2){
				// è un arco e sto aspettando di aprire
				stage = 1;
			}
			if (word == "[" && stage == 0 && currentobject == 1){
				// è un nodo e sto aspettando di aprire
				// quindi ora aspetto gli attributi
				stage = 1;
				GameObject go = Instantiate(nodepf, new Vector3(Random.Range(-width/2, width/2), Random.Range(-length/2, length/2), Random.Range(-height/2, height/2)), Quaternion.identity);
				n = go.GetComponent<Node>();
				n.transform.parent = transform; // La transfrom è la posizione in 3D dell'oggetto
				n.SetEdgePrefab(edgepf); // Assegna al nodo n la prefab dell'arco (var globale)
				n.SetDirEdgePrefab(diredgepf);
				continue;
			}
			if (word == "]"){
				// Ho finito il nodo/arco/grafo, quindi mi rimetto in posizione di partenza
				stage = -1;
			}
			if (word == "id" && stage == 1 && currentobject == 1){
				// Sono in un nodo, sto aspettando gli attributi, sto aspettando il valore dell'id
				stage = 2; // Sono nello step che si aspetta un ID, passo al prossimo ciclo
				continue;
			}
			if (word == "name" && stage == 1 && currentobject == 1){
				// Sono in un nodo, sto aspettando gli attributi, sto aspettando il valore dell'etichetta name
				stage = 3; // Sono nello step che si aspetta un etichetta name, passo alla prossima parola
				n.name = ""; // Svuoto la varibile dal nome di default che aveva, forse posso farlo dal prefab
				continue;
			}
			if (stage == 2){
				// Mi stavo aspettando un id, quindi quello che ho ora è un id
				nodes.Add(word, n); // Aggiungo il nodo assegnandogli l'id appena letto
				stage = 1; // Siccome ho appena aggiunto un nodo, ora mi aspetto di leggere i suoi attributi
				break; // Passo alla linea successiva
			}
			if (stage == 3){
				// Mi stavo aspettando un nome, quindi ora ho il nome del nodo
				// Aggiungo il valore alla variabile d'istanza del nodo
				if(word != lastwordinline){
					n.name += word.Trim('"');
					n.name += " ";
					continue;
				}
				else{
					n.name += word.Trim('"');
					stage = 1; // Mi aspetto altri attributi, se ci sono
					break; // Passo alla linea successiva
				}
				
			}
			if (word == "status" && stage == 1 && currentobject == 1){
				// Se sono alla riga status, sto aspettando un attributo e sono un nodo
				stage = 6;
				continue;
			}

			if (word == "house-birth" && stage == 1 && currentobject == 1){
				// Se sono alla riga family, sto aspettando un attributo e sono un nodo
				stage = 7;
				continue;
			}

			if (stage == 6){
				if (word == "\"Alive\""){
					var nodeRenderer = n.GetComponent<Renderer>();
					nodeRenderer.material.color = Color.green;
				}
				if (word == "\"Deceased\""){
					var nodeRenderer = n.GetComponent<Renderer>();
					nodeRenderer.material.color = Color.red;
				}
				if (word == "\"Uncertain\""){
					var nodeRenderer = n.GetComponent<Renderer>();
					nodeRenderer.material.color = Color.gray;
				}
				stage = 1;
				break;
			}
			
			if (stage == 7){
				// Sto assegnando un colore per la famiglia
				if(word == "\"House"){
					continue;
				}
				else{
					if(word == "Stark\""){
						n.familycolor = colorList[0];
					}
					if(word == "Arryn\""){
						n.familycolor = colorList[1];
					}
					if(word == "Baelish\""){
						n.familycolor = colorList[2];
					}
					if(word == "Baratheon\""){
						n.familycolor = colorList[3];
					}
					if(word == "Bolton\""){
						n.familycolor = colorList[4];
					}
					if(word == "Clegane\""){
						n.familycolor = colorList[5];
					}
					if(word == "Dayne\""){
						n.familycolor = colorList[6];
					}
					if(word == "Dondarrion\""){
						n.familycolor = colorList[7];
					}
					if(word == "Frey\""){
						n.familycolor = colorList[8];
					}
					if(word == "Greyjoy\""){
						n.familycolor = colorList[9];
					}
					if(word == "Lannister\""){
						n.familycolor = colorList[10];
					}
					if(word == "Martell\""){
						n.familycolor = colorList[11];
					}
					if(word == "Mormont\""){
						n.familycolor = colorList[12];
					}
					if(word == "Payne\""){
						n.familycolor = colorList[13];
					}
					if(word == "Redwyne\""){
						n.familycolor = colorList[14];
					}
					if(word == "Reed\""){
						n.familycolor = colorList[15];
					}
					if(word == "Targaryen\""){
						n.familycolor = colorList[16];
					}
					if(word == "Tarth\""){
						n.familycolor = colorList[17];
					}
					if(word == "Thorne\""){
						n.familycolor = colorList[18];
					}
					if(word == "Tully\""){
						n.familycolor = colorList[19];
					}
					if(word == "Tyrell\""){
						n.familycolor = colorList[20];
					}
					if(word == "\"Sand"){
						n.familycolor = colorList[21];
					}
					stage = 1; // Mi aspetto altri attributi, se ci sono
					break; // Passo alla linea successiva
				}
				
			}

			if (word == "source" && stage == 1 && currentobject == 2){
				// Sono in un arco, sto aspettando un attributo
				stage = 4; // La prossima info sarà il numero del nodo di partenza
				continue;
			}
			if (word == "target" && stage == 1 && currentobject == 2){
				stage = 5;
				continue;
			}
			if (word == "directed" && stage == 1 && currentobject == 2){
				stage = 11;
				continue;
			}
			if (word == "relation" && stage == 1 && currentobject == 2)
			{
				stage = 10;
				continue;
			}
			if (stage == 4){
				n = nodes[word]; // Aggiungo il nodo di partenza e scendo di linea
				stage = 1;
				break;
			}
			if (stage == 5){
				lasttarget = word;
				stage = 1;
				break;
			}
			if (stage == 10){
				currentreleation = word.Trim('"');
				stage = 1;
				break;
			}
			if (stage == 11){
				// se è directed allora metto true
				if (word == "0"){
					n.AddEdge(nodes[lasttarget], false, currentreleation);
				}
				else{
					n.AddEdge(nodes[lasttarget], true, currentreleation);
				}
				stage = 1;
				break;
			}
		}
      }
	  Node starting_node = nodes["0"].GetComponent<Node>();
	  Rigidbody _rigidbody = starting_node.GetComponent<Rigidbody>();
	  _rigidbody.velocity = Vector3.zero;
	  _rigidbody.isKinematic = true;
    }
    
}
