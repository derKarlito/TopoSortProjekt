using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class NodeCreationControl : MonoBehaviour
{
    private Collider2D Collider;
    public string NodeValue;
    private static Sprite[] NodeSprites;


    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponentInChildren<Collider2D>();
        NodeSprites = Resources.LoadAll<Sprite>("Sprites/Nodes/node_sprites");
    }

    // Update is called once per frame
    void Update()
    {
        bool onInventory = MouseManager.MouseHover(Collider);
        if(onInventory)
        {
            InventoryManager.EnterHover(this);
        }
        else
        {
            InventoryManager.ExitHover(this);
        }
        if(Input.GetMouseButtonDown(0) && onInventory)
        {
            var prefab = Resources.Load<NodeControl>("Models/Node");  //creates new Node prefab
            var node = Object.Instantiate(prefab);          //enables use of Node
            node.name = (string.Empty + node.GetInstanceID());

            Vector3 pos = MouseManager.GetMousePos().Z(5);  //".Z" von VectorUtil extension-Method. Sets Z to 0 bc otherwise the node isn't in cam on creation
            node.transform.position = pos;
            node.isHeld = true;                             //to immediately start dragging it
            node.sprite.sprite = NodeSprites[FindSpriteIndex()]; 
            node.node = new Node(NodeValue, node.GetInstanceID());     //Initializes new Node with unique identifier
            GraphManager.graph.AddNode(node.node);      //Graph gets updated to contain new node
        }
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
}
