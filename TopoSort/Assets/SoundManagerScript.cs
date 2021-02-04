using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playButtonSound, forthButtonSound, backButtonSound, errorSound, atmosphereButtonSound, plantButtonSound, groundButtonSound, waterButtonSound, moonButtonSound, buttonSound;
    public static AudioSource AudioSrc;

    // Start is called before the first frame update
    void Start()
    {
        playButtonSound = Resources.Load<AudioClip>("Sounds/playButton");
        forthButtonSound = Resources.Load<AudioClip>("Sounds/forthButton");
        backButtonSound = Resources.Load<AudioClip>("Sounds/backButton");
        errorSound = Resources.Load<AudioClip>("Sounds/errorSound");
        atmosphereButtonSound = Resources.Load<AudioClip>("Sounds/atmosphereSound");
        plantButtonSound = Resources.Load<AudioClip>("Sounds/plantSound");
        waterButtonSound = Resources.Load<AudioClip>("Sounds/waterSound");
        moonButtonSound = Resources.Load<AudioClip>("Sounds/moonSound");
        buttonSound = Resources.Load<AudioClip>("Sounds/buttonSound");

        AudioSrc = GetComponent<AudioSource> ();
    }

    /*
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    */
    public static void PlaySound (string clip)
    {
        switch(clip)
        {
            case "playButton":
            AudioSrc.PlayOneShot (playButtonSound);
            break;
            case "forthButton":
            AudioSrc.PlayOneShot (forthButtonSound);
            break;
            case "backButton":
            AudioSrc.PlayOneShot (backButtonSound);
            break;
            case "buttonSound":
            AudioSrc.PlayOneShot (buttonSound);
            break;
        }
    }
}
