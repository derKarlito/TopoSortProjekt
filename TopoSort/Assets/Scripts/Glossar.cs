using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TopoSort;

public class Glossar : MonoBehaviour
{
    private Dictionary<string, GlossarEntry> Entries = new Dictionary<string, GlossarEntry>();

    private string[] Terms =
    {
        "Graph",
        "Knoten",
        "Kanten",
        "gerichteter Graph",
        "gerichtete Kanten",
        "Topologische Sortierung"
    };

    // just there to fill the entry with test values
    private void loadEntries()
    {
        for (int i = 0; i < Terms.Length; i++)
        {
            string[] syns = new string[Terms.Length - i];
            for (int j = 0; j < syns.Length; j++ )
            {
                syns[j] = "" + j;
            }

            string expl = "" + testFunc((i + 1));
            
            
            GlossarEntry entry = new GlossarEntry(Terms[i], syns, expl);

            Debug.Log(string.Format("Added Entry: [{0}, {1}]", Terms[i], expl));
            
            Entries.Add(Terms[i], entry);
        }
    }

    
    // just there to fill the explanation with some test values
    private int testFunc(int i)
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
        loadEntries();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
