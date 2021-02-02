using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Models;
using Newtonsoft.Json;
using TopoSort;


public class PersistanceUtility{
    private PersistantObject persistanceObject = new PersistantObject();


    public void AddLogEntry(Planet planet, Graph graph)
    {  
        var tempGraph = new Graph(graph.Nodes);
        persistanceObject.graphList.Insert(persistanceObject.nextIndex,graph);
        persistanceObject.planetList.Insert(persistanceObject.nextIndex,planet.CreatedPlanet);
        persistanceObject.nextIndex++;
        if(persistanceObject.nextIndex > 10){
            persistanceObject.graphList.RemoveAt(0);
            persistanceObject.planetList.RemoveAt(0);
        }
        WriteFile();
    }

    public void WriteFile()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),"aplicationData.json" );
        var json = JsonConvert.SerializeObject(persistanceObject, Formatting.Indented,
            new JsonSerializerSettings() {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }
        );
        Task.Run(() => {
            File.WriteAllText(path,json);
        });
        RestoreLog();
    }

    public async void RestoreLog()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),"aplicationData.json" );
        if(File.Exists(path))
        {
            var data = string.Empty;
            PersistantObject persistantObject = null;
            await Task.Run(async() => {
                data = await FileUtil.ReadAllTextAsync(path);
                persistantObject = JsonConvert.DeserializeObject<PersistantObject>(data);
            });
            if (persistantObject != null){
                persistanceObject = persistantObject;
            }
        }
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