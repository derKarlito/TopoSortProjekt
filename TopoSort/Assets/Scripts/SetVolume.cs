using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour 
{

    public void SetLevel (float sliderValue)
    {
        MusicManager.Music.volume = sliderValue;
        SoundManagerScript.AudioSrc.volume = sliderValue;
    }
}