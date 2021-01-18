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
    public static float Opacity = 0.5f;

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



    public void ColourGraphState(Algorithm algorithm)
    {

        foreach (GameObject node in Nodes)                                          // iterates through every node object on the screen
        {
            var spriteRenderer = node.GetComponentInChildren<SpriteRenderer>();     

            spriteRenderer.color = new Color(255, 255, 255, 1);                     // sets default colouring

            if (AlignmentUtil.finished)                                             // Green when finished and aligned
            {
                spriteRenderer.color = new Color(0, 255, 0, 1f);
                continue;
            }

            foreach (Node source in algorithm.SourceQueue)                          // colours the sources
            {
                if (source.Id.ToString().Equals(node.name))
                {                                                                                                                                           
                    spriteRenderer.color = new Color(192, 64, 0, 1);                   
                }                                                                                                                                                  
            }

            foreach (Node sorted in algorithm.SortedNodes)                          // colours the nodes already sorted
            {
                if (sorted.Id.ToString().Equals(node.name))
                {
                    spriteRenderer.color = new Color(0, 255, 0, 1);
                }
            }

            if (algorithm.CurrentState != null) {                       
                if (algorithm.CurrentState.Current.Id.ToString().Equals(node.name)) // colours the node the algorithm is currently looking at
                {
                    spriteRenderer.color = new Color(192, 0, 0, 1);
                }
            }
        }

        foreach (GameObject edge in Edges)                                          // iterates through every edge object on the screen
        {

            EdgeControl script = edge.GetComponent<EdgeControl>();
            var lineRenderer = edge.GetComponent<LineRenderer>();
            Color color = lineRenderer.material.color;

            lineRenderer.material.color = new Color(color.r, color.g, color.b, 1f);


            if (AlignmentUtil.finished)
            {
                lineRenderer.material.color = new Color(color.r, color.g, color.b, 1f);
                continue;
            }

            foreach (Node source in algorithm.SourceQueue)                          // colours the edges coming from the sources
            {
                if (script.SourceNode.node.Equals(source))
                {
                    lineRenderer.material.color = new Color(color.r, color.g, color.b, 0.75f);
                }
            }

            foreach (Node sorted in algorithm.SortedNodes)                          // colours the edges coming from already sorted nodes
            {
                if (script.SourceNode.node.Equals(sorted))
                {
                    lineRenderer.material.color = new Color(color.r, color.g, color.b, Opacity);
                }
            }

            if (algorithm.CurrentState != null)
            {
                if (script.SourceNode.node.Equals(algorithm.CurrentState.Current))       // colours the edges of the current viewed node
                {
                    lineRenderer.material.color = new Color(color.r, color.g, color.b, Opacity);
                }
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
                        lineRenderer.material.color = new Color(lineRenderer.material.color.r,lineRenderer.material.color.g,lineRenderer.material.color.b,Opacity);
                    }
                }
            }
        
             for(int i = 0; i< Nodes.Length; i++) //Check + Färben der Nodes
            {
                if(node.Id.ToString().Equals(Nodes[i].name)) // Object comparisons should use .Equals() instead of ==  
                {
                    var spriteRenderer = Nodes[i].GetComponentInChildren<SpriteRenderer>();
                    if(AlignmentUtil.finished)
                    {
                        spriteRenderer.color = new Color(0,255,0, 1f);
                    }
                    else
                    {
                        spriteRenderer.color = new Color(0,255,0, Opacity);
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
