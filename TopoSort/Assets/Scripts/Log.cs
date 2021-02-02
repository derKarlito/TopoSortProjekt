using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TopoSort;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Log : MonoBehaviour
{
    PersistanceUtility persistanceUtility = new PersistanceUtility();
    void Awake()
    {
        persistanceUtility.RestoreLog();       
    }
    void Start()
    {
        var lol = "test";
    }

    void Update()
    {

    }
}