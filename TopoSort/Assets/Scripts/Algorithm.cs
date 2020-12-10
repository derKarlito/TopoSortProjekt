using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Models;
using TopoSort;


// TODO: Constructor
namespace TopoSort{
    public class Algorithm 
    {
        public Algorithm(List<Node> input)
        {
            StartTopoSort(input);
        }
        
        
        // Main-Function.
        // Takes the directed graph (list with Nodes)
        // iterates through every Node
        // and returns a sorted graph.
        
        public static List<Node> StartTopoSort(List<Node> input)
        {
            var sorted = new List<Node>();                      //Output list after sorting
            var visited = new Dictionary<Node, bool>();         //Dictionary: Node; false(already visited) | true(temporary visited flag)
            
            foreach(Node node in input)
            {
                Debug.Log(node.Name);
                Debug.Log(node.Dependencies.Count); 
                NodeResolve(node, sorted, visited);
            }
            
            return sorted;
        }
        
        
        // Takes a node
        // Checks for cyclic dependencies
        // 
        private static List<Node> NodeResolve(Node node, List<Node> sorted, Dictionary<Node, bool> visited)
        {
            visited.TryGetValue(node, out var working);   //Assigning 'working' true if (dependency) node was already visited

            if (working)
            {
                throw new ArgumentException("Cyclic dependency found.");    //If node was already visited -> throw Exception for Cyclic dependency
            }
            else
            {
                visited[node] = true;                        //Set flag for current node to true temporarily
                var dependencies = node.Dependencies;
                
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)                    //For all dependencies of a node do:
                    {
                        NodeResolve(dependency, sorted, visited);                     //Visit node and check for dependencies
                    }
                    
                }

                visited[node] = false;
                sorted.Add(node);
            }

            return sorted;
        }

    }
    
}