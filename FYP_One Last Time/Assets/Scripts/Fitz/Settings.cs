using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
//using UnityEngine.UI.Extensions;
using Unity.VisualScripting;

public class Settings : MonoBehaviour
{
    public AudioMixer mixer;
    //public TMP_Dropdown resolutionDropdown;
    //Resolution[] resolutions;
    int[] resoWidth = { 1920, 1600, 1280 };
    int[] resoHeight = { 1280, 900, 720 };
    private void Start()
    {
        
    }

    // ==================== Screen Resolution ========================
    public void SetResolution (int resolutionID)
    {
        //Resolution resolution = resolutions[resolutionID];
        Screen.SetResolution(resoWidth[resolutionID], resoHeight[resolutionID], Screen.fullScreen);
        Debug.Log("Changed resolution to " + resoWidth[resolutionID] + " x " + resoHeight[resolutionID]);
    }

    // ==================== Set Fullscreen ========================
    public void SetFullScreen (bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        Debug.Log("Changed fullscreen to " + fullScreen);
    }

    // ==================== Audio Stuff ========================
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

    // ==================== Pause ========================
    public void TogglePause(bool toggle)
    {
        Time.timeScale = toggle ? 0f : 1f;
    }
}
