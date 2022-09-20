using System.Collections.Generic;
using UnityEngine;
public class ParsingEdge
    {
        string from_id;
        string to_id;
        bool directed;
        string relationship;
        string typeofrelationship;

        public void SetFromID(string id){
            this.from_id = id;
        }
        public void SetToID(string id){
            this.to_id = id;
        }
        public void SetDirected(bool directed){
            this.directed = directed;
        }
        public void SetRelationship(string relationship){
            this.relationship = relationship;
        }
        public void SetTypeRelationship(string type){
            this.typeofrelationship = type;
        }
        public string GetFromID(){
            return this.from_id;
        }
        public string GetToID(){
            return this.to_id;
        }
        public bool IsDirected(){
            return this.directed;
        }
        public string GetRelationship(){
            return this.relationship;
        }
        public string GetTypeRelationship(){
            return this.typeofrelationship;
        }
    }