using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TopoSort;
namespace TopoSort{
    public class Algorithm 
{
    public Algorithm(List<Node> input){
        foreach(Node node in input){
            Debug.Log(node.name);
            Debug.Log(node.Dependencies.Length); 
       }
    }
    static void Sort(List<Node> source){
        var sorted = new List<Node>();
        var visited = new Dictionary<Node,bool>();
        foreach(Node node in source){

        }
    }
    static void Visit(){
        
    }
}
}
