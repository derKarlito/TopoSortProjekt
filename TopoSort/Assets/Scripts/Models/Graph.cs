using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models
{
    public class Graph
    {
        public string Name { get; private set; }
        public int Length => Nodes.Count;       //Makes it so that length always points to the count of the current list of Nodes
        public List<Node> Nodes { get; private set; }
        public Graph(string name, List<Node> nodes)        //Graph can have a Name and contains Nodes
        {
            Name = name;
            Nodes = nodes;
        }

        public Graph()                                    //Graph can be empty
        {
            
        }

        public Graph(List<Node> nodes)
        {
            Nodes = nodes;
        }
        
        public void AddNodes(List<Node> toAdd)        //Adds List of Nodes to Graph
        {
            foreach (var i in toAdd)
            {
                Nodes.Append(i);
            }
        }
        
        public void AddNodes(Node toAdd)            //Adds Node to Graph
        {
            Nodes.Append(toAdd);
        }

        public void RmvNode(Node trem)                //Removes Node from Graph
        {
            Nodes.Remove(trem);
        }
    }
}
