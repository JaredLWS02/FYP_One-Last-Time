using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }
    
    // ============================================================================

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadSettings();
    }

    // ============================================================================

    // public const string CamSensKey="CamSens"; 
    // public const string MaxFPSKey="MaxFPS"; 
    // public const string VSyncKey="VSync"; 
    // public const string HapticsKey="Haptics";
    // public const string CharShaderTypeKey="CharShaderType";
    // public const string EnvShaderTypeKey="EnvShaderType";

    // [Header("Defaults")]
    // public float camSens=0.05f;
    // public int maxFPS=30;
    // public int vSync=1;
    // public int haptics=1;
    // public ShaderType charShaderType=ShaderType.ToonOld;
    // public ShaderType envShaderType=ShaderType.Toon;

    void LoadSettings()
    {
        // camSens = PlayerPrefs.GetFloat(CamSensKey, camSens);
        // maxFPS = PlayerPrefs.GetInt(MaxFPSKey, maxFPS);
        // vSync = PlayerPrefs.GetInt(VSyncKey, vSync);
        // haptics = PlayerPrefs.GetInt(HapticsKey, haptics);
        // charShaderType = (ShaderType) PlayerPrefs.GetInt(CharShaderTypeKey, (int)charShaderType);
        // envShaderType = (ShaderType) PlayerPrefs.GetInt(EnvShaderTypeKey, (int)envShaderType);

        // GameEventSystem.Current.OnChangeCamSens(camSens);
        // Application.targetFrameRate = maxFPS;
        // QualitySettings.vSyncCount = vSync;
        // GameEventSystem.Current.OnToggleHaptics(haptics==1);
        // GameEventSystem.Current.OnChangeCharShaderType(charShaderType);
        // GameEventSystem.Current.OnChangeEnvShaderType(envShaderType);
    }
}

// public enum ShaderType
// {
//     Toon = 0,
//     ToonOld = 1,
//     URP = 2,
// }