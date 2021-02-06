using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TopoSort;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Log : MonoBehaviour
{
    public static TextMeshProUGUI LogText;
    public static PersistanceUtility PersistanceUtility = new PersistanceUtility();
    
    
    void Awake()
    {
        PersistanceUtility.RestoreLog();       
    }

    
}