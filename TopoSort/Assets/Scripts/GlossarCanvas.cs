using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using TopoSort;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class GlossarCanvas : MonoBehaviour
{
    private static Dictionary<string, GlossarEntry> EntriesGerman = new Dictionary<string, GlossarEntry>();
    private static Dictionary<string, GlossarEntry> EntriesEnglish = new Dictionary<string, GlossarEntry>();
    private static GlossarCanvas Instance;
    private static string Last;
    
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI Synonyms;
    public TextMeshProUGUI InfoText;

    public Button CloseButton;

    
    
    private static string[] TermsGerman =
    {
        "Graph",
        "Knoten",
        "Kante",
        "Knotengrad",
        "Sortierung",
        "Algorithmus",
        "Kahn Algorithmus"
    };
    
    private static string[] TermsEnglish =
    {
        "Graph",
        "Vertex",
        "Edge",
        "Degree",
        "Sorting",
        "Algorithmn",
        "Kahn Algorithmn"
    };

    // just there to fill the entry with test values
    private static void LoadEntries()
    {
        GlossarEntry entryGerman;
        GlossarEntry entryEnglish;
        TextLoader germanLoader = new TextLoader("xml/Glossar_deutsch");
        TextLoader englishLoader = new TextLoader("xml/Glossar_englisch");


        for (int i = 0; i < 7; i++)
        {
            entryGerman = germanLoader.LoadGlossarEntry(i);
            entryEnglish = englishLoader.LoadGlossarEntry(i);

            EntriesGerman.Add(TermsGerman[i], entryGerman);
            EntriesEnglish.Add(TermsGerman[i], entryEnglish);
        }
    }
    
    public static GlossarEntry getEntry(string key)
    {
        if (Localisation.isGermanActive)
        {
            return GlossarCanvas.EntriesGerman[key];
        }

        return GlossarCanvas.EntriesEnglish[key];
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        GlossarCanvas.Instance = this;
        try{
            LoadEntries();
            SetTexts(TermsGerman[0]);
        }
        catch{}


        
        CloseButton.onClick.AddListener(Hide);
        this.Hide();
    }

    public void SetTexts(string key)
    {
        GlossarEntry entry = GlossarCanvas.getEntry(key);

        Last = key;
        
        TitleText.GetComponent<TextMeshProUGUI>().text = entry.GetTitle();
        InfoText.GetComponent<TextMeshProUGUI>().text = entry.GetExplanation();

        string syns = "";

        for (int i = 0; i < entry.GetSynonyms().Count; i++)
        {
            if (i == 0)
            {
                if (Localisation.isGermanActive)
                {
                    syns += "Auch: ";
                }
                else
                {
                    syns += "Also: ";
                }
            }
            else
            {
                syns += ", ";
            }

            syns += entry.GetSynonyms()[i];
        }

        Synonyms.GetComponent<TextMeshProUGUI>().text = syns;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static GlossarCanvas GetInstance()
    {
        return GlossarCanvas.Instance;
    }
    
    public void Show()
    {
        SetTexts(Last); 
        
        this.gameObject.SetActive(true);
    }
    
    
    public void Hide()
    {  
        this.gameObject.SetActive(false);
    }
    
}
