using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using System.Linq;
using TopoSort;

public class VisuellFeedback //check if algorithmmanager hold a node
{
    public List<SpriteRenderer> spriteRenderers;
    public static GameObject[] Nodes;
    public static GameObject[] Edges;
    public LineRenderer edge;
    public static List<UnityEngine.Component> edgeScripts = new List<UnityEngine.Component>();
    public static float opacity = 0.5f;

    public VisuellFeedback()
    {
        Nodes = GameObject.FindGameObjectsWithTag("Node");
        Edges = GameObject.FindGameObjectsWithTag("Edge");
        edgeScripts = new List<UnityEngine.Component>();
        for(int i = 0; i < Edges.Length; i++)
        {
            edgeScripts.Add(Edges[i].GetComponent(typeof(EdgeControl)));
        }
    }

    public void Update()
    {
        //visualfeedback();
    }
    public void ColourRed(Node node)
    {
        for(int i = 0; i< Nodes.Length; i++)
        {
            if (node.Id.ToString() == Nodes[i].name)
            {
                var spriteRenderer = Nodes[i].GetComponentInChildren<SpriteRenderer>();
                //ink red
                spriteRenderer.color = new Color(255,0,0);
            }
        }
    }
    public void ColourProcess(List<Node> nodes)
    {
        foreach(Node node in nodes)
        {
           
            for(int i = 0 ; i< Edges.Length; i++)
            {
                EdgeControl script = (EdgeControl) Edges[i].GetComponent(typeof(EdgeControl));
                    
                if(script.SourceNode.node.Id == node.Id)
                {
                    var lineRenderer = Edges[i].GetComponent<LineRenderer>();
                    if(AlignmentUtil.finished)
                    {
                        lineRenderer.material.color = new Color(lineRenderer.material.color.r,lineRenderer.material.color.g,lineRenderer.material.color.b,1f);
                    }
                    else
                    {
                        lineRenderer.material.color = new Color(lineRenderer.material.color.r,lineRenderer.material.color.g,lineRenderer.material.color.b,opacity);
                    }
                }
            }
        
             for(int i = 0; i< Nodes.Length; i++) //Check + Färben der Nodes
            {
                if(node.Id.ToString() == Nodes[i].name)
                {
                    var spriteRenderer = Nodes[i].GetComponentInChildren<SpriteRenderer>();
                    if(AlignmentUtil.finished)
                    {
                        spriteRenderer.color = new Color(0,255,0, 1f);
                    }
                    else
                    {
                        spriteRenderer.color = new Color(0,255,0, opacity);
                    }
                }
            }
        
        }
    }

    public static void ColourRevert(List<Node> nodes)
    {
        foreach(Node node in nodes)
        {
            for(int i = 0; i< Nodes.Length; i++) //Check + Färben der Nodes
            {
                if(node.Id.ToString() == Nodes[i].name)
                {
                    var spriteRenderer = Nodes[i].GetComponentInChildren<SpriteRenderer>();
                    spriteRenderer.color = new Color (255, 255, 255, 1);
                }
            }
        
        }
    }


   /* public void visualfeedback()
    {
        

        GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");

        if(AlgorithmManager.finishedNodes.Any(node => node.Id.ToString() == this.name && !AlignmentUtil.finished))
        {
            //ink green and set Opacity to half to emulate removing the node from the graph
            spriteRenderer.color = new Color(0,255,0, opacity);
            //also get the according line and set it's opacity to half
            GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
            for(int i = 0; i < edges.Length; i++)
            {
                edgeScripts.Add(edges[i].GetComponent(typeof(EdgeControl))); //We get the scripts to be able to see what source Node is connected to the line we wanna make transparent
                foreach (EdgeControl script in edgeScripts) //We go through each edge script each time since there's also multiple edges with the same sourceNode tehat need to be transparented
                {
                    if(script.SourceNode.node.Id.ToString() == this.name) //wenn die SourceNode der edge mit der Node übereinstimmt auf die zugegriffen werden soll
                    {
                        edge = edges[i].GetComponent<LineRenderer>();
                        edge.material.color = new Color(edge.material.color.r,edge.material.color.g,edge.material.color.b,opacity);

                    }
                }
            }
            
        }
        if(AlgorithmManager.finishedNodes.Any(node => node.Id.ToString() == this.name && AlignmentUtil.finished))
        {
            
            //gives opacity back to Nodes and Edges
            spriteRenderer.color = new Color(0,255,0, 1f);

            GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
            for(int i = 0; i < edges.Length; i++)
            {
                edgeScripts.Add(edges[i].GetComponent(typeof(EdgeControl))); //We get the scripts to be able to see what source Node is connected to the line we wanna make transparent
                foreach (EdgeControl script in edgeScripts) //We go through each edge script each time since there's also multiple edges with the same sourceNode tehat need to be transparented
                {
                    if(script.SourceNode.node.Id.ToString() == this.name) //wenn die SourceNode der edge mit der Node übereinstimmt auf die zugegriffen werden soll
                    {
                        edge = edges[i].GetComponent<LineRenderer>();
                        edge.material.color = new Color(edge.material.color.r,edge.material.color.g,edge.material.color.b,1f);
                        
                    }
                }
            }
        }

    }*/
}
