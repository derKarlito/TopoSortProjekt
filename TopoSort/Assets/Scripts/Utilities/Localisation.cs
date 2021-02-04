using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TopoSort;

public class Localisation : MonoBehaviour
{
    public static bool isGermanActive = true;

    public Collider2D Collider;

    public Algorithm Algorithm;

    public static Dictionary<string, string> Translator = new Dictionary<string, string>()
    {
        {"Water", "Wasser"},
        {"Ground", "Tektonik"},
        {"Plants", "Pflanzen"},
        {"Moon", "Mond"},
        {"Atmosphere", "Atmosphäre"},
        {"Default", "Leerer Planet"},
        {"Desert", "Wüstenplanet"},
        {"Lava", "Lavaplanet"},
        {"Earth", "Erdplanet"},
        {"Earth_Relief_1", "Hügliger Planet"},
        {"Wastes", "Planet Der Ödnis"},
        {"Mud", "Schlammplanet"},
        {"Gas", "Gasplanet"},
        {"Stone", "Gesteinsplanet"},
        {"Glass", "Glasplanet"},
        {"Ice", "Eisplanet"},
        {"Phytoplankton", "Phytoplanktonplanet"},
        {"Ocean", "Ozeanplanet"},
        {"Toxic", "Gifitger Planet"},
        {"Fire", "Feuerplanet"},
        {"Asteroids", "AsteroidenFeld"},
        {"Duster", "Sandsturm"},
        {"Toxic Atmosphere", "Giftatmosphäre"},
        {"Toxic Fog", "Giftnebel"},
        {"Thin Atmosphere", "Dünner Atmosphäre"},
        {"Hurricanes", "Orkanen"},
        {"Clouds", "Wolken"},
        {"Asteroid Belt", "Asteroidengürtel"},
        {"Rings", "Ringen"},
        {"Obisdian", "Obisdian"}
    };

    private void Start() 
    {
        Collider = GetComponentInChildren<Collider2D>();    
    }

    private void Update() 
    {
        bool onButton = MouseManager.MouseHover(Collider);
        if(Input.GetMouseButtonDown(0) && onButton)
        {
            isGermanActive = !isGermanActive;
            Algorithm.TextUpdate();
        }
    }
}
