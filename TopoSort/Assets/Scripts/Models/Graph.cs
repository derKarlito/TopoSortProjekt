﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models
{
    public class Graph
    {
        public int Length => Nodes.Count;       //Makes it so that length always points to the count of the current list of Nodes
        public List<Node> Nodes { get; set; }

        public Graph(List<Node> nodes)        //Graph can NOT have Name ONLY contain Nodes
        {
            Nodes = nodes;
        }

        public Graph()                                    //Graph can be empty
        {
            Nodes = new List<Node>();                     //But still needs to have a new, empty list to it
        }
        
        public void AddNodes(List<Node> toAdd)        //Adds List of Nodes to Graph
        {
            foreach (var i in toAdd)
            {
                Nodes.Append(i);
            }
        }
        
        public void AddNode(Node toAdd)            //Adds Node to Graph
        {
            Nodes.Add(toAdd);
        }

        public void RmvNode(Node trem)                //Removes Node from Graph
        {
            foreach(Node node in Nodes){
                if(node.Descendants.Contains(trem)){
                    node.Descendants.Remove(trem);
                }
            }
            Nodes.Remove(trem);
        }

        public string toString()
        {
            string text = "";
            for(int i = 1; i <= Length; i++)
            {
                foreach (Node node in Nodes)
                {
                    if(node.position == i)
                    {
                        text += node.Name+" + ";
                    }
                }
            }
            var result = text.Remove(text.Length-3);
            return result;
        }
    }
}
