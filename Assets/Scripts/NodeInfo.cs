using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeInfo : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private CurrentNode cnode;
    // Start is called before the first frame update
    void Start()
    {
        cnode = GameObject.Find("Ciao").GetComponent<CurrentNode>();
        _text = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    public void ResetCounter()
    {
        _text.text = "RESET";
    }
    
    public string FindRelationship(CurrentNode selected, NewNode target)
    {
        string status = "";
        string selectedname = selected.GetCurrentName();

        if(target.childrens.ContainsKey(selectedname))
        {
            status += "[Genitore " + target.childrens[selectedname] + "]";
        }
        if(target.killerof.Contains(selectedname))
        {
            status +=  "[Killer]";
        }
        if(target.siblings.ContainsKey(selectedname))
        {
            status +=  "[Fratello o Sorella " + target.siblings[selectedname] + "]";
        }
        if(target.mother.ContainsKey(selectedname))
        {
            status +=  "[Figlio/a " + target.mother[selectedname] + "]";
        }
        if(target.father.ContainsKey(selectedname))
        {
            status +=  "[Figlio/a " + target.father[selectedname] + "]";
        }
        if(target.killedby.Contains(selectedname))
        {
            status +=  "[Vittima]";
        }
        if(target.lover.Contains(selectedname))
        {
            status +=  "[Amante]";
        }
        if(target.spouse.Contains(selectedname))
        {
            status +=  "[Sposa/o]";
        }
        if(target.allegiance.ContainsKey(selectedname))
        {
            status +=  "[Alleato {" + target.allegiance[selectedname] + "}]";
        }
        if(status == "")
        {
            status +=  "Relationship not found";
        }
        return status;
    }
    public void UpdateText()
    {   
        string newtext = $"<b>Name</b>: <color=\"white\">{cnode.GetCurrentName()}</color>\n<b>Connected nodes:</b>\n";
        foreach (NewNode node in cnode.GetConnectedNodes())
        {
            newtext += "<color=" + node.GetHexHouseColor() + ">" + node.name + "</color>" + " <color=\"orange\">(" + FindRelationship(cnode, node) + ")</color>\n";
        }
        newtext += "</color><b>Outgoing:</b>\n<color=\"green\">";
        foreach (NewNode node in cnode.GetOutgoingNodes())
        {
            newtext += "<color=" + node.GetHexHouseColor() + ">" + node.name + "</color>" + "<color=\"green\"> (" + FindRelationship(cnode, node) + ")</color>\n";
        }
        newtext += "</color><b>Incoming:</b>\n<color=\"black\">";
        foreach (NewNode node in cnode.GetIncomingNodes())
        {
            newtext += "<color=" + node.GetHexHouseColor() + ">" + node.name + "</color>" + "<color=\"black\"> (" + FindRelationship(cnode, node) + ")</color>\n";
        }
        newtext += "</color>";
        _text.text = newtext;
    }

    void Update(){
        
        if(_text.text != cnode.GetCurrentName() && _text.text != "RESET")
        {
            UpdateText();
        }
    }

}
