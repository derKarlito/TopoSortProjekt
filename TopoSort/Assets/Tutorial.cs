using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Collider2D Collider;

    public DarkCanvas DarkCanvas;

    public bool isActive = true;

    public bool TutorialStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponentInChildren<Collider2D>();
        DarkCanvas = DarkCanvas.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        bool onButton = MouseManager.MouseHover(Collider);
        if(Input.GetMouseButtonDown(0) && onButton)
        {
            isActive = !isActive;
            DarkCanvas.gameObject.SetActive(isActive);
            if(isActive)
            {
                TutorialStarted = true;
            }
        }
        if(TutorialStarted)
        {
            StartTutorial();
        }
        TutorialStarted = false;
    }

    public void StartTutorial()
    {
        Debug.Log("Tutorial started");
    }
}
