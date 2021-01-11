using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;

public class NodeControl : MonoBehaviour
{
    public float startPosX;
    public float startPosY;

    public Vector2 targetPosition;
    public bool moveNode = false;
    public bool isHeld = false;
    public int value = 0;
    public SpriteRenderer sprite;    
    Collider2D Collider;
    public Node node;
    
    void Start()
    {
        Collider = GetComponent<Collider2D>();
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
        if(!GraphManager.isActive && targetPosition != new Vector2 (0,0) )
        {
            var t = Time.deltaTime;
            var currentPosition = gameObject.transform.position;
            this.gameObject.transform.position = Vector2.Lerp(currentPosition,targetPosition,t); // move node to target position
        }
        
    }

    private void MouseSignal() 
    {
        bool onNode = MouseManager.MouseHover(Collider);        
        if (Input.GetMouseButtonDown(0) && onNode)  //left mouse button drags the node on the screen
        {
            if(!GraphManager.isActive)
            {
                GraphManager.RegainControl();
            }
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;
            isHeld = true;
        }

        if(Input.GetMouseButtonUp(0) && isHeld)
        {
    
            isHeld = false;
            if(InventoryManager.hoverControl != null) //Node gets deleted if Let go of over inventory
            {
                if(node.Descendants.Count != 0)
                    node.rmvDescendants(node.Descendants);        //Removes all Descendants
                GraphManager.graph.RmvNode(node);            //Removes Node from Graph
                Destroy(gameObject);                         //Destroys this Object
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
