using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentNode : NewNode
{
    string currentname;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetCurrentName(string name){
        this.currentname = name;
    }
    public string GetCurrentName(){
        return this.currentname;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
