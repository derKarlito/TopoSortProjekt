using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlossarEntryButton : MonoBehaviour
{
    private string Text;
    private Button ButtonComponent;
    private TextMeshProUGUI Content;
    
    public Color EnterColor = new Color(1.0f, 1.0f, 1.0f);
    public Color ExitColor = new Color(0.75f, 0.75f, 0.75f);
    
    // Start is called before the first frame update
    void Start()
    {
        Content = this.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        ButtonComponent = this.GetComponent<Button>();
        ButtonComponent.onClick.AddListener(OnClick);

        EventTrigger.Entry enterEvent = new EventTrigger.Entry();
        enterEvent.eventID = EventTriggerType.PointerEnter;
        enterEvent.callback.AddListener(ButtonEnter);
        
        EventTrigger.Entry exitEvent = new EventTrigger.Entry();
        exitEvent.eventID = EventTriggerType.PointerExit;
        exitEvent.callback.AddListener(ButtonExit);

        this.gameObject.AddComponent<EventTrigger>();
        EventTrigger trigger = this.gameObject.GetComponent<EventTrigger>();

        trigger.triggers.Add(enterEvent);
        trigger.triggers.Add(exitEvent);

        Text = Content.text;
    }
    
    // Update is called once per frame
    void Update()
    {
        Content.text = GlossarCanvas.getEntry(Text).GetTitle();
    }

    public void ButtonEnter(BaseEventData data)
    {
        Content.color = EnterColor;
    }
    
    public void ButtonExit(BaseEventData data)
    {
        Content.color = ExitColor;
    }
    
    public void OnClick()
    {
        GlossarCanvas.GetInstance().SetTexts(Text);
    }
}
