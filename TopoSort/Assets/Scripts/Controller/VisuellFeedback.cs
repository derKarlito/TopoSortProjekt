using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class VisuellFeedback : monobehaviour //check if algorithmmanager hold a node
{
    public SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GameObject.GetComponentInChildren<spriteRenderer>();
    }

    public static void visualfeedback


    {
        if (nodeControl == nodeHold)
        {
        //ink red
          spriteRenderer.Color = Color.Red;
        
        }

        else
        {
        //ink green
         spriteRenderer.Color = Color.Green;
        }

    }


}
