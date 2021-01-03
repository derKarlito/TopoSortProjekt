using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class VisuellFeedback : MonoBehaviour //check if algorithmmanager hold a node
{
    public SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void Update()
    {
        visualfeedback();
    }

    public void visualfeedback()
    {
        if (AlgorithmManager.nodeHold != null)
        {
        //ink red
          spriteRenderer.color = new Color(255,0,0);
        }

        foreach (Node node in AlgorithmManager.finishedNodes)
        {
        //ink green
         spriteRenderer.color = new Color(0,255,0);
        }

    }


}
