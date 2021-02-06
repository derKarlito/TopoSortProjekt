using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System.Linq;

public class Atmosphere : MonoBehaviour
{
    private static SpriteRenderer Overlay;
    
    public static Atmosphere AtmosphereInstance;

    public static string State;

    private static Sprite[] OverlaySprites;

    public static List<string> RequiredNodes = new List<string>();
    
    public static Dictionary<string, List<string>> Requirements = new Dictionary<string, List<string>>()
    {
        {"Duster", new List<string> {"Atmosphere", "Ground"}},
        {"Toxic Atmosphere", new List<string> {"Atmosphere", "Atmosphere", "Atmosphere"}},
        {"Toxic Fog", new List<string> {"Atmosphere", "Water", "Atmosphere"}},
        {"Thin Atmosphere", new List<string>{"Plants", "Water", "Plants"}},
        {"Hurricanes", new List<string>{"Water", "Atmosphere", "Water"}},
        {"Clouds", new List<string> {"Ground", "Atmosphere"}},
        {"Asteroid Belt", new List<string>{"Moon", "Moon", "Moon", "Ground"}},
        {"Rings", new List<string>{"Moon", "Moon", "Moon", "Atmosphere"}}
    };

    public static List<string> AvailableAtomspheres = new List<string>()
    {
        "Asteroid Belt",
        "Rings",
        "Thin Atmosphere",
        "Duster",
        "Toxic Atmosphere",
        "Hurricanes",
        "Clouds",
        "Toxic Fog"
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
        State = atmosphere;
        for(int i = 0; i < AvailableAtomspheres.Count ; i++)
        {
            
            if(AvailableAtomspheres[i] == atmosphere)
            {
                Overlay.sprite = OverlaySprites[i];
            }
        }
    }
}