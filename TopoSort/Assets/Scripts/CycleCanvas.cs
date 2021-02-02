using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * simple singleton to toggle the cycle dialog on or off
 */
public class CycleCanvas : MonoBehaviour
{
    private static CycleCanvas Instance;
    
    private bool LastLanguage;

    public TextMeshProUGUI Content;
    
    public static CycleCanvas getInstance()
    {
        return CycleCanvas.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        CycleCanvas.Instance = this;
        this.Hide();

        LastLanguage = Localisation.isGermanActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (LastLanguage == Localisation.isGermanActive)
        {
            setText();
        }
    }


    public void setText()
    {
        LastLanguage = Localisation.isGermanActive;
        if (LastLanguage)
        {
            Content.text = "Der Graph enthält einen Zyklus.";
        }
        else
        {
            Content.text = "The graph contains a cycle.";
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
