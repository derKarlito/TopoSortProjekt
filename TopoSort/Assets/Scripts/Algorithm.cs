using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Models;
using TopoSort;


namespace TopoSort{
    public class Algorithm 
    {
        public Algorithm(Graph input)
        {
            WriteGraph(input);                              //writing unsorted graph first for clarity's sake in testing
            List<Node> sortedList = StartTopoSort(input);

        }
        
        
        // Main-Function.
        // Takes the directed graph (list with Nodes)
        // iterates through every Node
        // and returns a sorted graph.
        
        public static List<Node> StartTopoSort(Graph input)
        {
            List<Node> origins = new List<Node>();              // Nodes with no ancestors
            var sorted = new List<Node>();                      //Output list after sorting
            var visited = new Dictionary<Node, bool>();         //Dictionary: Node; false(already visited) | true(temporary visited flag)

            foreach(Node node in input.Nodes)
            {

                if(node.Ancestors.Count == 0){  //get a list of all "starting" nodes
                    origins.Add(node);
                }  

                if(isCycleless(node, visited) == false){
                     throw new ArgumentException("Cyclic dependency found."); 
                }

                sorted.Add(node);
            }
            
            return sorted;
        }

        // Takes a node
        // Checks for cyclic dependencies

        /*  changelog 10.12.
        *   If the method only checks if there's a cycle in the graph. Why is it returning a list of nodes?
        *   The nodes are not sorted and duplicates are added into the returned List
        *   We do not want that
        *   Solution make it return a bool let the main method handle the Exception (for prettiness)
        */

        private static bool isCycleless(Node node, Dictionary<Node, bool> visited)
        {
            visited.TryGetValue(node, out var working);   //Assigning 'working' true if (descendant) node was already visited

            if (working)
            {
               return false;   //If node was already visited -> throw Exception for Cyclic dependency
            }
            else
            {
                visited[node] = true;                        //Set flag for current node to true temporarily
                var descendants = node.Descendants;
                
                if (descendants != null)
                {
                    foreach (var descendant in descendants)                    //For all descendants of a node do:
                    {
                        isCycleless(descendant, visited);                     //Visit node and check for descendants
                    }
                    
                }
                visited[node] = false;
            }

            return true; 
        }


        private void WriteGraph(Graph input){

            if(input == null)
                Debug.Log("Invalid Graph");

         foreach(Node node in input.Nodes)
            {
            Debug.Log("Node " + node.Name +" Has " + node.Descendants.Count + " descendants." );
        
               foreach(Node n in node.Descendants){
                   Debug.Log(n.Name);
               }
            }

    }
    }
    
}