using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class VisuellFeedback : MonoBehaviour //check if algorithmmanager hold a node
{
    public static SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GameObject.Find("Node").GetComponentInChildren<SpriteRenderer>();
    }

    public static void visualfeedback()
    {
        if (AlgorithmManager.nodeHold != null)
        {
        //ink red
          spriteRenderer.color = new Color(255,0,0);
        }

        else
        {
        //ink green
         spriteRenderer.color = new Color(0,255,0);
        }

    }


}
