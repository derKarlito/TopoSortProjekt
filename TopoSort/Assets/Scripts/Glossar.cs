using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using TopoSort;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Glossar : MonoBehaviour
{
    private Dictionary<string, GlossarEntry> Entries = new Dictionary<string, GlossarEntry>();
    private static Glossar Instance;
    private GameObject InfoText, TitleText, Synonyms;
    
    private string[] Terms =
    {
        "Graph",
        "Knoten",
        "Kante",
        "Knotengrad",
        "Sortierung",
        "Algorithmus",
        "Kahn Algorithmus"
    };

    // just there to fill the entry with test values
    private void LoadEntries()
    {
        for (int i = 0; i < Terms.Length; i++)
        {
            string[] syns = new string[Terms.Length - i];
            for (int j = 0; j < syns.Length; j++ )
            {
                syns[j] = "" + j;
            }

            string expl = "" + TestFunc((i + 1));
            
            
            GlossarEntry entry = new GlossarEntry(Terms[i], syns, expl);

            Debug.Log(string.Format("Added Entry: [{0}, {1}]", Terms[i], expl));
            
            Entries.Add(Terms[i], entry);
            
        }
    }

    
    // just there to fill the explanation with some test values
    private int TestFunc(int i)
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
    
    
    // Start is called before the first frame update
    void Start()
    {
        Glossar.Instance = this;
        LoadEntries();
        
        InfoText = this.transform.Find("Infotext").gameObject;
        TitleText = this.transform.Find("Title").gameObject;
        Synonyms = this.transform.Find("Synonyms").gameObject;
    }

    public void SetTexts(string key)
    {
        TitleText.GetComponent<TextMeshProUGUI>().text = this.Entries[key].GetTitle();
        InfoText.GetComponent<TextMeshProUGUI>().text = this.Entries[key].GetExplanation();

        string syns = "";

        for (int i = 0; i < this.Entries[key].GetSynonyms().Length; i++)
        {
            if (i > 0)
            {
                syns += ", ";
            }

            syns += this.Entries[key].GetSynonyms()[i];
        }

        Synonyms.GetComponent<TextMeshProUGUI>().text = syns;
    }

    public static Glossar GetInstance()
    {
        return Glossar.Instance;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
