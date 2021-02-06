using System.Collections.Generic;
using TMPro;
using TopoSort.Controller;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
    
public class ArchiveCanvas : MonoBehaviour
{
    // stores the sprite objects displayed in the archive
    private Dictionary<string, Image> PlanetSprites = new Dictionary<string, Image>();
    private Dictionary<string, Image> AtmosphereSprites = new Dictionary<string, Image>();

    // stores the header of the archive categories to alter them on a language change
    public TextMeshProUGUI DiscoveredPlanetText;
    public TextMeshProUGUI DiscoveredAtmosphereText;

    // stores the color values the sprite's RGB Values are multiplied with
    public Color DiscoveredColor = Color.white;
    public Color CoveredColor = Color.black;

    
    
    // Reset button
    public Button ResetButton;
    private TextMeshProUGUI ResetButtonContent;
    
    public Color ResetButtonEnterColor = new Color(1.0f, 1.0f, 1.0f);
    public Color ResetButtonExitColor = new Color(0.75f, 0.75f, 0.75f);

    
    
    // Start is called before the first frame update
    void Start()
    {
        ArchiveManager.LoadDataFromFile();
        
        LoadSprites();

        ColorArchive();

        ResetButtonContent = ResetButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        ResetButtonContent.color = ResetButtonExitColor;
        ResetButton.onClick.AddListener(ResetProgress);
        
        EventTrigger.Entry enterEvent = new EventTrigger.Entry();
        enterEvent.eventID = EventTriggerType.PointerEnter;
        enterEvent.callback.AddListener(ButtonEnter);
        
        EventTrigger.Entry exitEvent = new EventTrigger.Entry();
        exitEvent.eventID = EventTriggerType.PointerExit;
        exitEvent.callback.AddListener(ButtonExit);

        EventTrigger trigger = ResetButton.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Add(enterEvent);
        trigger.triggers.Add(exitEvent);
        // this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DiscoveredPlanetText.text = "Discovered Planets";
        DiscoveredAtmosphereText.text = "Discovered Atmospheres";
        ResetButtonContent.text = "Reset Progress";
        
        if (Localisation.isGermanActive)
        {
            DiscoveredPlanetText.text = "Entdeckte Planeten";
            DiscoveredAtmosphereText.text = "Entdeckte Atmosphären";    
            ResetButtonContent.text = "Fortschritt zurücksetzen";
        }
        
    }

    /*
     * Colors the Archive every time the ArchiveCanvas is set to active
     */
    public void OnEnable()
    {
        ColorArchive();
    }


    /*
     * loads the image objects displayed in the game and caches them into a dictionary to avoid further
     * Find() or GetComponent() calls
     */
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


    public void ResetProgress()
    {
        ArchiveManager.ResetDiscoveries();
        ColorArchive();
    }
    
    
    /*
     * colors the archive sprites depending on its associated entity's state of discovery
     */
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
    
    
    public void ButtonEnter(BaseEventData data)
    {
        ResetButtonContent.color = ResetButtonEnterColor;
    }
    
    public void ButtonExit(BaseEventData data)
    {
        ResetButtonContent.color = ResetButtonExitColor;
    }
}
