using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeManager  //manages when Nodes are clicked etc
{
    public static NodeControl holdControl;
    public static NodeControl dragControl;
    public static NodeControl hoverControl;
    

    //make instanceneisn line 
    //make line know start (and end)

    //Make known when Nodes are created from "inventory" 

    public static void StartHold(NodeControl nodeControl)
    {
        holdControl = nodeControl;
    }

    public static void StopHold(NodeControl nodeControl)
    {
        if(holdControl == nodeControl)
        {
            holdControl = null;
        }
    }

    public static void StartDrag(NodeControl nodeControl)
    {
        var prefab = Resources.Load<EdgeControl>("Models/Edge");  //creates new Edge prefab
        var line = Object.Instantiate(prefab); //enables use of line
        
        line.FromNode = nodeControl;
        dragControl = nodeControl;
    }

    public static void StopDrag(NodeControl nodeControl)
    {
        if(dragControl == nodeControl)
            dragControl = null;
    }

    public static void EnterHover(NodeControl nodeControl)
    {
        hoverControl = nodeControl;
    }

    public static void ExitHover(NodeControl nodeControl)
    {
        if(hoverControl == nodeControl)
            hoverControl = null;
    }

}
