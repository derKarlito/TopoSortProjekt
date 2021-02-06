using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public Collider2D Collider;

    // Start is called before the first frame update
    void Start()
    {
        Collider = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool onButton = MouseManager.MouseHover(Collider);        
        if (Input.GetMouseButtonUp(0) && onButton)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
