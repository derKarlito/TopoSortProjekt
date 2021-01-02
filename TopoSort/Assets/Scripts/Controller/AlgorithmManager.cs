using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public static class AlgorithmManager //manages when nodes are hold
{
    public static NodeControl nodeHold; //hold node which are currently being viewd
    public static NodeControl nodeFin; //hold node which already done with processing


    public static void StartFeed(NodeControl nodeControl)
    {
        nodeHold = nodeControl;
    }

    public static void FinFeed(NodeControl nodeControl)
    {
        nodeFin = xxx;
    }

}
