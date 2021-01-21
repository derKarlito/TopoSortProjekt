﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        private Collider2D BackwardCollider;

        public TextMeshPro QueueText;
        public Planet Planet;           //gameobject to then get the script from

        public LinkedList<Node> SourceQueue;                // contains all sources that should be processed next
        public LinkedList<Node> SortedNodes;                // contains all the nodes sorted so far
        public static Dictionary<Node, int> InDegrees;             // holds the in degrees of all nodes
        public AlgorithmState CurrentState;                 // holds the state, that's active
        private Stack<AlgorithmState> StepStack;            // stores all the steps done

        // Flags
        public bool Automatic;         // steps in a fixed interval specified by MAX_STEP_TIME    
        public bool Prepared;          // is set to true, when the Graph and all needed Objects are prepared
        public bool Changed;           // is set to true, when there were a state change
        public bool Finished;          // is set to true, when the algorithm finishes successful
        public bool Failed;            // is set to true, when the algorithm fails

        public float DeltaStep;                            // holds the time past since the last step
        public static float MAX_STEP_TIME = 1.0f;           // specifies the time between each step (seconds)

        public bool Testing; //Enables extra stats for testing. Only changeable from Editor

        // Start is called before the first frame update
        void Start()
        {
            Collider = this.gameObject.transform.Find("Play").GetComponent<Collider2D>(); // To precisely pick the collider of the Play button
            ForwardCollider = this.gameObject.transform.Find("Forward").GetComponent<Collider2D>();
            BackwardCollider = this.gameObject.transform.Find("Backward").GetComponent<Collider2D>();

            StepStack = new Stack<AlgorithmState>();
            SourceQueue = new LinkedList<Node>();
            SortedNodes = new LinkedList<Node>();
            InDegrees = new Dictionary<Node, int>();

            ResetGraph();

        }


        void Update()
        {
            Changed = false;            

            CheckAlgorithmControls();               // checks colliders

            if (Automatic)                             
            {
                DeltaStep += Time.deltaTime;

                if (DeltaStep >= MAX_STEP_TIME)     
                {
                    DeltaStep = 0.0f;               // timer reset
                    StepForward();                  // automatic step
                }
            }

            if (Changed)                            
            {

                if (StepStack.Count > 0)
                {
                    this.CurrentState = StepStack.Peek();   
                }

                AlgorithmManager.ColourGraph(this);     // colours the graph accordingly to the change
            }
        }


        /*
         *  checks the colliders and reacts to them 
         */
        void CheckAlgorithmControls()
        {
            // Play/Pause Button
            bool onButton = MouseManager.MouseHover(Collider);
            if (Input.GetMouseButtonDown(0) && onButton)
            {

                Automatic = Automatic ? false : true;               // switches between true and false
                if (!Prepared)
                {
                    SoundManagerScript.PlaySound("playButton");
                    PrepareAlgorithm(GraphManager.graph);           // Prepares the algorithm in case it is not already
                }

            }

            // Forward Button
            onButton = MouseManager.MouseHover(ForwardCollider);
            if (Input.GetMouseButtonDown(0) && onButton)
            {
                SoundManagerScript.PlaySound("forthButton");

                if (!Prepared)
                {
                    PrepareAlgorithm(GraphManager.graph);           
                }
                else if (!Finished)
                {
                    if (Automatic)                                  // pauses the automatic stepping and avoids stepping one step further
                    {
                        Automatic = false;
                    }
                    else
                    {
                        StepForward();
                    }
                }

            }

            // Backward Button
            onButton = MouseManager.MouseHover(BackwardCollider);
            if (Input.GetMouseButtonDown(0) && onButton)
            { 
                SoundManagerScript.PlaySound("backButton");
                if ((Prepared && !Finished) && StepStack.Count > 0)     
                {
                    if (Automatic)                                  
                    {
                        Automatic = false;
                    }
                    else
                    {
                        StepBackward();
                    }
                }

            }

            // Reset-Key / Stop
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Reset");
                ResetGraph();
            }
        }


        /*
         * checks if the Algorithm reached an end state and sets the flags
         */
        void CheckFinished()
        {
            if (SortedNodes.Count > 0 && SourceQueue.Count == 0 && !(Finished || Failed))
            {
                if (SortedNodes.Count == GraphManager.graph.Nodes.Count)    // all nodes are sorted   
                {
                    Finished = true;

                    List<Node> sorted = new List<Node>();                   

                    foreach (Node n in SortedNodes)                         // preparing sorted nodes for alignment util
                    {
                        sorted.Add(n);
                    }

                    AlignmentUtil alignment = new AlignmentUtil();
                    alignment.sorted = sorted;
                    alignment.ImprovedGraphVisualisation();

                    Debug.Log("Algorithmus erfolgreich beendet");
                    Planet.PlanetReset();
                }
                else                                                        // there are unsorted nodes
                {
                    Failed = true;
                    Debug.Log("Graph enthält einen Zyklus oder einen isolierten Knoten");
                }

                Automatic = false;
            }
        }


        /*
         *  fills source queue and builds the in degree table
         */
        void PrepareAlgorithm(Graph graph)
        { 

            foreach (Node node in graph.Nodes)                   // initializes the indegree table
            {              
                InDegrees.Add(node, 0);
            }

            foreach (Node node in graph.Nodes)                  // iterates through all nodes
            {       
                foreach (Node desc in node.Descendants)         // iterates through the descendants of a node and increments its indegree
                {
                    InDegrees[desc]++;
                }
            }

            foreach (Node node in graph.Nodes)                  // iterates through the nodes to find sources in the final table
            {
                if (InDegrees[node] == 0)
                {
                    SourceQueue.AddLast(node);                  
                }
            }

            Prepared = true;
            Changed = true;
            Finished = false;
            AlignmentUtil.finished = false;  

            TextUpdate();
        }


        /*
         * Does one single iteration of the algorithm
         */
        public void StepForward()
        {

            if (!Prepared)
            {
                Debug.Log("Algorithmus ist noch nicht vorbereitet.");
                return;
            }

            CheckFinished();                        // checks if the algorithm reached a finished stat
            if(Finished)        //attempt of getting out of NullRefExcep but-- kinda doesnt work
            {
                ResetGraph();
                return;
            }

            if (Finished || Failed)
            {
                Planet.PlanetReset();
                ResetGraph();                                   //Gives another chance at executing the Graph
                PrepareAlgorithm(GraphManager.graph);
            }

            AlgorithmState state = new AlgorithmState();
            Node current = SourceQueue.First.Value;             // gets the next source node in the queue
            Planet.NodeEvaluation(current);
            SourceQueue.RemoveFirst();                          // remove node from the queue
            SortedNodes.AddLast(current);                       // add node to sorted list

            foreach (Node desc in current.Descendants)          // iterate through all descendants of a node
            {
                int deg = --InDegrees[desc];                    // decrement the indegree
                if (deg == 0)                                   // if it's now 0 it has to be a source
                {
                    state.NewSources.Add(desc);
                    SourceQueue.AddLast(desc);
                }
            }


            state.Current = current;
            StepStack.Push(state);                              // push the current state on the stack

            Changed = true;
            TextUpdate();
        }


        /*
         *  Goes back to the last state
         */
        public void StepBackward()
        {
            AlgorithmState state = StepStack.Pop();             // get the current state
            SortedNodes.RemoveLast();                           // remove the last node from the sorted List
            SourceQueue.AddFirst(state.Current);                // put the node of the state at the beginning of the queue
            Planet.RemoveNode(state.Current);

            foreach (Node desc in state.Current.Descendants)    // iterate through all the descendants
            {
                int deg = ++InDegrees[desc];                    // increment their indegree
                if (deg == 1)                                   // if it's now 1 it has been a source before and was added to the source queue
                {
                    SourceQueue.RemoveLast();
                }


            }

            Changed = true;
            TextUpdate();
        }


        /*
         *  Resets and clears everything
         */
        public void ResetGraph()
        {
            Automatic = false;
            Changed = false;
            Prepared = false;
            Finished = false;
            Failed = false;

            this.CurrentState = null;

            SourceQueue.Clear();
            SortedNodes.Clear();
            InDegrees.Clear();
            StepStack.Clear();

            AlignmentUtil.finished = false;
            AlgorithmManager.ColourGraph(this);
        }

        public void TextUpdate()
        {
            string queueText = default;
            string sortedText = default;
            string german = default;
            foreach (Node source in SourceQueue)
            {
                switch (source.Name)
                {
                    case "Water":
                        german = "Wasser";
                        break;
                    case "Ground":
                        german = "Tektonik";
                        break;
                    case "Plants":
                        german = "Pflanzen";
                        break;
                    case "Moon":
                        german = "Mond";
                        break;
                    case "Atmosphere":
                        german = "Atmosphäre";
                        break; 
                    default:
                        german = source.Name;
                        break;
                }
                queueText += german + " | ";
            }
            foreach (Node sorted in SortedNodes)
            {
                switch (sorted.Name)
                {
                    case "Water":
                        german = "Wasser";
                        break;
                    case "Ground":
                        german = "Tektonik";
                        break;
                    case "Plants":
                        german = "Pflanzen";
                        break;
                    case "Moon":
                        german = "Mond";
                        break;
                    case "Atmosphere":
                        german = "Atmosphäre";
                        break; 
                    default:
                        german = sorted.Name;
                        break;
                }
                sortedText += german + " -> ";
            }

            if(Testing)
            {
                string planetStats = "Planet\n[";
                for(int i = 0; i < Planet.CreatedPlanet.Length; i++)
                {
                    if(i != Planet.CreatedPlanet.Length-1)
                        planetStats += Planet.CreatedPlanet[i]+", ";
                    else
                        planetStats += Planet.CreatedPlanet[i]+"]";
                }
                QueueText.text = "Queue:\n"+queueText+"\n-------------"+sortedText+"\n-------------"+planetStats;
            }
            else
                QueueText.text = "Queue:\n"+queueText+"\n-------------"+sortedText;
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
            SortCoroutine(input);
            AlgorithmManager.EmptyList();
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

        IEnumerator SortCoroutine(Graph input)
        {
            Planet.PlanetReset();
            int posInGraph = 0;
            
            List<Node> sorted = new List<Node>(); //List to get the sorted nodes
        
            Queue<Node> q = new Queue<Node>();
            for (int i = 0; i < input.Length; i++)     //fills the queue with all nodes that have no incomming edges/ancestors
            {
                if(input.Nodes[i].InDegree == 0)
                {
                    q.Enqueue(input.Nodes[i]);
                }
            }
            


            AlignmentUtil.finished = false;

            while (q.Count != 0)
            {
                Node Element = q.Dequeue();  //remove the first Node of the queue
                AlgorithmManager.StartFeed(Element);
                
                yield return new WaitForSeconds(1f);
                Element.position = posInGraph;
                Planet.NodeEvaluation(Element); //Visualize thew effect of the node on the planet
                sorted.Add(Element);   //insert that Node in the sorted list
                foreach(Node Descendant in Element.Descendants)     //removes the node and its edgeds from the graph 
                {
                    Descendant.SimulatedInDegree -= 1;
                    if (Descendant.SimulatedInDegree == 0)   //if thus a new node with 0 incomming edges is created, it is added to the queue
                    {
                        q.Enqueue(Descendant);
                    }
                }
                AlgorithmManager.ExitFeed(Element);
                posInGraph++;
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
            alignment.ImprovedGraphVisualisation();
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
            if (input == null)
            {
                Debug.Log("Invalid Graph");
            }
                
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
