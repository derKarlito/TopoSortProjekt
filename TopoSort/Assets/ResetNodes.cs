using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopoSort;
using TMPro;

public class ResetNodes : MonoBehaviour
{
    private Collider2D Collider;
    public static GameObject[] Nodes;
    public Algorithm Algorithm;
    public TextMeshPro ToolTip;
    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool onButton = MouseManager.MouseHover(Collider);
        if(onButton)
        {
            SetToolTip("Delete All Nodes");
            if(Input.GetMouseButtonDown(0))
            {
                DeleteAllNodes();
                Algorithm.ResetGraph();
            }
        }
        else
        {
            DeleteToolTip();
        }
    }
    
    public void DeleteAllNodes()
    {
        Nodes = GameObject.FindGameObjectsWithTag("Node"); //Gets all nodes on screen

        foreach (GameObject node in Nodes)  //iterates through every node
        {
            NodeControl nodeControl = node.GetComponent<NodeControl>();
            nodeControl.DeleteNode();
        }
    }

    public void SetToolTip(string tip)
    {
        string german = default;
        ToolTip.transform.position = MouseManager.GetMousePos().Z(-2);

        if(Localisation.isGermanActive)
        {
            Localisation.Translator.TryGetValue(tip, out german);
            ToolTip.text = german;       
        }
        else
        {
            ToolTip.text = tip;
        }
    }
    public void DeleteToolTip()
    {
        ToolTip.text = "";
    }
}
