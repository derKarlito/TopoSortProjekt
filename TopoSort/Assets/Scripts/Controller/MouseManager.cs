using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseManager //manages mouse position
{
    public static Vector3 GetMousePos(){
        Vector3 mousePos;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos); 
        return mousePos; 
    }
}
