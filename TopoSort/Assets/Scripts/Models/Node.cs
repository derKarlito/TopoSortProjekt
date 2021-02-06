using System;
using System.Collections.Generic;
using TopoSort;
using Newtonsoft.Json;
namespace Models {
    public class Node{
        
        public string Name{get; set;} //equal to value of Nodetype i.e. Water-Node/Atmosphere-Node/etc
        public int Id;
        public int position; //Position in Graph. Important for the Node to know, bc of how planets are created
        [JsonIgnore]
        public List<Node> Descendants = new List<Node>(); //immediate children of the node
        [JsonIgnore]
        public List<Node> Ancestors = new List<Node>();   //Nodes need to be able to know what came before them for some impacts on the planet
        public int InDegree = 0; //Number of incomming edges default is no ancestors
        public int SimulatedInDegree = 0;
        
        public Node()
        {}
        
        public Node(string name, List<Node> descendants){
            Name = name;
            Descendants = descendants;
        }

        public Node(int id)
        {
            Id = id;
        }

        public Node(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public void addDescendant(Node descendant)
        {            
            Descendants.Add(descendant);
            descendant.Ancestors.Add(this);
            descendant.InDegree++;
        }

        public void rmvDescendants(Node descendant)
        {
            Descendants.Remove(descendant);
            descendant.Ancestors.Remove(this);
            descendant.InDegree--;
        }
        
        public void rmvDescendants(List<Node> descendant)
        {
            //int length = descendant.Count;
            for(int i = 0 ; i < descendant.Count; i++)             //This used to be a foreach loop, which at its base, is valid, however
            {                                               //when a foreach loop iterates over a list which ('s size) is going to be modifed (like the removal of an element)
                descendant[i].Ancestors.Remove(this);       //then it can't execute properly. That's why we quickly take the initial length of the list and then use that in a for loop
                descendant[i].InDegree -= 1; 
                if(Algorithm.InDegrees.ContainsKey(descendant[i]))                  //removing each element from the end to the beginning
                    Algorithm.InDegrees[descendant[i]]--;
                Algorithm.InDegrees.Remove(this);
                descendant.RemoveAt(i);                     //I'm unsure if it can be done the other way around but this way seems more logical and also using a for loop where the index decreases each time is always a nice flex
            }
            
        }

        public void rmvAncestors(List<Node> ancestors)
        {
            for(int i = 0 ; i < ancestors.Count; i++)             //This used to be a foreach loop, which at its base, is valid, however
            {                                               //when a foreach loop iterates over a list which ('s size) is going to be modifed (like the removal of an element)
                ancestors[i].Descendants.Remove(this);       //then it can't execute properly. That's why we quickly take the initial length of the list and then use that in a for loop                  //removing each element from the end to the beginning
                ancestors.RemoveAt(i);                     //I'm unsure if it can be done the other way around but this way seems more logical and also using a for loop where the index decreases each time is always a nice flex
            }
        }

        public bool Equals(Node eNode)
        {
            return (Id == eNode.Id);
        }
    }
}
