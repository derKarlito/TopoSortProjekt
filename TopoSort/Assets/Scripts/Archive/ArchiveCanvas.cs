using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TopoSort.Controller;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveCanvas : MonoBehaviour
{
    private Dictionary<string, Image> PlanetSprites = new Dictionary<string, Image>();
    private Dictionary<string, Image> AtmosphereSprites = new Dictionary<string, Image>();

    public TextMeshProUGUI DiscoveredPlanetText;
    public TextMeshProUGUI DiscoveredAtmosphereText;

    public Color DiscoveredColor = Color.white;
    public Color CoveredColor = Color.black;
    
    // Start is called before the first frame update
    void Start()
    {
        ArchiveManager.LoadDataFromFile();
        
        LoadSprites();
        
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DiscoveredPlanetText.text = "Discovered Planets";
        DiscoveredAtmosphereText.text = "Discovered Atmospheres";
        
        if (Localisation.isGermanActive)
        {
            DiscoveredPlanetText.text = "Entdeckte Planeten";
            DiscoveredAtmosphereText.text = "Entdeckte Atmosphären";    
        }
        
    }

    public void OnEnable()
    {
        ColorArchive();
    }



    private void LoadSprites()
    {
        Transform tmpTrans = null;

        foreach (string planetType in Planet.AvailablePlanets)
        {
            if (planetType != "Default")
            {
                for (int i = 0; i < 4; i++)
                {
                    tmpTrans = transform.Find("Planets/Row" + i + "/" + planetType);

                    if (tmpTrans != null)
                    {
                        break;
                    }
                }

                if (tmpTrans)
                {
                    PlanetSprites[planetType] = tmpTrans.gameObject.GetComponent<Image>();
                }

            }
        }
        
        foreach (string atmosphereType in Atmosphere.AvailableAtomspheres)
        { 
            tmpTrans = this.transform.Find("Atmospheres/Row0/" + atmosphereType);

            if (tmpTrans != null)
            {
                    AtmosphereSprites[atmosphereType] = tmpTrans.gameObject.GetComponent<Image>();
            }
        }
    }
    
    
    public void ColorArchive()
    {
        CoveredColor.a = 1;
        DiscoveredColor.a = 1;
        
        
        foreach (string planetType in Planet.AvailablePlanets)
        {
            if (planetType != "Default" && PlanetSprites.ContainsKey(planetType))
            {
                PlanetSprites[planetType].color = CoveredColor;

                if (ArchiveManager.IsPlanetDiscovered(planetType))
                {
                    PlanetSprites[planetType].color = DiscoveredColor;
                } 
            }
        }
        
        foreach (string atmosphereType in Atmosphere.AvailableAtomspheres)
        {
            if (atmosphereType != "Default" && AtmosphereSprites.ContainsKey(atmosphereType))
            {
                AtmosphereSprites[atmosphereType].color = CoveredColor;
                
                if (ArchiveManager.IsAtmosphereDiscovered(atmosphereType))
                {
                    AtmosphereSprites[atmosphereType].color = DiscoveredColor;
                } 
            }
        }
    }
}
