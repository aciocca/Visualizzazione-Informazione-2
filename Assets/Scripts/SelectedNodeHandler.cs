using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedNodeHandler : MonoBehaviour
{
    public void SetAsCurrentNode()
    {
        NewNode selectednode = gameObject.GetComponent<NewNode>();
        CurrentNode copynode = GameObject.Find("Ciao").GetComponent<CurrentNode>();
        copynode.SetCurrentName(selectednode.GetName());
/*         copynode.SetHousebirth(selectednode.GetHouseBirth());
        copynode.SetGroup(selectednode.GetGroup());
        copynode.SetStatus(selectednode.GetStatus());
        copynode.SetMarriage(selectednode.GetHouseMarriage());
        copynode.SetColor(selectednode.GetColor()); */
        copynode.SetOutgoingNodes(selectednode.GetOutgoingNodes());
        copynode.SetIncomingNodes(selectednode.GetIncomingNodes());
        copynode.SetConnectedNodes(selectednode.GetConnectedNodes());

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
