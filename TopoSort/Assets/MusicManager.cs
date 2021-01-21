using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MusicManager : MonoBehaviour
{
    public Sprite[] ButtonSprites;
    public SpriteRenderer Speaker;
    public Collider2D Collider;
    public static AudioSource Music;

    bool Play;
    bool ToggleChange;  //makes sure music isn't played multiple times

    int SpeakIndex = 0;
    int MuteIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        ButtonSprites = Resources.LoadAll<Sprite>("Sprites/volume_sprites");
        Speaker = GetComponent<SpriteRenderer>();
        Collider = GetComponent<Collider2D>();
        Music = GetComponent<AudioSource>();

        Play = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool onButton = MouseManager.MouseHover(Collider);
        if(Input.GetMouseButtonUp(0) && onButton && ToggleChange && Play)
        {
            ToggleChange = false;
            Music.Pause();
            Speaker.sprite = ButtonSprites[MuteIndex];
            Play = false;
        }
        if(Input.GetMouseButtonUp(0) && onButton && ToggleChange && !Play)
        {
            ToggleChange = false;
            Music.UnPause();
            Speaker.sprite = ButtonSprites[SpeakIndex];
            Play = true;
        }
        ToggleChange = true;
    }
    void OnGUI() 
    {
        
    }
}
