using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playButtonSound, forthButtonSound, backButtonSound, errorSound, atmosphereButtonSound, plantButtonSound, groundButtonSound, waterButtonSound, moonButtonSound, buttonSound;
    public static AudioSource AudioSrc;
    public static float StartVolume;

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
        groundButtonSound = Resources.Load<AudioClip>("Sounds/groundSound");
        buttonSound = Resources.Load<AudioClip>("Sounds/buttonSound");

        AudioSrc = GetComponent<AudioSource> ();

        StartVolume = AudioSrc.volume;
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
            case "Water":
            AudioSrc.PlayOneShot(waterButtonSound);
            break;
            case "Ground":
            AudioSrc.PlayOneShot(groundButtonSound);
            break;
            case "Plants":
            AudioSrc.PlayOneShot(plantButtonSound);
            break;
            case "Moon":
            AudioSrc.volume += 90000;
            AudioSrc.volume *= 90000;       // Moon is barely audible :(
            AudioSrc.PlayOneShot(moonButtonSound);
            goto case "Volume";
            case "Atmosphere":
            AudioSrc.volume *= 50;
            AudioSrc.PlayOneShot(atmosphereButtonSound);
            goto case "Volume";
            case "Error":
            AudioSrc.PlayOneShot(errorSound);
            break;
            case "Volume":
            AudioSrc.volume = StartVolume;
            break;
        }
    }
}
