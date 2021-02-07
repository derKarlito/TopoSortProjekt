using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using UnityEngine.Rendering;
using TopoSort;

public class Tutorial : MonoBehaviour
{
    public Collider2D Collider;

    public DarkCanvas DarkCanvas;

    public TextMeshProUGUI TextField;

    public bool isActive;

    public bool TutorialStarted = false;

    public string[] TutorialTexts;

    public GameObject[] HighlightedObjects;

    public int SlideNumber = 0;

    public enum Sequence
    {
        Nodes,
        Inventory,
        Controls,
        Editor,
        Console,
        Settings,
        Planet,
        Count
    }

    public enum ObjectSequence
    {
        Controls,
        Settings,
        Inventory,
        Graph,
        Console,
        Planet
    }

    //these are fake as of now; check the editor values for the real values. idk Unity is weird with serialization. Dont @me
    public Vector4[] marginDimensions = new []
    {
        //Nodes
        new Vector4(320f, 69f, 320f, 132f),
        //Inventory
        new Vector4(320f, 69f, 649.3f, 323.2f),
        //Controls
        new Vector4(302.2f, 188.7f, 261f, 342.9f),
        //Editor
        new Vector4(385.7f, 48.1f, 174.3f, 346.1f),
        //Console
        new Vector4(447.9f, 202.4f, 611.9f, 132f),
        //Settings
        new Vector4 (1126.2f, 219.1f, 105.9f, 132f),
        //Planet
        new Vector4(1047f, 74f, 87f, 36f)
    };

    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponentInChildren<Collider2D>();
        DarkCanvas = DarkCanvas.Instance;
        LoadTexts();
        HighlightedObjects = GameObject.FindGameObjectsWithTag("Highlighted");
        for(int i = 0; i < HighlightedObjects.Length; i++)
        {
            Debug.Log(HighlightedObjects[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool onButton = MouseManager.MouseHover(Collider);
        if(Input.GetMouseButtonDown(0) && onButton)
        {

            if(!isActive)
            {
                LoadTexts();
                StartTutorial();
            }
            else
            {
                StopTutorial();
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = false;
            StopTutorial();
        }
    }

    public void StartTutorial()
    {
        DarkCanvas.gameObject.SetActive(true);
        isActive = true;
        TutorialUpdate();
    }

    public void StopTutorial()
    {
        DarkCanvas.gameObject.SetActive(false);
        isActive = false;
        TextField.text = String.Empty;
    }

    public void NextSlide()
    {
        SlideNumber++;
        if(SlideNumber >= TutorialTexts.Length)
        {
            SlideNumber = 0;
        }
        TutorialUpdate();
    }

    public void PrevSlide()
    {
        SlideNumber--;
        if(SlideNumber < 0)
        {
            SlideNumber = TutorialTexts.Length-1;
        }
        TutorialUpdate();
    }

    public void TutorialUpdate()
    {
        TextField.margin = marginDimensions[SlideNumber];
        TextField.text = TutorialTexts[SlideNumber];

        DetermineScreenHighlights();

        Debug.Log(SlideNumber);
    }

    private void  LoadTexts()
    {
            TextLoader loaderGerman = new TextLoader("xml/Tutorial_deutsch");
            TextLoader loaderEnglish = new TextLoader("xml/Tutorial_englisch");
            
            if (Localisation.isGermanActive)
            {
                TutorialTexts = new string[]
                {   
                    loaderGerman.LoadTutorialPage(1),
                    loaderGerman.LoadTutorialPage(2),
                    loaderGerman.LoadTutorialPage(3),
                    loaderGerman.LoadTutorialPage(4),
                    loaderGerman.LoadTutorialPage(5),
                    loaderGerman.LoadTutorialPage(6),
                    loaderGerman.LoadTutorialPage(7)
                };
            }
            else
            {
                TutorialTexts = new string[]
                {   
                    loaderEnglish.LoadTutorialPage(1),
                    loaderEnglish.LoadTutorialPage(2),
                    loaderEnglish.LoadTutorialPage(3),
                    loaderEnglish.LoadTutorialPage(4),
                    loaderEnglish.LoadTutorialPage(5),
                    loaderEnglish.LoadTutorialPage(6),
                    loaderEnglish.LoadTutorialPage(7),
                };  
            }

    }

    private void DetermineScreenHighlights()
    {
        int litIndex = -1;
        
        
        switch (SlideNumber)
        {
            case 0:
            case 1: //talk abt nodes + inventory
                litIndex = 1;
                break;
            case 2: //Control
                litIndex = 4;
                break;
            case 3: //Editor
                litIndex = 2;
                break;
            case 4: //Console
                litIndex = 3;
                break;
            case 5: //Settings
                litIndex = 0;
                break;
            case 6: //Planet
                litIndex = 5;
                break;
            default:
                break;
        }

        for(int i = 0; i < HighlightedObjects.Length; i++)
        {
            if(i==litIndex)
            {
                SortingGroup sortingGroup =  HighlightedObjects[i].GetComponent<SortingGroup>();
                sortingGroup.sortingLayerName = "Highlighted";
            }
            else
            {
                SortingGroup sortingGroup =  HighlightedObjects[i].GetComponent<SortingGroup>();
                sortingGroup.sortingLayerName = "Default";
            }
        }
        
        
    }
}
