using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using System.Linq;

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
        if (AlgorithmManager.nodeHold?.Id.ToString() == this.name) //"?" to see if nodeHold isn't null
        {
        //ink red
          spriteRenderer.color = new Color(255,0,0);
        }

        if(AlgorithmManager.finishedNodes.Any(node => node.Id.ToString() == this.name))
        {
            
        //ink green
         spriteRenderer.color = new Color(0,255,100,25);
        }

    }
}
