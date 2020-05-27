using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameSettings : MonoBehaviour
{
    public AudioMixer MusicAM;
    public AudioMixer GameAM;
    private CameraFollow camera;
    void Start()
    {
        camera = GameObject.Find("/Camera").GetComponent<CameraFollow>(); //Set de la var lookAt de la cam
    }
    public void SetMusicVolume (float volume)
    {
        MusicAM.SetFloat("volume", volume);
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

    public void SetXSensivity(float sensivity)
    {
        camera.inputXSensitivity = sensivity;
    }

    public void SetYSensivity(float sensivity)
    {
        camera.inputYSensitivity = sensivity;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
