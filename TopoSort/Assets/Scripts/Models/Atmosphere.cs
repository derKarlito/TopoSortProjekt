using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System.Linq;

public class Atmosphere : MonoBehaviour
{
    private static SpriteRenderer Overlay;
    
    public static Atmosphere AtmosphereInstance;

    private static Sprite[] OverlaySprites;

    public static List<string> RequiredNodes = new List<string>();
    
    public static Dictionary<string, List<string>> Requirements = new Dictionary<string, List<string>>()
    {
        {"Sandsturm", new List<string> {"Ground", "Atmosphere", "Ground"}},
        {"Giftatmo", new List<string> {"Atmosphere", "Atmosphere", "Atmosphere"}},
        {"Giftnebel", new List<string> {"Atmosphere", "Water", "Atmosphere"}},
        {"Dünne Atmo", new List<string>{"Plants", "Water", "Plants"}},
        {"Hurricanes", new List<string>{"Water", "Atmosphere", "Water"}},
        {"Wolken", new List<string> {"Ground", "Atmosphere"}},
        {"Asteroidengürtel", new List<string>{"Moon", "Moon", "Moon", "Ground"}},
        {"Ringe", new List<string>{"Moon", "Moon", "Moon", "Atmosphere"}}
    };

    public static List<string> AvailableAtomspheres = new List<string>()
    {
        "Asteroidengürtel",
        "Ringe",
        "Dünne Atmo",
        "Sandsturm",
        "Giftatmo",
        "Hurricanes",
        "Wolken",
        "Giftnebel"
    };


    // Start is called before the first frame update
    void Start()
    {
        OverlaySprites = Resources.LoadAll<Sprite>("Sprites/Planets/Atmosphere_Sprite_Sheet");

        Overlay = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        AtmosphereInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetAtmosphereActive(bool active)
    {
       AtmosphereInstance.gameObject.SetActive(active);
       Debug.Log(active);
    }

    public static void AddNode(Node node)
    {
        RequiredNodes.Add(node.Name);
    }

    public static void RemoveNode(Node node)
    {
        RequiredNodes.Remove(node.Name);
    }

    public static void ResetAtmosphere()
    {
        RequiredNodes.Clear();
    }

    public static string GetAtmosphere() //checks which requirements are met
    {
        string allCurrentNodes = default;
        string atmosphere = default;
        bool atmosphereExists = true;

        foreach (string node in RequiredNodes) // converting the list of all looked at Nodes to a singlestring for ease of access
        {
            allCurrentNodes += node;
        }

        foreach (KeyValuePair<string, List<string>> overlayRequirement in Requirements.Reverse())
        {
            atmosphereExists = true;
            string requirement = default;
            
            foreach (string reqNode in overlayRequirement.Value) //same shit as above
            {
                requirement += reqNode;
            }
            
            if(allCurrentNodes.Contains(requirement)) //Then we got a match for an atmosphere that should be displayed
            {
                atmosphere = overlayRequirement.Key;
                break;
            }
            else
            {
                atmosphereExists = false;
            }
        }
        SetAtmosphereActive(atmosphereExists);
        return atmosphere;
    }

    public static void SetAtmosphere(string atmosphere)
    {
        for(int i = 0; i < AvailableAtomspheres.Count ; i++)
        {
            
            if(AvailableAtomspheres[i] == atmosphere)
            {
                Overlay.sprite = OverlaySprites[i];
            }
        }
    }
}