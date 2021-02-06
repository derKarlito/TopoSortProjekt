using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
        if(PersistanceObject.nextIndex > 10){
            PersistanceObject.graphList.RemoveAt(0);
            PersistanceObject.PlanetName.RemoveAt(0);
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
        string nodes = PersistanceObject.graphList[PersistanceObject.nextIndex-1].toString();

        //uses that information to write that into the log
        logImage.sprite = Planet.GetPlanetSprite(planetState);
        logText.text = ($"Planet: {planetState}\nNodes: {nodes}");
        
    }

    public Image CreateLogImage()
    {
        var imageprefab = Resources.Load<Image>("Models/LogImage");
        var image = UnityEngine.Object.Instantiate(imageprefab);
        image.transform.SetParent(ImageTransform, false); 
        return image;
    }

    public TextMeshProUGUI CreateLogText()
    {
        var textprefab = Resources.Load<TextMeshProUGUI>("Models/LogText");
        var text = UnityEngine.Object.Instantiate(textprefab);
        text.transform.SetParent(TextTransform, false); 
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