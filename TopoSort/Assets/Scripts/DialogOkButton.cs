using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogOkButton : MonoBehaviour
{
    public Button ButtonComponent;
    public TextMeshProUGUI Content;
    
    public Color HoverColor = new Color(1.0f, 1.0f, 1.0f);
    public Color DefaultColor = new Color(0.75f, 0.75f, 0.75f);

    
    // Start is called before the first frame update
    void Start()
    {
        Content = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        ButtonComponent = GetComponent<Button>();
        ButtonComponent.onClick.AddListener(OnClick);
        
        EventTrigger.Entry enterEvent = new EventTrigger.Entry();
        enterEvent.eventID = EventTriggerType.PointerEnter;
        enterEvent.callback.AddListener(EnterButton);
        
        EventTrigger.Entry exitEvent = new EventTrigger.Entry();
        exitEvent.eventID = EventTriggerType.PointerExit;
        exitEvent.callback.AddListener(ExitButton);

        this.gameObject.AddComponent<EventTrigger>();
        EventTrigger trigger = this.gameObject.GetComponent<EventTrigger>();

        trigger.triggers.Add(enterEvent);
        trigger.triggers.Add(exitEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterButton(BaseEventData eventData)
    {
        Content.color = HoverColor;
    }

    public void ExitButton(BaseEventData data)
    {
        Content.color = DefaultColor;
    }
    
    
    public void OnClick()
    {
        CycleCanvas.getInstance().Hide();
    }
}
