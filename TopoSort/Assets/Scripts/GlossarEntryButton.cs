using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlossarEntryButton : MonoBehaviour
{
    private string Text;
    
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        string t = this.GetComponentInChildren<TextMeshProUGUI>().text;
        Glossar.GetInstance().SetTexts(t);
    }
}
