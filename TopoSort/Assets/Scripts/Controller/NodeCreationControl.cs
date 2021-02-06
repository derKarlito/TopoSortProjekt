using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System;
using System.Linq;
using TMPro;

public class NodeCreationControl : MonoBehaviour
{
    private Collider2D Collider;
    public string NodeValue;
    private static Sprite[] NodeSprites;
    public Planet Planet;
    public TextMeshPro ActivePlanetStat;
    public TextMeshPro ToolTip;

    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponentInChildren<Collider2D>();
        NodeSprites = Resources.LoadAll<Sprite>("Sprites/Nodes/node_sprites");
        ActivePlanetStat = GetComponentInChildren<TextMeshPro>();
        //ToolTip = GameObject.FindWithTag("ToolTip").GetComponent<TextMeshPro>();
        ToolTip.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlanetStat();
        bool onInventory = MouseManager.MouseHover(Collider);
        if(onInventory)
        {
            InventoryManager.EnterHover(this);
            SetToolTip();
        }
        else
        {
            InventoryManager.ExitHover(this);
            DeleteToolTip();
        }
        if(Input.GetMouseButtonDown(0) && onInventory)
        {
            PlaySound();
            var prefab = Resources.Load<NodeControl>("Models/Node");  //creates new Node prefab
            var node = UnityEngine.Object.Instantiate(prefab);          //enables use of Node
            node.name = (string.Empty + node.GetInstanceID());

            Vector3 pos = MouseManager.GetMousePos().Z(5);  //".Z" von VectorUtil extension-Method. Sets Z to 0 bc otherwise the node isn't in cam on creation
            node.transform.position = pos;
            node.isHeld = true;                             //to immediately start dragging it
            node.sprite.sprite = NodeSprites[FindSpriteIndex()]; 
            node.node = new Node(NodeValue, node.GetInstanceID());     //Initializes new Node with unique identifier
            GraphManager.graph.AddNode(node.node);      //Graph gets updated to contain new node
        }
    }

    public void PlaySound()
    {
        SoundManagerScript.PlaySound(NodeValue);
    }

    public int FindSpriteIndex()
    {
        int spriteIndex = 0;
        for(int i = 0; i < NodeSprites.Length; i++)
        {
            if(NodeSprites[i].name == NodeValue)
            {
                spriteIndex = i;
                break;
            }
        }
        return spriteIndex;
    }

    public void UpdatePlanetStat()
    {
        var zeroArray = new int[Planet.CreatedPlanet.Length];
        for(int i = 0; i < zeroArray.Length; i++)
        {
            zeroArray[i] = 0;
        }
        if(Planet.CreatedPlanet == null || Enumerable.SequenceEqual(Planet.CreatedPlanet, zeroArray))
        {
            return;
        }
        
        var planetStats = Planet.CreatedPlanet;

        int attribute = (int)Enum.Parse(typeof(Planet.PlanetParam), NodeValue);

        ActivePlanetStat.text = planetStats[attribute].ToString();
    }

    public void SetToolTip()
    {
        string german = default;
        ToolTip.transform.position = MouseManager.GetMousePos().Z(-2);
        ToolTip.transform.position += new Vector3 (0.5f, -0.2f, 0);

        if(Localisation.isGermanActive)
        {
            Localisation.Translator.TryGetValue(NodeValue, out german);

            if(german == "Atmosphäre")
            {
                ToolTip.text = "Atmo\nsphäre";
            }
            else
            {
                ToolTip.text = german;
            }
        }
        else
        {
            ToolTip.text = NodeValue;
        }
    }
    public void DeleteToolTip()
    {
        ToolTip.text = "";
    }
}
