using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using TopoSort;

public class GraphManager : MonoBehaviour
{
    public static Graph graph;
    public static bool isActive;
    public static List<Node> SafetyList = new List<Node>();

    void Start()
    {
        graph = new Graph();                        //Creates empty graph for level
        isActive = true;
    }

    void Update() 
    {
        if(isActive)
        {
            SafetyList = graph.Nodes;
        }
    }

    public static void RegainControl()
    {
        isActive = true;
        VisuellFeedback.ColourRevert(graph.Nodes);
        foreach(Node node in graph.Nodes)
        {
            GameObject currentNodeObject = GameObject.Find(node.Id.ToString());
            var nodeControl = currentNodeObject.GetComponent<NodeControl>();
            nodeControl.targetPosition = new Vector2 (0,0);
        }
        graph = new Graph(SafetyList);
    }
}