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
        for (int i = 0; i < TermsGerman.Length; i++)
        {
            string[] syns = new string[TermsGerman.Length - i];
            for (int j = 0; j < syns.Length; j++ )
            {
                syns[j] = "" + j;
            }

            string expl = "" + TestFunc((i + 1));
            
            
            GlossarEntry entry = new GlossarEntry(TermsGerman[i], syns, expl);
            GlossarEntry entryEnglish = new GlossarEntry(TermsEnglish[i], syns, expl + " ---- English");

            Debug.Log(string.Format("Added Entry: [{0}, {1}]", TermsGerman[i], expl));
            Debug.Log(string.Format("Added Entry: [{0}, {1}]", TermsEnglish[i], expl));
            
            EntriesGerman.Add(TermsGerman[i], entry);
            EntriesEnglish.Add(TermsGerman[i], entryEnglish);

        }
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
        LoadEntries();
        SetTexts(TermsGerman[0]);
        
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

        for (int i = 0; i < entry.GetSynonyms().Length; i++)
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
