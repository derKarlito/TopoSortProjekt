using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Models;
using TopoSort;



namespace TopoSort
{
    public class Algorithm : MonoBehaviour
    {

        private Collider2D Collider;
        private Collider2D ForwardCollider;
        private Stack StepStack;
        public Queue<Node> SourceQueue;
        public List<Node> SortedNodes;
        private bool Prepared;
        public Dictionary<Node, int> InDegrees;

        // Start is called before the first frame update
        void Start()
        {
            Collider = this.gameObject.transform.Find("Play").GetComponent<Collider2D>(); // To precisely pick the collider of the Play button
            ForwardCollider = this.gameObject.transform.Find("Forward").GetComponent<Collider2D>();
            StepStack = new Stack();
            SourceQueue = new Queue<Node>();
            SortedNodes = new List<Node>();
            Prepared = false;
            InDegrees = new Dictionary<Node, int>();

        }


        void Update()
        {
            bool onButton = MouseManager.MouseHover(Collider);
            if(Input.GetMouseButtonDown(0) && onButton)
            {
                Debug.Log("Algorithm clicked");
                AlgorithmSetup(GraphManager.graph);
            }

            onButton = MouseManager.MouseHover(ForwardCollider);
            if (Input.GetMouseButtonDown(0) && onButton)
            {
                Debug.Log("Step forward");
              
                if (!Prepared)
                {
                    PrepareAlgorithm(GraphManager.graph);
                    Prepared = true;
                }
                StepForward();
            }
        }

        void PrepareAlgorithm(Graph graph)
        {
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                Node n = graph.Nodes[i];                
                InDegrees.Add(n, 0);
            }

            for (int i = 0; i < graph.Nodes.Count; i++) {
                Node n = graph.Nodes[i];
                for (int k = 0; k < n.Descendants.Count; k++)
                {
                    InDegrees[n.Descendants[k]]++;
                }
            }
            
            Queue<Node> sources = FindSources(graph);
            for (int i = 0; i < sources.Count; i++) {
                SourceQueue.Enqueue(sources.Dequeue());
            }
            Debug.Log("Algorithm prepared!");
            
        }

        void StepForward()
        {
            AlgorithmState state = new AlgorithmState();
            Node current = SourceQueue.Dequeue();
            SortedNodes.Add(current);
            Debug.Log("Current Node added: " + current.Id);

            for (int i = 0; i < current.Descendants.Count; i++)
            {
                Node des = current.Descendants[i];
                int deg = --InDegrees[des];
                if (deg == 0)
                {
                    state.NewSources.Add(des);
                    SourceQueue.Enqueue(des);
                }
                

            }

            state.Current = current;
            StepStack.Push(state);
        }

        static Queue<Node> FindSources(Graph graph)
        {
            Queue<Node> result = new Queue<Node>();

            int[] inDegree = new int[graph.Length];
            foreach (Node n in graph.Nodes)
            {

                foreach (Node d in n.Descendants)
                {
                    int i = graph.Nodes.IndexOf(d);
                    inDegree[i]++;
                }

            }

            for (int i = 0; i < inDegree.Length; i++)
            {
                if (inDegree[i] == 0)
                {
                    result.Enqueue(graph.Nodes[i]);
                }
            }

            return result;
        }

        public void AlgorithmSetup(Graph input)
        {
            Debug.Log("UNSORTED:");
            WriteGraph(input);                              //writing unsorted graph first for clarity's sake in testing
            StartTopoSort(input);
            
        }
        ///<summary>
        /// Main-Function.
        /// Takes the directed graph (list with Nodes)
        /// iterates through every Node
        /// and returns a sorted graph.
        ///</summary>
        public void StartTopoSort(Graph input)
        {
           // CheckForCycles(input); //Checks if graph has cycles, throws argument if so
            
            ExecuteTopoSort(input);
            
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

        IEnumerator SortCoroutine(Queue<Node> q, List<Node> sorted, Graph input)
        {


            AlignmentUtil.finished = false;

            while (q.Count != 0)
            {
                Node Element = q.Dequeue();  //remove the first Node of the queue
                AlgorithmManager.StartFeed(Element);
                
                Debug.Log("Waiting...");
                yield return new WaitForSeconds(2f);
                sorted.Add(Element);   //insert that Node in the sorted list
                foreach(Node Descendant in Element.Descendants)     //removes the node and its edgeds from the graph 
                {
                    Descendant.InDegree -= 1;
                    if (Descendant.InDegree == 0)   //if thus a new node with 0 incomming edges is created, it is added to the queue
                    {
                        q.Enqueue(Descendant);
                    }
                }
                AlgorithmManager.ExitFeed(Element);
            }

            if(sorted.Count != input.Length) //happens when there's cycles
            {
                Debug.Log("Smh Graph contains cycle");
                Debug.Log("Probably one of these nodes...");
                //THIS ONLY WORKS WHEN THERE'S ONE SINGLE CYLCE IN THE GRAPH
                //Otherwise it causes a stack overflow :(
                List<Node> nodesInCycle = FindCycleNodes(input);
                foreach(Node node in nodesInCycle)
                {
                    Debug.Log(node.Id);
                }
               
            }

            AlignmentUtil alignment = new AlignmentUtil();
            alignment.sorted = sorted;
            alignment.SortNotesInArrs();
        }

        public void ExecuteTopoSort(Graph input)
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

            StartCoroutine(SortCoroutine(q, sorted, input));

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

        
        private static List<Node> FindCycleNodes(Graph input)
        {
            //We can determine which nodes are in a cycle by checking every ancestry of every node to see if it contains the node we are looking at :^)
            //I'm sure this is very runtime-efficient :^)
            List<Node> cycleNodes = new List<Node>();

            for(int i = 0; i < input.Nodes.Count; i++) //for each node
            {
                InspectAncestry(input.Nodes[i], input.Nodes[i].Ancestors, cycleNodes);
            }
            return cycleNodes;
        }
        private static List<Node> InspectAncestry(Node input, List<Node> ancestors, List<Node> result)
        {
            if(input.Ancestors.Count != 0) //if there are ancestors at all
            {
                for(int j = 0; j < ancestors.Count; j++) //for each node in the ancestor list
                { 
                    if(input.Equals(ancestors[j])) //wenn die root node (i) gleich einer ihrer ancestors (j) ist
                    {
                        // node input is part of cycle
                        if(!result.Contains(input))
                        {
                            result.Add(input);
                            return result;
                        }
                    }  
                    else 
                        InspectAncestry(input, ancestors[j].Ancestors, result); //ancestors of j are also ancestors of input
                }
            }
            return null;   
        }
        
        private void WriteGraph(Graph input)
        {
            if(input == null)
                Debug.Log("Invalid Graph");
            foreach(Node node in input.Nodes)
            {
                Debug.Log("Node " + node.Id +" Has " + node.Descendants.Count + " descendants." );
        
                foreach(Node n in node.Descendants)
                {
                    Debug.Log(n.Id);
                    
                }
            }
        }  
        
    }  
}
