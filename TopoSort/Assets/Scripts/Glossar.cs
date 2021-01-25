using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glossar : MonoBehaviour
{


    private Collider2D GlossarButtonCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        GlossarButtonCollider = this.GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool onButton = MouseManager.MouseHover(GlossarButtonCollider);
        if(Input.GetMouseButtonDown(0) && onButton)
        {
            GlossarCanvas.GetInstance().Show();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GlossarCanvas.GetInstance().Hide();
        }
    }
}
