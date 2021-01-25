using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseManager //manages mouse position
{
    public static Vector3 GetMousePos(){
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public static bool MouseHover(Collider2D Collider)
    {
        return Collider.OverlapPoint(MouseManager.GetMousePos());
    }
}
