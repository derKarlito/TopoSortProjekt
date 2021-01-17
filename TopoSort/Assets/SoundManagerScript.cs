using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playButtonSound, forthButtonSound, backButtonSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        playButtonSound = Resources.Load<AudioClip>("Sounds/playButton");
        forthButtonSound = Resources.Load<AudioClip>("Sounds/forthButton");
        backButtonSound = Resources.Load<AudioClip>("Sounds/backButton");

        audioSrc = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        switch(clip)
        {
            case "playButton":
            audioSrc.PlayOneShot (playButtonSound);
            break;
            case "forthButton":
            audioSrc.PlayOneShot (forthButtonSound);
            break;
            case "backButton":
            audioSrc.PlayOneShot (backButtonSound);
            break;
        }
    }
}
