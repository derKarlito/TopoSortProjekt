using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryManager //manages when the inventory should listen up
{
   public static NodeCreationControl hoverControl;

   public static void EnterHover(NodeCreationControl nodeControl)
    {
        hoverControl = nodeControl;
    }

    public static void ExitHover(NodeCreationControl nodeControl)
    {
        if(hoverControl == nodeControl)
            hoverControl = null;
    }
}
