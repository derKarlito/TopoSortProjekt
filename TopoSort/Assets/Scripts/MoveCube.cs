using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TopoSort{

 public class MoveCube : MonoBehaviour {
 
 
     public GameObject Cube; //The Cube
 
     // Update is called once per frame
     void Start()
     {
         Debug.Log("Iam alive");
         
         Algorithm test = new Algorithm(mockNodes());
     }
     void Update () {
        
         
 
     }
     public List<Node> mockNodes(){
         List<Node> returnList = new List<Node>();
         Node a = new Node("a");
         Node b = new Node("b");
         Node c = new Node("c", a);
         Node d = new Node("d",b);
         Node e = new Node("e",b,c);
         returnList.Add(a);
         returnList.Add(b);
         returnList.Add(c);
         returnList.Add(d);
         returnList.Add(e);
         return returnList;
   }
 }
}