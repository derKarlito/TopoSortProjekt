using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtil
{
    ///<summary>
    ///This is a utility class for extension methods
    ///extension methods can be easily placed as an extension where you need them
    ///in this case they help with whenever a Vector3 is created and needs a specific adjustment for either coordinates
    ///Use like this: 
    ///Vector3 pos = MouseManager.GetMousePos().Z(0);
    ///the above line creates the position of the mouse but instead of taking the z-value of the camera plane, it is adjusted to be 0
    ///</summary>
    public static Vector3 X(this Vector3 value, float newX)
    {
        value.x = newX;
        return value;
    }

    public static Vector3 Y(this Vector3 value, float newY)
    {
        value.y = newY;
        return value;
    }

    public static Vector3 Z(this Vector3 value, float newZ)
    {
        value.z = newZ;
        return value;
    }
}