using System;
using System.Collections.Generic;
namespace Models {
    
    public class Node{
        
        public string Name{get; private set;}
        
        public List<Node> Dependencies{get; private set;}
        
        public Node(string name, List<Node> dependencies){
            Name = name;
            Dependencies = dependencies;
        }

        public Node(string name){
            Name = name;
        }

        public void addDependency(Node dependency){
            Dependencies.Add(dependency);
        }
        
    }
    
}