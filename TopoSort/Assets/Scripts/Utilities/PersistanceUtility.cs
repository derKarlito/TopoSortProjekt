using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Newtonsoft.Json;
using TopoSort;
using TMPro;


public class PersistanceUtility : MonoBehaviour
{
    public PersistantObject PersistanceObject = new PersistantObject();
    public Planet Planet;
    public Transform TextTransform;
    public Transform ImageTransform;

    public void AddLogEntry(Planet planet, Graph graph)
    {  
        var tempGraph = new Graph(graph.Nodes);
        PersistanceObject.graphList.Insert(PersistanceObject.nextIndex,graph);
        PersistanceObject.PlanetName.Insert(PersistanceObject.nextIndex, planet.State);
        PersistanceObject.nextIndex++;
        if(PersistanceObject.nextIndex > 6){
            //UpdateLogList();
            PersistanceObject.graphList.RemoveAt(0);
            PersistanceObject.PlanetName.RemoveAt(0);
            PersistanceObject.nextIndex = 0;
        }
        WriteFile();
    }

    public void WriteFile()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),"aplicationData.json" );
        var json = JsonConvert.SerializeObject(PersistanceObject, Formatting.Indented,
            new JsonSerializerSettings() {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }
        );
        File.WriteAllText(path,json);
        RestoreLog();
    }

    public void RestoreLog()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),"aplicationData.json" );
        if(File.Exists(path))
        {
            var data = string.Empty;
            PersistantObject persistantObject = null;
            data = File.ReadAllText(path);
            persistantObject = JsonConvert.DeserializeObject<PersistantObject>(data); 
            if (persistantObject != null)
            {
                PersistanceObject = persistantObject;
            }
        }
        if(PersistanceObject != null)
        {
            WriteToLog();
        }
    }

    public void WriteToLog() //DONT FORGET LOCALISATZIONS
    {
        // Creates new gameobjects that act as the visible Log Entries
        var logImage = CreateLogImage();
        var logText = CreateLogText();

        //Gets the planet name and the nodes it took to get there
        string planetState = PersistanceObject.PlanetName[PersistanceObject.nextIndex-1];
        
        //string nodes = PersistanceObject.graphList[PersistanceObject.nextIndex-1].toString();

        //uses that information to write that into the log
        logImage.sprite = Planet.GetPlanetSprite(planetState);
        if(Localisation.isGermanActive) //localisation of the planet name. Nodenames get localised in the toStroing() method called earlier
        {
            Localisation.Translator.TryGetValue(planetState, out var german);
            planetState = german;
        }
        //logText.text = ($"Planet: {planetState}\nNodes: {nodes}");
        
    }

    private void OnEnable() 
    {
        ConsiderLanguage();
    }

    public void ConsiderLanguage()
    {
        for(int i = 0; i < TextTransform.childCount; i++)
        {
            // Cut up the single parts of the log entry
            var text = TextTransform.GetChild(i).GetComponent<TextMeshProUGUI>();  // text.text smth like $"Planet: {planetState}\nNodes: {nodes}"
            var split = text.text.Split('\n'); //splits once at the newline
            var planetName = split[0].Split(' ')[1]; // takes the first line and splits it at the space then takes the latter part the "{planetstate}"
            var nodeList = split[1].Split(' '); // the second line of a log entry has multiple spaces so we just put that all into a new array called nodeList (Note that nodeList[0] will be the normal declaration of "Nodes:")
            
            if(Localisation.isGermanActive)
            {
                if(!Localisation.Translator.ContainsKey(planetName)) //then it already is german and we can skip
                    break;
                Localisation.Translator.TryGetValue(planetName, out var germanPlanet);
                var germanNodes = nodeList;
                var germanNodeLine = "";
                for(int j = 1; j < nodeList.Length; j+=2) //Takes two steps bc every other array place is '+'
                {
                    Localisation.Translator.TryGetValue(planetName, out germanNodes[j]);
                    germanNodeLine += germanNodes[j] + " + ";
                }
                germanNodeLine = germanNodeLine.Remove(germanNodeLine.Length-3);
                
                text.text = ($"Planet: {germanPlanet}\nNodes: {germanNodeLine}");
            }
            else
            {
                if(Localisation.Translator.ContainsKey(planetName))
                    break;
                var englishPlanet = "";
                foreach (KeyValuePair<string, string> dictEntry in Localisation.Translator)
                {
                    if(dictEntry.Value == planetName)
                        englishPlanet = dictEntry.Key;
                }
                var englishNodes = nodeList;
                var englishNodeLine = "";
                for(int j = 1; j < nodeList.Length; j += 2)
                {
                    foreach (KeyValuePair<string, string> dictEntry in Localisation.Translator)
                    {
                        if(dictEntry.Value == englishNodes[j])
                            englishNodeLine += dictEntry.Key + " + ";
                    }
                }
                englishNodeLine = englishNodeLine.Remove(englishNodeLine.Length-3);
                text.text = ($"Planet: {englishPlanet}\nNodes: {englishNodeLine}");
            }
            
        }
    }

    public void UpdateLogList()
    {
        //0 kommt raus
        //everrything else moves one up
        for(int i = 1; i < 6; i++)
        {
            var prevEntryImage = ImageTransform.GetChild(i-1).gameObject;
            var currentEntryImage = ImageTransform.GetChild(i).gameObject;
            prevEntryImage = currentEntryImage;
        }
        PersistanceObject.nextIndex = 6;
    }

    public Image CreateLogImage()
    {
        Image image;

        if(ImageTransform.childCount < 6 ) //check to see if we already have a gameobject we can fill or if we need to instantiate a new one if true
        {
            var imageprefab = Resources.Load<Image>("Models/LogImage");
            image = UnityEngine.Object.Instantiate(imageprefab);
            image.transform.SetParent(ImageTransform, false); 
            image.tag = ("Log"+(PersistanceObject.nextIndex-1).ToString());
        }
        else
        {
            image = ImageTransform.GetChild(PersistanceObject.nextIndex).GetComponent<Image>();
        }
        return image;
    }

    public TextMeshProUGUI CreateLogText()
    {
        TextMeshProUGUI text;
        if(TextTransform.childCount < 6 )
        {
            var textprefab = Resources.Load<TextMeshProUGUI>("Models/LogText");
            text = UnityEngine.Object.Instantiate(textprefab);
            text.transform.SetParent(TextTransform, false); 
            text.tag = ("Log"+(PersistanceObject.nextIndex-1).ToString());
        }
        else
        {
            text = TextTransform.GetChild(PersistanceObject.nextIndex).GetComponent<TextMeshProUGUI>();
        }
        return text;
    }
}

public static class FileUtil
{
    public static async Task<string> ReadAllTextAsync(string filePath)
    {
        var stringBuilder = new StringBuilder();
        using (var fileStream = File.OpenRead(filePath))
        using (var streamReader = new StreamReader(fileStream))
        {
            string line = await streamReader.ReadLineAsync();
            while(line != null)
            {
                stringBuilder.AppendLine(line);
                line = await streamReader.ReadLineAsync();
            }
            return stringBuilder.ToString();
        }
    }
}