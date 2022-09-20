using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    Outline _nodeoutline;
    void Awake()
    {
        _nodeoutline = transform.gameObject.GetComponent<Outline>();
        _nodeoutline.enabled = false;
    }

    public void EnableConnectedOutline()
    {
        List<NewNode> connectednodes = transform.GetComponent<NewNode>().GetConnectedNodes();
        foreach (NewNode node in connectednodes)
        {
            node.GetComponent<Outline>().enabled = true;
        }
        List<NewNode> outgoingnodes = transform.GetComponent<NewNode>().GetOutgoingNodes();
        foreach (NewNode node in outgoingnodes){
            node.GetComponent<Outline>().enabled = true;
        }
        List<NewNode> incomingnodes = transform.GetComponent<NewNode>().GetIncomingNodes();
        foreach (NewNode node in incomingnodes){
            node.GetComponent<Outline>().enabled = true;
        }
    }

    public void DisableConnectedOutline()
    {
        List<NewNode> connectednodes = transform.GetComponent<NewNode>().GetConnectedNodes();
        foreach (NewNode node in connectednodes)
        {
            node.GetComponent<Outline>().enabled = false;
        }
        List<NewNode> outgoingnodes = transform.GetComponent<NewNode>().GetOutgoingNodes();
        foreach (NewNode node in outgoingnodes){
            node.GetComponent<Outline>().enabled = false;
        }
        List<NewNode> incomingnodes = transform.GetComponent<NewNode>().GetIncomingNodes();
        foreach (NewNode node in incomingnodes){
            node.GetComponent<Outline>().enabled = false;
        }
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
