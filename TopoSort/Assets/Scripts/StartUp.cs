using UnityEngine;
using UnityEditor;
using TopoSort;
[InitializeOnLoad]
public class StartUp{
    static StartUp()
    {
        var persistanceUtil = new PersistanceUtility();
        persistanceUtil.RestoreLog();
    }
}