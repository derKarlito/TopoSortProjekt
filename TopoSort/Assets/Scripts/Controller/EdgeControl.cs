using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeControl : MonoBehaviour 
{

    public float minDistance;

    public NodeControl FromNode;
    public NodeControl ToNode;

    public LineRenderer Line;
    //New class Edge control. for clicking Edges

    //Line needs to know when clicked

    private void Start()
    {
        Line = GetComponent<LineRenderer>();
    }

    private void Update() 
    {
        if(FromNode != null)
            Line.SetPosition(0, FromNode.transform.position);
        if(ToNode != null)
            Line.SetPosition(1, ToNode.transform.position);
        else
            Line.SetPosition(1, MouseManager.GetMousePos());

        if(Input.GetMouseButtonUp(1) && ToNode == null)
        {
            if(NodeManager.hoverControl != null)
            {
                ToNode = NodeManager.hoverControl; 
            }else
            {
                Destroy(gameObject);
            }
        }

        if(Input.GetMouseButtonDown(1) && ToNode != null) //DeletesEdge per right click if edge does reall exist
        {

            if(OnEdge(FromNode.transform.position, ToNode.transform.position))
            {
                Destroy(gameObject);
            }
        }
    }

    public bool OnEdge(Vector2 StartPos, Vector2 EndPos)
    {
        Vector3 MousePos = MouseManager.GetMousePos();
        Vector2 MouseToStart = (Vector2) MousePos - StartPos;
        Vector2 StartToEnd = EndPos - StartPos;

        //h is progress on Line
        float h = Mathf.Clamp01(Vector2.Dot(MouseToStart, StartToEnd)/StartToEnd.sqrMagnitude); //It's complex, be happy other ppl figured it out instead of us

        float distance = Vector2.Distance(MouseToStart, StartToEnd*h);

        if(distance <= minDistance)
        {
            return true;
        }

        return false;
    }
}
