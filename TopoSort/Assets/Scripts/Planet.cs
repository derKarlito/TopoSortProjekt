using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System;

public class Planet : MonoBehaviour
{

    private static SpriteRenderer Sprite;

    private static Sprite[] PlanetSprites;

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
    
    public static Dictionary<string, int[]> Planets = new Dictionary<string, int[]>() //Has all the planets and their archetype stats
    {
        {"Desert", new[]{4, 0, 1, 0, 0}},
        {"Lava", new[]{6, 0, 0, 1, 3}},
        {"Earth", new[]{3, 2, 2, 1, 1}},
        {"Earth_Relief_1",new[]{5, 2, 2, 1, 1}},
        {"Wastes", new[]{2, 1, 0, 0, 0}},
        {"Mud", new[]{2, 4, 1, 2, 1}},
        {"Gas", new[]{0, 2, 0, 6, 5}},
        {"Stone", new[]{2, 0, 0, 1, 0}},
        {"Glass", new[]{4, 1, 0, 4, 0}},
        {"Ice", new[]{1, 6, 0, 4, 0}},
        {"Phytoplankton", new[]{1, 4, 3, 2, 4}},
        {"Ocean", new[]{1, 6, 1, 2, 4}},
        {"Toxic", new[]{3, 2, 0, 3, 0}},
        {"Fire", new[]{2, 0, 2, 6, 1}},
       /* {"Jungle", new[]{2, 3, 6, 2, 2}},
        {"Earth_Relief_2", new[]{69, 1337, 420, 69, 1312}},
        {"Fire_Jungle", new[]{4, 3, 6, 2, 2}},
        {"Asteroids", new[]{5, 1, 0, 0, 6}},
        {"Obsidian", new[]{6, 5, 0, 1, 0}} */
    };

    public static List<string> AvailablePlanets = new List<string>()
    {
        "Default",
        "Stone",
        "Glass",
        "Mud",
        "Ice",
        "Wastes",
        "Earth",
        "Lava",
        "Ocean",
        "Fire",
        "Earth_Relief_1",
        "Toxic",
        "Phytoplankton",
        "Gas",
        "Desert"
    };

    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();

        PlanetSprites = Resources.LoadAll<Sprite>("Sprites/Planets/spritesheet_planets");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        Debug.Log(planet);
        for(int i = 0; i < AvailablePlanets.Count ; i++)
        {
            
            if(AvailablePlanets[i] == planet)
            {
                Debug.Log(AvailablePlanets[i]);
                Debug.Log(PlanetSprites[i].name);
                Sprite.sprite = PlanetSprites[i];
            }
        }
    }

}
