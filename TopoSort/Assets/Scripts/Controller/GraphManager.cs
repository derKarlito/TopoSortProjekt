using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static Graph graph;
    void Start()
    {
        graph = new Graph();                        //Creates empty graph for level
    }
}