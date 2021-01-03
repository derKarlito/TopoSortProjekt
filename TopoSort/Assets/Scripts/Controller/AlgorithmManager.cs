using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class AlgorithmManager //manages when nodes are hold
{
    public static Node nodeHold; //hold node which are currently being viewd

    public static List<Node> finishedNodes=new List<Node>();


    public static void StartFeed(Node node)
    {
        nodeHold = node;
    }

    public static void ExitFeed(Node node)
    {
        nodeHold = null;
        finishedNodes.Add(node);
    }



}
