using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace TopoSort{

 public class MoveCube : MonoBehaviour {

     public GameObject Cube; //The Cube
 
     // Update is called once per frame
     void Start()
     {
         Debug.Log("I'm alive");

         Graph g1 = new Graph(mockNodes());
         
         Algorithm test = new Algorithm(g1);
     }
     void Update () {
        
         
 
     }
     public List<Node> mockNodes(){
         List<Node> returnList = new List<Node>();
         Node a = new Node("a");
         Node b = new Node("b");
         Node d = new Node("d");
         Node c = new Node("c");
         Node e = new Node("e");
         e.addDescendant(a);
         e.addDescendant(b);
         e.addDescendant(d);
         c.addAncestor(b);
         c.addAncestor(d);
         b.addDescendant(d);
         returnList.Add(a);
         returnList.Add(b);
         returnList.Add(d);
         returnList.Add(c);
         returnList.Add(e);
         return returnList;
   }
 }
}