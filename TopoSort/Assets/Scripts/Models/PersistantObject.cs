using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopoSort;
using Models;

public class PersistantObject
{

    public int nextIndex = 0;
    public List<string> PlanetName = new List<string>();
    //public List<int[]> planetList = new List<int[]>();
    public List<Graph> graphList = new List<Graph>();

}

