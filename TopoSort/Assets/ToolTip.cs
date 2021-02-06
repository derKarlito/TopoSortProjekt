using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTip : MonoBehaviour
{
    public Collider2D Collider;
    public TextMeshPro ToolTipDisplay;
    public string ToolTipText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MouseManager.MouseHover(Collider))
        {
            SetToolTip(ToolTipText);
        }
        else
        {
            DeleteToolTip();
        }
    }

    public void SetToolTip(string tip)
    {
        string german = default;
        ToolTipDisplay.transform.position = MouseManager.GetMousePos().Z(-2).Y(4.75f);

        if(Localisation.isGermanActive)
        {
            Localisation.Translator.TryGetValue(tip, out german);
            ToolTipDisplay.text = german;       
        }
        else
        {
            ToolTipDisplay.text = tip;
        }
    }

    public void DeleteToolTip()
    {
        ToolTipDisplay.text = "";
    }
}
