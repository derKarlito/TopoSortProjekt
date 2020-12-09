using System;
namespace Models {
    
    public class Node{
        
        public string Name{get; private set;}
        
        public Node[] Dependencies{get; private set;}
        
        public Node(string name, params Node[] dependencies){
            Name = name;
            Dependencies = dependencies;
        }

        
    }
    
}