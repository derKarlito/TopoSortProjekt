using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCanvas : MonoBehaviour
{
    public static DarkCanvas Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void OnDestroy() 
    {
        Instance = null;
    }
}
