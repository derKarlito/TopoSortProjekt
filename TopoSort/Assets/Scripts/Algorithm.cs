using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using Models;
using TopoSort;
using TopoSort.Controller;



namespace TopoSort
{
    public class Algorithm : MonoBehaviour
    {

        private Collider2D Collider;
        private Collider2D ForwardCollider;
        private Collider2D BackwardCollider;
        private Collider2D LastToolTipEnter;

        public PersistanceUtility persistanceUtility;
        public TextMeshPro ToolTip;
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

        public float DeltaStep;                             // holds the time past since the last step
        public static float MAX_STEP_TIME = 1.0f;           // specifies the time between each step (seconds)
        public int PosInGraph;                              // holds the pos of the node in the graph counting upward

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

            PosInGraph = 0;

            TextUpdate();
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
            if(onButton)
            {
                SetToolTip("Play", Collider);
                if (Input.GetMouseButtonDown(0))
                {

                    Automatic = Automatic ? false : true;               // switches between true and false
                    if (!Prepared)
                    {
                        SoundManagerScript.PlaySound("playButton");
                        Planet.RemoveAllMoons();
                        Planet.PlanetReset();
                        PrepareAlgorithm(GraphManager.graph);           // Prepares the algorithm in case it is not already
                    }

                }
            }
            else
            {
                DeleteToolTip(Collider);
            }

            // Forward Button
            onButton = MouseManager.MouseHover(ForwardCollider);
            if(onButton)
            {
                SetToolTip("Step Forward", ForwardCollider);
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
            }
            else
            {
                DeleteToolTip(ForwardCollider);
            }
            // Backward Button
            onButton = MouseManager.MouseHover(BackwardCollider);
            if(onButton)
            {
                SetToolTip("Step Back", BackwardCollider);
                if (Input.GetMouseButtonDown(0))
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
            }
            else
            {
                DeleteToolTip(BackwardCollider);
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

                    persistanceUtility.AddLogEntry(Planet, GraphManager.graph);
                    ArchiveManager.checkResult(Planet.State, Atmosphere.State);

                    Debug.Log("Algorithmus erfolgreich beendet");
                    
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
            InDegrees = new Dictionary<Node, int>();
            foreach (Node node in graph.Nodes)                   // initializes the indegree table
            {              
                InDegrees.Add(node, 0);
            }

            foreach (Node node in graph.Nodes)                  // iterates through all nodes
            {       
                foreach (Node desc in node.Descendants)         // iterates through the descendants of a node and increments its indegree
                {
                    if(InDegrees.ContainsKey(desc))
                    {
                        InDegrees[desc]++;
                    }
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
            PosInGraph++;
            current.position = PosInGraph;
            Planet.NodeEvaluation(current);
            SourceQueue.RemoveFirst();                          // remove node from the queue
            SortedNodes.AddLast(current);                       // add node to sorted list
            
            foreach (Node desc in current.Descendants)          // iterate through all descendants of a node
            {
                try{
                    int deg = --InDegrees[desc];                    // decrement the indegree
                    if (deg == 0)                                   // if it's now 0 it has to be a source
                    {
                        state.NewSources.Add(desc);
                        SourceQueue.AddLast(desc);
                    }
                }
                catch(KeyNotFoundException){
                    
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
            PosInGraph--;

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

            PosInGraph = 0;

            SourceQueue.Clear();
            SortedNodes.Clear();
            InDegrees.Clear();
            StepStack.Clear();

            Planet.PlanetReset();

            AlignmentUtil.finished = false;
            AlgorithmManager.ColourGraph(this);
        }

        public void TextUpdate()
        {
            string queueText = default;
            string sortedText = default;
            string planetName = default;
            string atmosphereName = default;
            string german = default;
            List<Node> tempSortedNodes = new List<Node>(SortedNodes);
            List<Node> tempSourceQueue = new List<Node>(SourceQueue);

            // Counting + translating of Source Nodes
            for(int i = 0; i < tempSourceQueue.Count; i++)
            {
                if(Localisation.isGermanActive)
                    Localisation.Translator.TryGetValue(tempSourceQueue[i].Name, out german);
                else
                    german = tempSourceQueue[i].Name;
                
                if(i < tempSourceQueue.Count - 1)
                {
                    queueText += german + " | ";
                }
                else
                {
                    queueText += german;
                }
            }
            
            // Counting + translating of Sorted Nodes
            for(int i = 0; i < tempSortedNodes.Count; i++)
            {
                if(Localisation.isGermanActive)
                    Localisation.Translator.TryGetValue(tempSortedNodes[i].Name, out german);
                else
                    german = tempSortedNodes[i].Name;
                
                if(i < tempSortedNodes.Count-1)
                {
                    sortedText += german + " -> ";
                }
                else
                {
                    sortedText += german;
                }
            }

            // Reading + Translating of the PlanetState
            if (Localisation.isGermanActive)
                Localisation.Translator.TryGetValue(Planet.State, out german);
            else 
                german = Planet.State;
            
            planetName = german;

            if(Atmosphere.State != null)
            {
                if(Localisation.isGermanActive)
                    Localisation.Translator.TryGetValue(Atmosphere.State, out german);
                else
                    german = Atmosphere.State;
                atmosphereName = german;
            }

            //Formatting it into a nice output
            if(Localisation.isGermanActive)
            {
                if(queueText != null && atmosphereName != null)
                    QueueText.text = "Warteschlange:\n"+queueText+"\n-------------"+sortedText+"\n-------------"+planetName+" mit "+atmosphereName;
                else if(queueText == null && atmosphereName != null)
                    QueueText.text = "Sortierung:\n"+sortedText+"\n-------------Ergebnis:\n"+planetName+" mit "+atmosphereName;
                else if(queueText == null && atmosphereName == null)
                    QueueText.text = "Sortierung:\n"+sortedText+"\n-------------Ergebnis:\n"+planetName;
                else if(queueText != null && atmosphereName == null)
                    QueueText.text = "Warteschlange:\n"+queueText+"\n-------------"+sortedText+"\n-------------"+planetName;
            }
            else
            {
                if(queueText != null && atmosphereName != null)
                    QueueText.text = "Queue:\n"+queueText+"\n-------------"+sortedText+"\n-------------"+planetName+" with "+atmosphereName;
                else if(queueText == null && atmosphereName != null)
                    QueueText.text = "Sequence:\n"+sortedText+"\n-------------Result:\n"+planetName+" with "+atmosphereName;
                else if(queueText == null && atmosphereName == null)
                    QueueText.text = "Sequence:\n"+sortedText+"\n-------------Result:\n"+planetName;
                else if(queueText != null && atmosphereName == null)
                    QueueText.text = "Queue:\n"+queueText+"\n-------------"+sortedText+"\n-------------"+planetName;
            }
        }
        
        
        public void SetToolTip(string tip, Collider2D lastEnter)
        {
            string german = default;
            ToolTip.transform.position = MouseManager.GetMousePos().Z(-2);
            DetermineToolTipPos(tip);
            LastToolTipEnter = lastEnter;

            if(Localisation.isGermanActive)
            {
                Localisation.Translator.TryGetValue(tip, out german);

                    ToolTip.text = german;       
            }
            else
            {
                ToolTip.text = tip;
            }
        }
    
        public void DeleteToolTip(Collider2D lastExit)
        {
            if(LastToolTipEnter == lastExit)
                ToolTip.text = "";
        }

        public void DetermineToolTipPos(string tip)
        {
            switch (tip)
            {
                case "Play":
                    ToolTip.transform.position += new Vector3 (0.5f, -1f, 0f);
                    break;
                case "Step Forward":
                    ToolTip.transform.position += new Vector3(0.5f, -0.5f, 0f);
                    break;
                case "Step Back":
                    ToolTip.transform.position += new Vector3 (0.5f, 0.2f, 0);
                    break;
            }
        }
    }
}
