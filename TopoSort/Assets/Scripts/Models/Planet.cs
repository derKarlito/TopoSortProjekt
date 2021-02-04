using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System;
using System.Linq;

public class Planet : MonoBehaviour
{

    private static SpriteRenderer Sprite;

    private static Sprite[] PlanetSprites;

    private static Sprite[] MoonSprites;

    public static string State = "Default";

    public enum PlanetParam //Enum listing all possible Planet attributes. Last item MUST be count, for array length
    {
        Ground,             //all these names are implicitly defined from 0 at the top to 5 for the last element (in this case cause there's 6 Elements)
        Water,              //So these can work wonderfully as indecies
        Plants,
        Atmosphere,
        Moon,
        Count
    }

    public int[] CreatedPlanet = new int[(int)PlanetParam.Count]; 
    
    public static Dictionary<string, int[]> Planets = new Dictionary<string, int[]>() //Has all the planets and their archetype stats
    {
        {"Default", new[]{0, 0, 0, 0, 0}},
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
        {"Asteroids", new[]{5, 1, 0, 0, 6}},
        {"Obsidian", new[]{6, 5, 0, 1, 0}}
       /* {"Jungle", new[]{2, 3, 6, 2, 2}},
        {"Earth_Relief_2", new[]{69, 1337, 420, 69, 1312}},
        {"Fire_Jungle", new[]{4, 3, 6, 2, 2}}, */
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
        "Desert",
        "Asteroids",
        "Obsidian"
    };

    // Start is called before the first frame update
    void Start()
    {
        Sprite = GetComponentInChildren<SpriteRenderer>();

        PlanetSprites = Resources.LoadAll<Sprite>("Sprites/Planets/spritesheet_planets");
        MoonSprites = Resources.LoadAll<Sprite>("Sprites/Planets/Moons");
        
        PrepareMoons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NodeEvaluation(Node node)
    {
        int maxBase = 6;    //maximum Bonus given to the first node in the graph
        int multiplier = Mathf.Clamp(maxBase-node.position, 1, maxBase);//+((node.Ancestors.Count + node.Descendants.Count-1)/2 +1); //Swap position for Count Ancs + count descan * 0.5 bc of Quellen and senken who only have In or Out degree then round up by 0.5                             
        int attribute = (int)Enum.Parse(typeof(PlanetParam), node.Name);

        CreatedPlanet[attribute] += multiplier;

        State = DiffManhattan(CreatedPlanet, Planets);

        if(node.Name == "Moon" && State != "Asteroids")
        {
            Moon.SetAllActive(true);
            AddMoon();
        }

        Atmosphere.AddNode(node);

        SetPlanetSprite(State);

    }

    public void PrepareMoons()
    {
        var index = 0;
        var origin = this.transform.position;
        var radius = Sprite.transform.localScale.x/2 + 0.75;
        // create nodes
        for(index = 0 ; index < MoonSprites.Length; index++)
        {
            //create child with spriterenderer with moon
            GameObject moonObject = new GameObject("Moon"+index);
            moonObject.transform.parent = this.transform;
            moonObject.AddComponent<SpriteRenderer>();
            moonObject.AddComponent<Moon>();
            

            // set position of the new gameobject
            var new_x = origin.x + radius * (float)(Mathf.Cos(2 * index * Mathf.PI / MoonSprites.Length));
            var new_y = origin.y + radius * (float)(Mathf.Sin(2 * index * Mathf.PI / MoonSprites.Length));
            
            moonObject.transform.position = new Vector2((float)new_x, (float)new_y);
        }
    }

    public void AddMoon()
    {
        System.Random random = new System.Random();
        int index = random.Next(12); //random value for the placement of the moon
        SpriteRenderer moonSprite = GameObject.Find("Moon"+index).GetComponentInChildren<SpriteRenderer>();
        index = random.Next(12); //another random value for the sprite of the moon
        moonSprite.sprite = MoonSprites[index];
        moonSprite.transform.localScale = new Vector3(4,4,0);
    }

    public void RemoveMoon()
    {
        for(int i = 0; i < MoonSprites.Length; i++)
        {
            SpriteRenderer localMoonSprite = GameObject.Find("Moon"+i).GetComponentInChildren<SpriteRenderer>();
            if(localMoonSprite.sprite != null)
            {
                localMoonSprite.sprite = null;
                break;
            }
        }
        Moon.Moons.RemoveAt(0);
    }

    public void RemoveNode(Node node)
    {
        int maxBase = 6;    //maximum Bonus given to the first node in the graph
        int multiplier = Mathf.Clamp(maxBase-node.position, 0, maxBase)+((node.Ancestors.Count + node.Descendants.Count-1)/2 +1); //Swap position for Count Ancs + count descan * 0.5 bc of Quellen and senken who only have In or Out degree then round up by 0.5                             
        int attribute = (int)Enum.Parse(typeof(PlanetParam), node.Name);

        CreatedPlanet[attribute] -= multiplier;

        for(int i = 0; i < CreatedPlanet.Length; i++)
        {
            if(CreatedPlanet[i] < 0)                    //Prevents having negative values and displaying the opposite to the source planet
            {
                CreatedPlanet[i] = 0;
            }
        }

        string planetDisplayed = DiffRay(CreatedPlanet, Planets);

        if(node.Name == "Moon" && planetDisplayed != "Asteroids")
        {
            Moon.SetAllActive(true);
            RemoveMoon();
        }

        Atmosphere.RemoveNode(node);

        SetPlanetSprite(planetDisplayed);

    }

    public void PlanetReset()
    {
        CreatedPlanet = new int[(int)PlanetParam.Count];  //cleans up the planet
        Atmosphere.ResetAtmosphere();
    }

    public static void RemoveAllMoons()
    {
        for(int i = 0; i < MoonSprites.Length; i++)
        {
            SpriteRenderer localMoonSprite = GameObject.Find("Moon"+i).GetComponentInChildren<SpriteRenderer>();
            if(localMoonSprite.sprite != null)
            {
                localMoonSprite.sprite = null;
            }
        }
        Moon.Moons.Clear();
    }

    //returns the planet which is closest to whatever the player created
    public string DiffManhattan(int[] createdPlanet, Dictionary<string, int[]> planetArchetype)
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
            //Debug.Log($"{valuePair.Key}: {localDifference}");
            if(localDifference <= absDifference)     //if archetype with less difference is found. We want equl too, bc the latest change is valued more to us and the player
            {
                absDifference = localDifference;    //make that smaller difference the new reference value
                archetype = valuePair.Key;          //remember which key has that least difference
            }
        }
        //Debug.Log($"{archetype}: {absDifference}");
        
        return archetype;
    }

    public string DiffRay(int[] createdPlanet, Dictionary<string, int[]> planetArchetype)
    {
        int absDifference = 0;
        string archetype = "";
        foreach (KeyValuePair<string, int[]> valuePair in planetArchetype)
        {
            var closestPoint = ArrMult(valuePair.Value, ArrDotProduct(createdPlanet, valuePair.Value)); 
            var distance = ArrSquareDistance(createdPlanet, closestPoint);
            Debug.Log($"{valuePair.Key}: {distance}");
            if(distance >= absDifference)     //if archetype with less difference is found. We want equl too, bc the latest change is valued more to us and the player
            {
                absDifference = distance;    //make that smaller difference the new reference value
                archetype = valuePair.Key;          //remember which key has that least difference
            }
        }
        return archetype;
    }

    public int ArrDotProduct(int[] factor1, int[] factor2)
    {
        int result = 0;

        for(int i = 0; i < factor1.Length; i++)
        {
            result += factor1[i]*factor2[i];
        }

        return result;
    }

    public int[] ArrMult(int[] array, int factor)
    {
        return array.Select(value => value * factor).ToArray();  //Select takes each value of the array and the content of () makes each element of the array (named value) be itself times the factor 
                                                                 //Actually returns a new array and the original is left compeltely in tact
    }

    public int ArrSquareDistance(int[] array1, int[] array2)
    {
        return array1.Zip(array2, (a,b) => (a-b)*(a-b)).Sum(); //creates a list of the squared differences of two arrays and then sums those together aka a^2 + b^2 + ... 
    }


    public void SetPlanetSprite(string planet)
    {
        bool defaultCheck = true;

        for(int i = 0 ; i < CreatedPlanet.Length; i++)
        {
            if(CreatedPlanet[i] > 0)
            {
                defaultCheck = false;
                break;
            }
        }
        if(defaultCheck)
        {
            Sprite.sprite = PlanetSprites[0];
            return;
        }

        Atmosphere.SetAtmosphere(Atmosphere.GetAtmosphere());

        if(planet == "Asteroids")
        {
            Moon.SetAllActive(false);
            Atmosphere.SetAtmosphereActive(false);
        }
        

        for(int i = 0; i < AvailablePlanets.Count ; i++)
        {
            
            if(AvailablePlanets[i] == planet)
            {
                Sprite.sprite = PlanetSprites[i];
            }
        }
    }

}
