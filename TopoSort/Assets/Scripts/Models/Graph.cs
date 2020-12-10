using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO: The Constructors should accept Lists, not Arrays
//++the method
//(++ the class property Nodes)(?)
//Why is this red in my IDE??
namespace Models
{
    public class Graph
    {
        public string Name { get; private set; }
        
        public List<Node> Nodes { get; private set; }
        
        public Graph(string name, List<Node> nodes)        //Graph can have a Name and contains Nodes
        {
            Name = name;
            //var glength = nodes.Count;
            Nodes = nodes;
        }

        public Graph(List<Node> nodes)
        {
            Nodes = nodes;
        }
        
        public void AddNodes(List<Node> toAdd)
        {
            foreach (var i in toAdd)                    //Cycles through all "to-add" Nodes and adds them to the Graph
            {
                Nodes.Append(i);
            }

            return;
        }
        
    }

}
