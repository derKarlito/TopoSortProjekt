using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeControl : MonoBehaviour 
{
    public float HitboxShrink;
    public float MinDistance;
    public bool BeingCreated = true;
    public NodeControl SourceNode;
    public NodeControl TargetNode;
    public LineRenderer Line;
    //New class Edge control. for clicking Edges
    //Line needs to know when clicked
    private void Start()
    {
        Line = GetComponent<LineRenderer>();
    }

    private void Update() 
    {
        if(SourceNode == null || (!BeingCreated && TargetNode == null)) //if one of both nodes is not there anymore,edge destroys itself
        {
            Destroy(gameObject);
            return;                         //no update. Edge broken
        }
        Line.SetPosition(0, SourceNode.transform.position);
        if (!BeingCreated)                    //if being created is false, then the edge has a definite End aka the TargetNode
        {
            Line.SetPosition(1, TargetNode.transform.position);
            if(!SourceNode.node.Descendants.Contains(TargetNode.node)) //otherwise we'll get 2000+ times the same descendant
            {
                SourceNode.node.addDescendant(TargetNode.node);
            }
        }
        else                                                //while it is being created, then the other end points to the mousePointer
            Line.SetPosition(1, MouseManager.GetMousePos().Z(0));

        if(Input.GetMouseButtonUp(1) && BeingCreated)       //right-MouseButton let go while Edge does not have End Point
        {
            if(NodeManager.hoverControl != null)            //If mouse is hovering over any node
            {
                TargetNode = NodeManager.hoverControl;
                BeingCreated = false;
                if(TargetNode == SourceNode)  //Prevents Nodes being connected to themselves
                    Destroy(gameObject);
            }
            else   //if mouse is not hovering over a node the edge gets destroyed
            {
                Destroy(gameObject);
            }
        }
        if(Input.GetMouseButtonDown(1) && !BeingCreated) //DeletesEdge per right click if edge does really exist
        {
            if(IsOnEdge())
            {
                Destroy(gameObject);
                SourceNode.node.rmvDescendants(TargetNode.node);
            }
        }
    }

    public bool IsOnEdge()
    {
        Vector2 StartPos = SourceNode.transform.position;
        Vector2 EndPos = TargetNode.transform.position;

        Vector2 StartToEnd = EndPos - StartPos;

        StartToEnd.Normalize();                         //set Length to 1
        StartPos += StartToEnd * HitboxShrink;          //pushes start towards End
        EndPos -= StartToEnd * HitboxShrink;            //push End towards start
                                                        //so that there's no difficulty adding a ton of edges to a node                                                
        StartToEnd = EndPos - StartPos;                 //recalcute the new whole length of the line to see if the mouse is on that part of the line
        Vector3 MousePos = MouseManager.GetMousePos();
        Vector2 MouseToStart = (Vector2) MousePos - StartPos;    
        //h is progress on Line
        float h = Mathf.Clamp01(Vector2.Dot(MouseToStart, StartToEnd)/StartToEnd.sqrMagnitude); //It's complex, be happy other ppl figured it out instead of us
        float distance = Vector2.Distance(MouseToStart, StartToEnd*h); 
        if(distance <= MinDistance) //then mouse is considered "on Edge"
        {
            return true;
        }
        return false;
    }
    
    private void OnDrawGizmos() //this helped visualize the precise measurements for the shrinking and the min-Distance
    {
        Vector2 sourcePos = SourceNode.transform.position;
        Vector2 targetPos =  TargetNode.transform.position;

        Vector2 StartToEnd = targetPos - sourcePos;
        StartToEnd.Normalize();                          //set Length to 1
        sourcePos += StartToEnd * HitboxShrink;          //pushes start towards End
        targetPos -= StartToEnd * HitboxShrink;            //push End towards start

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(sourcePos, MinDistance);
        Gizmos.DrawWireSphere(targetPos, MinDistance);
    }
}
