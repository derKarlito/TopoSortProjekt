using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopoSort;

public class ResetNodes : MonoBehaviour
{
    private Collider2D Collider;
    public static GameObject[] Nodes;
    public Algorithm Algorithm;
    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool onButton = MouseManager.MouseHover(Collider);
        if(Input.GetMouseButtonDown(0) && onButton)
        {
            DeleteAllNodes();
            Algorithm.ResetGraph();
        }
    }
    
    public void DeleteAllNodes()
    {
        Nodes = GameObject.FindGameObjectsWithTag("Node"); //Gets all nodes on screen

        foreach (GameObject node in Nodes)  //iterates through every node
        {
            NodeControl nodeControl = node.GetComponent<NodeControl>();
            nodeControl.DeleteNode();
        }
    }
}
