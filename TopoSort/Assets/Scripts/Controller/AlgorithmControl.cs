using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using TopoSort;

public class AlgorithmControl : MonoBehaviour
{

    private Collider2D Collider;
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
            Debug.Log("Algorithm clicked");
            Algorithm test = new Algorithm(GraphManager.graph);
        }
    }
}
