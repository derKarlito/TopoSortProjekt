using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public static class AlgorithmManager //manages when nodes are hold
{
    public static NodeControl nodeHold; //hold node which are currently being viewd



    public static void StartFeed(NodeControl nodeControl)
    {
        nodeHold = nodeControl;
    }

    public static void ExitFeed()
    {
        nodeHold = null;
    }

}
