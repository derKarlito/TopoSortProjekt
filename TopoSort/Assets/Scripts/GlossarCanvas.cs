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
        TextLoader germanLoader = new TextLoader("Assets\\Resources\\xml\\Glossar_deutsch.xml");
        TextLoader englishLoader = new TextLoader("Assets\\Resources\\xml\\Glossar_englisch.xml");


        // 0 - Graph Entry
        entryGerman = germanLoader.LoadGlossarEntry(0);
        entryEnglish = englishLoader.LoadGlossarEntry(0);

        EntriesGerman.Add(TermsGerman[0], entryGerman);
        EntriesEnglish.Add(TermsGerman[0], entryEnglish);
        
        
        // 1 - Vertex Entry
        entryGerman = germanLoader.LoadGlossarEntry(1);
        entryEnglish = englishLoader.LoadGlossarEntry(1);
        
        EntriesGerman.Add(TermsGerman[1], entryGerman);
        EntriesEnglish.Add(TermsGerman[1], entryEnglish);
        
        
        // 2 - Edge Entry
        entryGerman = germanLoader.LoadGlossarEntry(2);
        entryEnglish = englishLoader.LoadGlossarEntry(2);
        EntriesGerman.Add(TermsGerman[2], entryGerman);
        EntriesEnglish.Add(TermsGerman[2], entryEnglish);
        
        
        // 3 - Degree Entry
        entryGerman = germanLoader.LoadGlossarEntry(3);
        entryEnglish = englishLoader.LoadGlossarEntry(3);
        
        EntriesGerman.Add(TermsGerman[3], entryGerman);
        EntriesEnglish.Add(TermsGerman[3], entryEnglish);
        
        
        
        // 4 - Sorting Entry
        entryGerman = germanLoader.LoadGlossarEntry(4);
        entryEnglish = englishLoader.LoadGlossarEntry(4);
        
        EntriesGerman.Add(TermsGerman[4], entryGerman);
        EntriesEnglish.Add(TermsGerman[4], entryEnglish);
        
        
        
        // 5 - Algorithm
        entryGerman = germanLoader.LoadGlossarEntry(5);
        entryEnglish = englishLoader.LoadGlossarEntry(5);
        
        EntriesGerman.Add(TermsGerman[5], entryGerman);
        EntriesEnglish.Add(TermsGerman[5], entryEnglish);
        
        
        
        // 6 - Kahn Algorithm
        entryGerman = germanLoader.LoadGlossarEntry(6);
        entryEnglish = englishLoader.LoadGlossarEntry(6);
        
        EntriesGerman.Add(TermsGerman[6], entryGerman);
        EntriesEnglish.Add(TermsGerman[6], entryEnglish);
    }

    
    // just there to fill the explanation with some test values
    private static int TestFunc(int i)
    {
        int res = 0;
        int f1 = 0;
        int f2 = 1;
        for (int n = 0; n < i; n++)
        {
            res = f1 + f2;
            f1 = f2;
            f2 = res;
        }

        return res;
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
