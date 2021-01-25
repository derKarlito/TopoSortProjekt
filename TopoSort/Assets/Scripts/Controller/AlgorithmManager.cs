using System.Collections;
using System.Collections.Generic;
using Models;
using TopoSort;
using UnityEngine;

public class AlgorithmManager //manages when nodes are hold
{
    public static Node nodeHold; //hold node which are currently being viewd


    public static List<Node> finishedNodes=new List<Node>();


    public static void StartFeed(Node node)
    {
        VisuellFeedback feedback = new VisuellFeedback();
        nodeHold = node;
        feedback.ColourRed(node);
    }

    public static void ExitFeed(Node node)
    {
        VisuellFeedback feedback = new VisuellFeedback();
        nodeHold = null;
        finishedNodes.Add(node);
        feedback.ColourProcess(finishedNodes);
    }
    public static void EmptyList()
    {
        finishedNodes.Clear();
    }

    public static void ColourGraph(Algorithm algorithm)
    {
        VisuellFeedback feedback = new VisuellFeedback();
        feedback.ColourGraphState(algorithm);
    }



}
