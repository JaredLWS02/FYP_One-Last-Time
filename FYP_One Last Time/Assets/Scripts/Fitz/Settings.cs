using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public AudioMixer mixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResoID = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResoID = i;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResoID;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetQuality (int qualityID)
    {
        QualitySettings.SetQualityLevel(qualityID);
    }

    public void SetResolution (int resolutionID)
    {
        Resolution resolution = resolutions[resolutionID];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen (bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void SetMasterVolume (float volume)
    {
        mixer.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume (float volume)
    {
        mixer.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume (float volume)
    {
        mixer.SetFloat("sfxVolume", volume);
    }
}
