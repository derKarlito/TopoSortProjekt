using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static Graph graph;
    public static bool isActive;
    void Start()
    {
        graph = new Graph();                        //Creates empty graph for level
        isActive = true;
    }

    public static void RegainControl()
    {
        isActive = true;
        VisuellFeedback.ColourRevert(graph.Nodes);
    }
}