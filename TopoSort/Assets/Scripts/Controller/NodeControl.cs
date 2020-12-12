using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeControl : MonoBehaviour
{

    private float startPosX;
    private float startPosY;
    private bool isHeld = false;
    private bool isDragged = false;
    
    Collider2D Collider;

    void Start()
    {
        Collider = GetComponent<Collider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        MouseSignal();

        if (isHeld)
        {
           Vector3 mousePos = MouseManager.GetMousePos();

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
        }

        if(isDragged)
        {
            Vector3 mousePos = MouseManager.GetMousePos();
        }
        
    }

    private bool MouseHover()
    {
        Vector3 mousePos = MouseManager.GetMousePos();
        return Collider.OverlapPoint(mousePos);
    }

    private void MouseSignal() 
    {
        bool onNode = MouseHover();
        
        if (Input.GetMouseButtonDown(0) && onNode)  //left mouse button drags the node on the screen
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            isHeld = true;

        }

        if(Input.GetMouseButtonUp(0))
        {
            isHeld = false;
        }

        if(Input.GetMouseButtonDown(1) && onNode) //right mouse button draws a line between Node and cursor
        {

            isDragged = true;
            NodeManager.StartDrag(this);
        }

        if(Input.GetMouseButtonUp(1) && isDragged)
        {
            isDragged = false;
            NodeManager.StopDrag(this);
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
