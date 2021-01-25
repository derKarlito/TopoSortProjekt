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
        
        
        
        // 0 - Graph Entry
        entryGerman = new GlossarEntry();
        entryEnglish = new GlossarEntry();
        
        entryGerman.SetTitle("Graph");
        entryGerman.GetSynonyms().Add("Digraph (gerichtet)");
        entryGerman.SetExplanation("Ein Graph ist eine aus Knoten und Kanten bestehende Struktur.\n" +
                                   "Weisen alle Kanten eine Orientierung auf, so ist dieser Graph \"gerichtet\"");
        
        entryEnglish.SetTitle("Graph");
        entryEnglish.GetSynonyms().Add("Digraph (directed)");
        entryEnglish.SetExplanation("A mathematical structure consisting of nodes and edges\n" +
                                   "If every edge has an orientation the Graph is called \"directed\"");
        
        EntriesGerman.Add(TermsGerman[0], entryGerman);
        EntriesEnglish.Add(TermsGerman[0], entryEnglish);
        
        
        // 1 - Vertex Entry
        entryGerman = new GlossarEntry();
        entryEnglish = new GlossarEntry();
        
        entryGerman.SetTitle("Knoten");
        entryGerman.GetSynonyms().Add("Vertex");
        entryGerman.SetExplanation("Bestandteil eines Graphen, welcher meißt einen Namen oder einen Wert besitzt\n");
        
        entryEnglish.SetTitle("Node");
        entryEnglish.GetSynonyms().Add("Vertex");
        entryEnglish.SetExplanation("A part of a graph. Usually it has a name or value associated with it.\n");
        
        EntriesGerman.Add(TermsGerman[1], entryGerman);
        EntriesEnglish.Add(TermsGerman[1], entryEnglish);
        
        
        // 2 - Edge Entry
        entryGerman = new GlossarEntry();
        entryEnglish = new GlossarEntry();
        
        entryGerman.SetTitle("Kante");
        entryGerman.GetSynonyms().Add("Bogen (gerichtet)");
        entryGerman.SetExplanation("Ein Bestandteil eines Graphen, welcher zwei Knoten miteinander verbindet\n" +
                                   "Eine Kante, die eine eindeutige Richtung aufweist (eine Orientierung hat)\n" +
                                   "nennt man \"gerichtet\" und werden meist als \"Bogen\" bezeichnet");
        
        entryEnglish.SetTitle("Edge");
        entryEnglish.GetSynonyms().Add("Arc (directed)");
        entryEnglish.GetSynonyms().Add("Arrow (directed)");
        entryEnglish.SetExplanation("A part of a graph, which connects two nodes with each other\n" +
                                    "An edge that has a defined orientation is called \"directed\" and is often referred to as an \"arc\"");
        
        EntriesGerman.Add(TermsGerman[2], entryGerman);
        EntriesEnglish.Add(TermsGerman[2], entryEnglish);
        
        
        // 3 - Degree Entry
        entryGerman = new GlossarEntry();
        entryEnglish = new GlossarEntry();
        
        entryGerman.SetTitle("Knotengrad");
        entryGerman.SetExplanation("Anzahl aller Kanten, die inzident (anliegend) an einen Knoten sind.\n" +
                                   "\n" +
                                   "In einem gerichteten Graphen:\n" +
                                   "- Die Anzahl der Bögen, die in einen Knoten eingehen nennt man \"Innengrad\"\n" +
                                   "- Die Anzahl der Bögen, die von einem Knoten ausgehen nennt man \"Außengrad\"");
        
        entryEnglish.SetTitle("Degree");
        entryEnglish.SetExplanation("The number of edges that are incident to a node\n" +
                                   "\n" +
                                   "In a directed graph:\n" +
                                   "- The number of arcs entering a node is called \"in-degree\"\n" +
                                   "- The number of arcs exiting a node is called \"out-degree\"");
        
        EntriesGerman.Add(TermsGerman[3], entryGerman);
        EntriesEnglish.Add(TermsGerman[3], entryEnglish);
        
        
        
        // 4 - Sorting Entry
        entryGerman = new GlossarEntry();
        entryEnglish = new GlossarEntry();
        
        entryGerman.SetTitle("Sortierung");
        entryGerman.SetExplanation("Eine mit einer bestimmten Ordnung versehenen Folge von Objekten.\n" +
                                   "\n" +
                                   "Topologische Sortierung:\n" +
                                   "Eine Sortierung, welche Objekte nach ihren Abhängigkeiten zueinander ordnet.");
        
        entryEnglish.SetTitle("Sorting");
        entryEnglish.SetExplanation("A sequence of objects in a certain order.\n" +
                                    "\n" +
                                    "topological sort:" +
                                    "A sorting which sorts objects according to their dependencies on each other.");
        
        EntriesGerman.Add(TermsGerman[4], entryGerman);
        EntriesEnglish.Add(TermsGerman[4], entryEnglish);
        
        
        
        // 5 - Algorithm
        entryGerman = new GlossarEntry();
        entryEnglish = new GlossarEntry();
        
        entryGerman.SetTitle("Algorithmus");
        entryGerman.SetExplanation("Endliche Folge von Handlungsschritten zur Lösung eines Problems oder einer Problemklasse.\n");
        
        entryEnglish.SetTitle("Algorithmn");
        entryEnglish.SetExplanation("A finite sequence of steps to solve a problem or a problem class");
        
        EntriesGerman.Add(TermsGerman[5], entryGerman);
        EntriesEnglish.Add(TermsGerman[5], entryEnglish);
        
        
        
        // 6 - Kahn Algorithm
        entryGerman = new GlossarEntry();
        entryEnglish = new GlossarEntry();
        
        entryGerman.SetTitle("Kahn-Algorithmus");
        entryGerman.SetExplanation("Algorithmus, der ein einem azyklischen, gerichteten Graphen eine topologische Sortierung auf den Knoten ausführt.\n" +
                                   "Ein Knoten wird immer nur dann in die Ausgabe geschrieben, wenn dessen Vorgänger bereits abgearbeitet wurden.\n" +
                                   "(Und der Algorithmus, der diese Simulation antreibt.)");
        
        entryEnglish.SetTitle("Kahn's Algorithmn");
        entryEnglish.SetExplanation("An algorithm that performs a topological sorting on the nodes of an acyclic, directed graph.\n" +
                                    "A node is only written to the output if its ancestor has already been processed.\n" +
                                    "(and the algorithm that drives this simulation.)");
        
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
