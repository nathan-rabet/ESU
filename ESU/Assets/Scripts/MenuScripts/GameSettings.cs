using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameSettings : MonoBehaviour
{
    public AudioMixer MusicAM;
    public AudioMixer GameAM;
    public void SetMusicVolume (float volume)
    {
        MusicAM.SetFloat("MusicVolume", volume);
    }
    public void SetGameVolume (float volume)
    {
        GameAM.SetFloat("GameVolume", volume);
    }
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
