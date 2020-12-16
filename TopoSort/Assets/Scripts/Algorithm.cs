﻿using System.Collections;
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
            Debug.Log("UNSORTED:");
            WriteGraph(input);                              //writing unsorted graph first for clarity's sake in testing
            Graph sorted = StartTopoSort(input);
            Debug.Log("SORTED:");
            WriteGraph(sorted);
        }
        
        
        // Main-Function.
        // Takes the directed graph (list with Nodes)
        // iterates through every Node
        // and returns a sorted graph.
        
        public static Graph StartTopoSort(Graph input)
        {
            CheckForCycles(input); //Checks if graph has cycles, throws argument if so
            
            List<Node> SortedList = ExecuteTopoSort(input);

            Graph sorted = new Graph(SortedList);
            
            return sorted;
        }

        public static void CheckForCycles(Graph input)
        {
            var visited = new Dictionary<Node, bool>();         //Dictionary: Node; false(already visited) | true(temporary visited flag)
            foreach(Node node in input.Nodes)
            {
                if(!isCycleless(node, visited)){
                     throw new ArgumentException("Cyclic dependency found."); 
                }
            }
        }

        public static List<Node> ExecuteTopoSort(Graph input)
        {
            int n = input.Length; //number of Nodes in graph
            Queue<Node> q = new Queue<Node>();

            for (int i = 0; i < n; i++)     //fills the queue with all nodes that have no incomming edges/ancestors
            {
                if(input.Nodes[i].InDegree == 0)
                {
                    q.Enqueue(input.Nodes[i]);
                }
            }

            List<Node> sorted = new List<Node>();

            while (q.Count != 0)
            {
                Node Element = q.Dequeue();  //remove the first Node of the queue
                sorted.Add(Element);   //insert that Node in the sorted list, then increment index

                foreach(Node Descendant in Element.Descendants)     //removes the node and its edgeds from the graph 
                {
                    Descendant.InDegree -= 1;
                    if (Descendant.InDegree == 0)   //if thus a new node with 0 incomming edges is created, it is added to the queue
                    {
                        q.Enqueue(Descendant);
                    }
                }
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
            visited.TryGetValue(node, out var working);   //Assigning 'working' true if (Descendant) node was already visited

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
                    foreach (var Descendant in descendants)                    //For all descendants of a node do:
                    {
                        isCycleless(Descendant, visited);                     //Visit node and check for descendants
                    }
                    
                }
                visited[node] = false;
            }

            return true; 
        }


        private void WriteGraph(Graph input)
        {

            if(input == null)
                Debug.Log("Invalid Graph");

         foreach(Node node in input.Nodes){
            Debug.Log("Node " + node.Name +" Has " + node.Descendants.Count + " descendants." );
        
               foreach(Node n in node.Descendants){
                   Debug.Log(n.Name);
               }
            }
        }
        
    }
    
}