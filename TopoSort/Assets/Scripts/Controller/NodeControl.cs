using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;

public class NodeControl : MonoBehaviour
{
    public float startPosX;
    public float startPosY;
    public bool isHeld = false;
    public int value = 0;
    public TextMeshPro text;    
    Collider2D Collider;
    public Node node;
    public int gnodeid;
    
    void Start()
    {
        Collider = GetComponent<Collider2D>();
        text.text = value.ToString();
        gnodeid = 0;                                    //Unique Nodeid for every Node, counts from 0 for every Node created
    }

    // Update is called once per frame
    void Update()
    {
        MouseSignal();
        if (isHeld) //the action of dragging the node across the screen
        {
            Vector3 mousePos = MouseManager.GetMousePos();

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
        }       
    }

    private void MouseSignal() 
    {
        bool onNode = MouseManager.MouseHover(Collider);        
        if (Input.GetMouseButtonDown(0) && onNode)  //left mouse button drags the node on the screen
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;
            isHeld = true;
            node = new Node(gnodeid++.ToString());            //Initializes new Node with unique identifier
            GraphManager.graph.AddNodes(node);                        //Graph gets updated
        }

        if(Input.GetMouseButtonUp(0) && isHeld)
        {
            isHeld = false;
            if(InventoryManager.hoverControl != null) //Node gets deleted if Let go of over inventory
            {
                GraphManager.graph.RmvNode(node);            //Removes Node from Graph
                node.rmvDescendants(node.Descendants);        //Removes all Descendants
                Destroy(gameObject);                           //Destroys this Object
            }
        }

        if(Input.GetMouseButtonDown(1) && onNode) //right mouse button draws a line between Node and cursor
        {
            NodeManager.StartDrag(this);
        }
        if(onNode)
        {
            NodeManager.EnterHover(this);
        }
        else{
            NodeManager.ExitHover(this);
        }
    }
}
