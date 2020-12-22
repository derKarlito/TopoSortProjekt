using System;
using System.Collections.Generic;
namespace Models {
    
    public class Node{
        
        public string Name{get; private set;}
        public List<Node> Descendants = new List<Node>(); //immediate children of the node
        public List<Node> Ancestors = new List<Node>();   //Nodes need to be able to know what came before them for some impacts on the planet
        public int InDegree = 0; //Number of incomming edges default is no ancestors
        
        public Node(string name, List<Node> descendants){
            Name = name;
            Descendants = descendants;
        }

        public Node(string name){
            Name = name;
        }

        public void addDescendant(Node descendant){
            Descendants.Add(descendant);
            descendant.Ancestors.Add(this);
            descendant.InDegree++;
        }
        
        public void addAncestor(Node ancestor){
            Ancestors.Add(ancestor);
            ancestor.Descendants.Add(this);
            InDegree++;
        }
    }
}