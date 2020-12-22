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
        
        public void addAncestor(Node ancestor){            //Kind of unnecessary duplicate, since addDescendant already adds an ancestor for target node
            Ancestors.Add(ancestor);
            ancestor.Descendants.Add(this);
            InDegree++;
        }

        public void rmvDescendants(Node descendant)
        {
            Descendants.Remove(descendant);
            descendant.Ancestors.Remove(this);
            descendant.InDegree--;
        }
        
        public void rmvDescendants(List<Node> descendant)
        {
            foreach (Node i in descendant)
            {
                Descendants.Remove(i);
                i.Ancestors.Remove(this);
                i.InDegree--;
            }
            
        }

        public void rmvAncestor(Node ancestor)            //See addAncestor comment
        {
            Ancestors.Remove(ancestor);
            ancestor.Descendants.Remove(this);
            InDegree--;
        }
        public bool Equals(Node eNode)
        {
            return Equals(Name, eNode.Name);
        }
    }
}