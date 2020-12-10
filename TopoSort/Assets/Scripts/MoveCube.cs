using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

//TODO: look at Start()
//it's too late
namespace TopoSort{

 public class MoveCube : MonoBehaviour {
 
 
     public GameObject Cube; //The Cube
 
     // Update is called once per frame
     void Start()
     {
         Debug.Log("Iam alive");

        // Graph g1 = new Graph(mockNodes());
         
         Algorithm test = new Algorithm(mockNodes());
     }
     void Update () {
        
         
 
     }
     public List<Node> mockNodes(){
         List<Node> returnList = new List<Node>();
         Node a = new Node("a");
         Node b = new Node("b");
         Node d = new Node("d");
         d.addDependency(b);
         Node e = new Node("e");
         e.addDependency(b);
         e.addDependency(d);
         returnList.Add(a);
         returnList.Add(b);
         returnList.Add(d);
         returnList.Add(e);
         return returnList;
   }
 }
}