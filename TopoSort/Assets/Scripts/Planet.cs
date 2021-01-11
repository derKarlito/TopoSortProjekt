using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System;

public class Planet : MonoBehaviour
{

    private static SpriteRenderer Sprite;

    public static string State = default;

    public static bool Final = false;

    public static int Level = 0;

    public enum PlanetParam //Enum listing all possible Planet attributes. Last item MUST be count, for array length
    {
        Ground,             //all these names are implicitly defined from 0 at the top to 5 for the last element (in this case cause there's 6 Elements)
        Water,              //So these can work wonderfully as indecies
        Plants,
        Atmosphere,
        Moon,
        Count
    }

    int[] CreatedPlanet = new int[(int)PlanetParam.Count]; 
    
    public static Dictionary<string, int[]> Planets = new Dictionary<string, int[]>() //Has all the states with available sprites IDK YET I JUST NEED TEH TRY GET VALUE THING
    {
        {"Desert", new[]{4, 0, 1, 0, 0}},
        {"Lava", new[]{6, 0, 0, 1, 3}},
        {"Earth", new[]{3, 2, 2, 1, 1}},
        {"Earth_Relief_1",new[]{5, 2, 2, 1, 1}},
        {"Earth_Relief_2", new[]{69, 1337, 420, 69, 1312}},
        {"Wastes", new[]{2, 1, 0, 0, 0}},
        {"Mud", new[]{2, 4, 1, 2, 1}},
        {"Gas", new[]{0, 2, 0, 6, 5}},
        {"Stone", new[]{2, 0, 0, 1, 0}}
        /*
        {"Glass", new[]{4, 1, 0, 4, 0}},
        {"Ice", new[]{1, 6, 0, 4, 0}},
        {"Phytoplankton", new[]{1, 4, 3, 2, 4}},
        {"Ocean", new[]{1, 6, 1, 2, 4}},
        {"Toxic", new[]{3, 2, 0, 3, 0}},
        {"Fire", new[]{2, 0, 2, 6, 1}},
        {"Jungle", new[]{2, 3, 6, 2, 2}},
        {"Fire_Jungle", new[]{4, 3, 6, 2, 2}},
        {"Asteroids", new[]{5, 1, 0, 0, 6}},
        {"Obsidian", new[]{6, 5, 0, 1, 0}}
        */
    };

    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public static void NodeEvaluation(Node node)
    {
        int runs = 0;
        Debug.Log("Evaluating node...");
        if(State == default && runs == 0) //first node/working off of default planet
        {
            runs++;
            switch (node.Name)
            {
                case "Plants":
                    State = "Swamp";
                    break;
                case "Ground":
                    State = "Stone";
                    break;
                case "Water":
                    State = "Ice";
                    goto case "final";
                case "final":
                    Final = true;
                    break;
            }
        }
        
        foreach(Node ancestor in node.Ancestors)
        {
            if (ancestor.Name == node.Name) //handles double same node stuff that might just break the planet
            {
                runs++;
                switch (node.Name)
                {
                    case "Ground":
                        State = "Lava";
                        break;
                    case "Atomsphere":
                        State = "Gas";
                        goto case "final";
                    case "Plants":
                        State += "thick atmosphere";
                        break;
                    case "final":
                        Final = true;
                        break;
                    default:
                        break;
                }
            }
        }

        if(State != default && runs == 0)
        {
            if(State == "Swamp" && runs == 0) //Handles all effects applieable to a "Swamp Planet"
            {
                runs++;
                switch (node.Name)
                {
                    case "Water":
                        State = "Mud";
                        break;
                    default:
                        break;
                }
            }
            if(State == "Stone" && runs == 0)
            {
                runs++;
                switch (node.Name)
                {
                    case "Plants":
                        State = "Desert";
                        break;
                    case "Water":
                        State = "Mud";
                        break;
                }
            }
            if(State == "Lava" && runs == 0)
            {
                runs++;
                switch (node.Name)
                {
                    case "Water":
                        State = "Stone";
                        break;
                    case "Atmosphere":
                        State = "Fire";
                        goto case "final";
                    case "Ground":
                        State = "Asteroids";
                        goto case "final";
                    case "final":
                        Final = true;
                        break;
                }
            }
            if(State == "Mud" && runs == 0)
            {
                runs++;
                switch (node.Name)
                {
                    case "Plants":
                        State = "Earth"; //OR "water" OR "islands with plants" wait for visual team input in this case
                        break;
                }
            }
            if(State.Contains("Earth") && runs == 0)
            {
                runs++;
                switch (node.Name)
                {
                    case "Ground":
                        Level++;
                        if(Level > 3)
                        {
                            goto case "Water";
                        }
                        State += "_Relief_"+Level;
                        break;
                    case "Water":
                        State = "Islands"; //unauthorized addition
                        break;
                }
            }
            if(State == "Islands" && runs == 0)
            {
                runs++;
                switch (node.Name)
                {
                    case "Water":
                        State = "Ocean";
                        break;
                    
                }
            }
            if(State == "Ocean" && runs == 0) 
            {
                runs++;
                switch(node.Name)
                {
                    case "Plants":
                        State = "PhytoPlankton";
                        goto case "final";
                    case "final":
                        Final = true;
                        break;
                }
            }
            if(State.Contains("thick atmosphere") && runs == 0)
            {
                runs++;
                switch (node.Name)
                {
                    case "Plants":
                        State = "Fire";
                        goto case "final";
                    case "final":
                        Final = true;
                        break;
                }
            }
        }
        PlanetCreation();
    }
    */

    public void NodeEvaluation(Node node)
    {
        int multiplier = (int) Mathf.Ceil((node.Ancestors.Count + node.Descendants.Count)*0.5f); //Swap position for Count Ancs + count descan * 0.5 bc of Quellen and senken who only have In or Out degree then round up by 0.5
        Debug.Log("Node gewicht von: "+node.Name+" ist "+multiplier);                              
        int attribute = (int)Enum.Parse(typeof(PlanetParam), node.Name);

        CreatedPlanet[attribute] += multiplier;

        string planetDisplayed = Diff(CreatedPlanet, Planets);

        SetPlanetSprite(planetDisplayed);

    }

    //returns the planet which is closest to whatever the player created
    public string Diff(int[] createdPlanet, Dictionary<string, int[]> planetArchetype)
    {
        int absDifference = 99;
        string archetype = "";
        foreach (KeyValuePair<string, int[]> valuePair in planetArchetype)
        {
            int localDifference = 0;
            for(int i = 0; i < createdPlanet.Length; i++)
            {
                localDifference += Mathf.Abs(createdPlanet[i] - valuePair.Value[i]);
            }
            if(localDifference <= absDifference)     //if archetype with less difference is found. We want equl too, bc the latest change is valued more to us and the player
            {
                absDifference = localDifference;    //make that smaller difference the new reference value
                archetype = valuePair.Key;          //remember which key has that least difference
            }
        }
        
        return archetype;
    }

    public static void SetPlanetSprite(string planet)
    {
        Debug.Log("Planet being changed?");
        
        if(Resources.Load<Sprite>("Sprites/Planets/"+planet+"_Planet") != null)
        {
            Sprite.sprite = Resources.Load<Sprite>("Sprites/Planets/"+planet+"_Planet"); 
        }
        else
        {
            Debug.Log("!!!!!!!!! Planet Not Found !!!!!!!!");
            Debug.Log(planet);
            Sprite.sprite = Resources.Load<Sprite>("Sprites/Planets/Default_Planet");
        }
    }

}
