using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using UnityEngine.Rendering;

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
    }

    // Update is called once per frame
    void Update()
    {
        bool onButton = MouseManager.MouseHover(Collider);
        if(Input.GetMouseButtonDown(0) && onButton)
        {
            isActive = !isActive;
            if(isActive)
            {
                StartTutorial();
            }
            else
            {
                StopTutorial();
            }
        }
    }

    public void StartTutorial()
    {
        DarkCanvas.gameObject.SetActive(true);
        TutorialUpdate();
    }

    public void StopTutorial()
    {
        DarkCanvas.gameObject.SetActive(false);
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

    private void LoadTexts()
    {
        TutorialTexts = new string[]
        {   
            "Knoten:\nHier siehst du einen Knoten. Diesen findest du links im Inventar.\nEs gibt 5 Knotenarten:\n\t- Wasser\n\t- Vegetation/Pflanzen\n\t- Atmosphäre\n\t- Plattentektonik\n\t- Mond\nAlle Knoten haben in bestimmten Kombinationen verschiedene Veränderungen des Planeten zur Folge.\n\nZum Anordnen der Knoten, musst du Ihn nur in den dafür vorgesehenen Graph-Editor mit einem Linksklick ziehen.\nPer gezogenem Rechtsklick können dann Knoten untereinander verbunden werden.\nSobald die Knoten angeordnet und miteinander verbunden sind, wirst du in der oberen rechten Ecke des Knotens den Eingangsgrad sehen.",
            "Inventar:\nIm Inventar findest du alle vorhandenen Knotenarten, zum Bauen deines Graphen.\nNatürlich kannst du jeden Knoten beliebig oft nutzen.\nDie Zahl an der Seite drückt lediglich aus, wie viele 'Punkte' dein Planet von dieser Knotenart bekommt\n\nUm einen Knoten aus dem Inventar zu verwenden, musst du ihn einfach aus dem Inventar, per Drag and Drop, in den Graph-Editor ziehen.\n um ihn zu Löschen ziehst du ihn einfach wieder zurück ins Inventar.",
            "Steuerung:\nDas Steuerelement für den Algorithmus.\nHier hast du 3 Funktionen zur Auswahl:\n     - Starten/Stoppen des Algorithmus (Play)\n     - Schritt überspringen (Pfeil nach rechts)\n     - Schritt wiederholen (Pfeil nach links)\n\nSobald du deinen Graph gebaut hast, kannst du den Start-Button betätigen.\nDer Algorithmus wird den Graph durchlaufen und die topologisch sortierte Reihenfolge deiner Knoten herstellen.\n\nDie einzelnen Schritte werden dir in der Konsole ausgegeben.\n", //Solltest du einmal nicht mitkommen oder du möchtest du dir nur einen bestimmten \nSchritt noch einmal ansehen, kannst du dich mittels der Pfeil-Buttons durch die \neinzelnen Schritte klicken. \n\nBeachte, dass der Algorithmus automatisch gestoppt wird, wenn du einen oder mehrere \nSchritte wiederholen oder übrspringen willst.\nZudem wird der Algorithmus abgebrochen, sobald man einen weitern Knoten platziert, solange \nder Algorithmus läuft bzw. pausiert ist",
            "Graph-Editor:\nHier siehst du die Hilfsoberfläche, in welcher du deinen Graphen bauen kannst.\nZum Bauen deines Graphen, musst du dir ein paar Knoten aus dem Inventar in den Editor ziehen.\nDanach kannst du diese per Rechtsklick miteinander verbinden.\nUm deinen Graphen etwas strukturierter anzuordnen, sind dir 3 Hilfslinien gegeben.\n\nAchte darauf, keine Zyklen in deinen Graphen einzuarbeiten!\nSobald du den Algorithmus durch deinen Graphen laufen lässt, wirt du anhand von farbigen Markierungen sehen, an welchem Knoten sich der Algorithmus gerade befindet und zu welchem Knoten er als nächstes geht. Zudem wird markiert, welche Knoten bereits besucht und ausgegeben wurden, oder eben noch nicht.",
            "Konsole:\nIn der Konsole wird dir der momentane Zustand des Planeten ausgegeben. \nDieser aktualiesiert sich mit jedem Schritt, den der Algorithmus durchführt.\nDie einzelnen Schritte, wie sich der Planet verändert, werden ebenfalls dokumentiert.\n\nDie topologisch sortierte Reihenfolge, der Knoten deines Graphen, wird sowohl am \nEnde als, auch während des Durchlaufens festgehalten.",
            "Einstellungen:\nÜber der Konsole befindet sich eine Reihe an Steuerelementen.\nNeben den Buttons zur Steuerung der Audio-Ausgabe und Starten/Beenden des Tutorials, findest\ndu auch ein Nachschlagewerk, sowie einen Knopf zum Umschalten der Sprache des Spiels",
            "Dynamischer Planet:\nHier befindet sich der Planet, welcher sich in Abhängigkeit deines Graphen\ndynamisch beim Durchlaufen verändert.\nDas bedeutet, dass die Reihenfolge und Häufigkeit der Knoten eine Auswirkung auf den\nPlaneten und seine äußere Erscheinungsform hat.\n\nDer Planet kann sich dabei in seiner Grundzusammensetzung (Lava-Planet, Wasser-Planet,...) als\nauch in seinen atmosphärischen Eigenschaften (Sandstürme, dünne Atmosphäre,...) verändern.\nZudem können Monde in der Umlaufbahn des Planeten erscheinen.\n"
        };
    }

    private void DetermineScreenHighlights()
    {
        switch (SlideNumber)
        {
            case 0:
            case 1:
                for(int i = 0; i < HighlightedObjects.Length; i++)
                {
                    if(i==2)
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
                break;
            case 2:
                for(int i = 0; i < HighlightedObjects.Length; i++)
                {
                    if(i==0)
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
                break;
            case 3:
                for(int i = 0; i < HighlightedObjects.Length; i++)
                {
                    if(i==3)
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
                break;

            case 4:
                for(int i = 0; i < HighlightedObjects.Length; i++)
                {
                    if(i==4)
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
                break;    
            case 5:
                for(int i = 0; i < HighlightedObjects.Length; i++)
                {
                    if(i==1)
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
                break;
            case 6:
                for(int i = 0; i < HighlightedObjects.Length; i++)
                {
                    if(i==5)
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
                break;
            default:
                break;
        }
    }
}
