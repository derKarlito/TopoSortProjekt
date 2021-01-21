using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    static List<Moon> Moons = new List<Moon>();
    
    void Awake()
    {
        Moons.Add(this);
    }

    void OnDestroy()
    {
        Moons.Remove(this);
    }

    public static void SetAllActive(bool active)
    {
        foreach(Moon m in Moons)
        {
            m.gameObject.SetActive(active);
        }
    }
}
