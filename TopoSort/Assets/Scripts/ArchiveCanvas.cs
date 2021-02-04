using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveCanvas : MonoBehaviour
{

    private static ArchiveCanvas Instance;
    
    private Dictionary<string, bool> DiscoveredPlanets = new Dictionary<string, bool>();
    private Dictionary<string, bool> DiscoveredAtmosphere = new Dictionary<string, bool>();

    private Dictionary<string, GameObject> sprites;
    
    private string SaveFile = "discoveries";
    
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        foreach (string type in Planet.AvailablePlanets)
        {
            DiscoveredPlanets[type] = type == "Default";        // "Default" is set to true by default
        }

        foreach (string type in Atmosphere.AvailableAtomspheres)
        {
            DiscoveredAtmosphere[type] = false;
        }

        LoadDataFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static ArchiveCanvas GetInstance()
    {
        return ArchiveCanvas.Instance;
    }

    public void ColorArchive()
    {
        foreach (string planetType in Planet.AvailablePlanets)
        {
            if (planetType != "Default")
            {
                Debug.Log("Color-Archive: " + planetType);

                if (transform.Find(planetType) == null)
                {
                    Debug.Log("Color-Archive: " + planetType);
                    continue;
                }
                
                GameObject entry = transform.Find(planetType).gameObject;

                
                
                
                Image image = entry.GetComponent<Image>();
                
                image.color = Color.black;

                if (DiscoveredPlanets[planetType])
                {
                    image.color = Color.white;
                } 
            }
        }
        
        foreach (string atmosphereType in Atmosphere.AvailableAtomspheres)
        {
            if (atmosphereType != "Default")
            {
                Image image = this.transform.Find(atmosphereType).gameObject.GetComponent<Image>();

                image.color = Color.black;
                
                if (DiscoveredAtmosphere[atmosphereType])
                {
                    image.color = Color.white;
                } 
            }
        }
    }
    
    /*
     * checks if the planet is new.
     * returns true, if it is, false, if not
     */
    public bool checkPlanet(string planetType)
    {
        if (this.DiscoveredPlanets[planetType])
        {
            return false;
        }

        Debug.Log("Added new Planet: " + planetType);
        
        this.DiscoveredPlanets[planetType] = true;

        GameObject planetSprite = this.transform.Find(planetType).gameObject;
        
        
        return true;
    }
    
    /*
     * checks if the atmosphere is discovered
     * returns true, if it is, false if not
     */
    public bool checkAtmosphere(string atmosphereType)
    {
        if (atmosphereType == null)
        {
            Debug.Log("No Atmosphere");
            return false;
        }
        if (this.DiscoveredAtmosphere[atmosphereType])
        {
            return false;
        }

        Debug.Log("Added new Atmosphere: " + atmosphereType);
        
        this.DiscoveredAtmosphere[atmosphereType] = true;
        
        return true;
    }
    
    
    public void LoadDataFromFile()
    {
        if (File.Exists(SaveFile))
        {
            BinaryReader reader = new BinaryReader(File.Open(SaveFile, FileMode.Open, FileAccess.Read));

            LoadPlanets(reader.ReadBytes(4));
            LoadAtmosphere(reader.ReadBytes(2));
        
            reader.Close();
        }
        else
        {
            File.Create(SaveFile);
        }

        ColorArchive();
        
        string discoveriesSoFar = "";

        foreach (string type in Planet.AvailablePlanets)
        {
            if (this.DiscoveredPlanets[type])
            {
                discoveriesSoFar += type + ", ";
            }
        }
        Debug.Log("Planets: " + discoveriesSoFar);

        discoveriesSoFar = " ";
        foreach (string type in Atmosphere.AvailableAtomspheres)
        {
            if (this.DiscoveredAtmosphere[type])
            {
                discoveriesSoFar += type + ", ";
            }
        }
        Debug.Log("Atmospheres: " + discoveriesSoFar);
        
        Debug.Log("Discoveries loaded");
    }
    
    
    
    public void WriteDataToFile()
    {
        
        if (! File.Exists(SaveFile))
        {
            File.Create(SaveFile);
        }
        
        BinaryWriter writer = new BinaryWriter(File.Open(SaveFile, FileMode.Open, FileAccess.Write));
        
        writer.Write(StorePlanets());
        writer.Write(StoreAtmosphere());
        
        writer.Close();
        
        Debug.Log("Discoveries Saved!");
    }
    
    

    byte[] StorePlanets()
    {
        string tmp;
        int bytePos, bitPos;
        byte[] result = new byte[4];
        byte value;
        
        for (int i = 0; i < Planet.AvailablePlanets.Count; i++)
        {
            tmp = Planet.AvailablePlanets[i];
            bytePos = i / 8;
            bitPos = i % 8;
            value = (byte) (this.DiscoveredPlanets[tmp] ? 0x01 : 0x00);
            result[bytePos] |= (byte) (value << bitPos);
        }
        
        return result;
    }
    
    byte[] StoreAtmosphere()
    {
        string tmp;
        int bytePos, bitPos;
        byte[] result = new byte[2];
        byte value;
        
        for (int i = 0; i < Atmosphere.AvailableAtomspheres.Count; i++)
        {
            tmp = Atmosphere.AvailableAtomspheres[i];
            bytePos = i / 8;
            bitPos = i % 8;
            value = (byte) (this.DiscoveredAtmosphere[tmp] ? 0x01 : 0x00);
            result[bytePos] |= (byte) (value << bitPos);
        }
        
        return result;
    }
    
    

    void LoadPlanets(byte[] data)
    {
        int index;
        string tmp;
        for (int bytePos = 0; bytePos < data.Length; bytePos++)
        {
            for (int bitPos = 0; bitPos < 8; bitPos++)
            {
                index = bytePos * 8 + bitPos;
                if (index < Planet.AvailablePlanets.Count)
                {
                    tmp = Planet.AvailablePlanets[index];
                    DiscoveredPlanets[tmp] = (data[bytePos] & (0x01 << bitPos)) > 0 ? true : false;
                }
            }
        }
    }
    
    void LoadAtmosphere(byte[] data)
    {
        int index;
        string tmp;
        for (int bytePos = 0; bytePos < data.Length; bytePos++)
        {
            for (int bitPos = 0; bitPos < 8; bitPos++)
            {
                index = bytePos * 8 + bitPos;
                if (index < Atmosphere.AvailableAtomspheres.Count)
                {
                    tmp = Atmosphere.AvailableAtomspheres[index];
                    DiscoveredAtmosphere[tmp] = (data[bytePos] & (0x01 << bitPos)) > 0 ? true : false;
                }
            }
        }
    }

    void Hide()
    {
        this.gameObject.SetActive(false);
    }

    void Show()
    {
        this.gameObject.SetActive(true);
    }
}
