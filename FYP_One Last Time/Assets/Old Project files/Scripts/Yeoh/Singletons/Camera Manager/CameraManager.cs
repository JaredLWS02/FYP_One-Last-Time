using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Current;

    void Awake()
    {
        if(!Current) Current=this;        
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        //GameEventSystem.Current.ToggleHapticsEvent += OnToggleHaptics;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        //GameEventSystem.Current.ToggleHapticsEvent -= OnToggleHaptics;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //CamPanSfx=false;
        RefreshAllCameras();
        RefreshAllNoises();
        RecordDefaultNoises();
        SetDefaultCamera();
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<CinemachineVirtualCamera> allCameras = new List<CinemachineVirtualCamera>();

    public void RefreshAllCameras()
    {
        allCameras.Clear();
        allCameras = GetAllCameras();
    }

    public List<CinemachineVirtualCamera> GetAllCameras()
    {
        List<CinemachineVirtualCamera> camerasList = new List<CinemachineVirtualCamera>(FindObjectsOfType<CinemachineVirtualCamera>());

        return camerasList;
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public CinemachineVirtualCamera defaultCamera = null;

    public void SetDefaultCamera()
    {
        if(!Camera.main) return;
        if(!Camera.main.transform.parent) return;

        CinemachineVirtualCamera virtualCamera = Camera.main.transform.parent.GetComponent<CinemachineVirtualCamera>();
        
        if(virtualCamera)
        {
            defaultCamera = virtualCamera;
        }
        else // if Camera.main is in a freelook camera instead
        {
            CinemachineFreeLook freelook = Camera.main.transform.parent.GetComponent<CinemachineFreeLook>();

            if(!freelook) return;
            
            defaultCamera = freelook.GetRig(1);
        }

        ChangeCameraToDefault();
    }

    public void ChangeCameraToDefault()
    {
        ChangeCamera(defaultCamera);
    }

    public bool IsDefaultCamera(CinemachineVirtualCamera camera)
    {
        return defaultCamera==camera;
    }
                
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public CinemachineFreeLook IsCameraFreeLook(CinemachineVirtualCamera camera)
    {
        CinemachineFreeLook freeLookParent = camera.ParentCamera as CinemachineFreeLook;

        return freeLookParent;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public CinemachineVirtualCamera currentCamera;

    public void ChangeCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;

        if(IsCameraFreeLook(camera))
        {
            IsCameraFreeLook(camera).Priority = 10;
        }
        
        currentCamera = camera;

        foreach(CinemachineVirtualCamera c in allCameras)
        {
            if(c!=camera)
            {
                c.Priority = 0;
            }
        }

        // if(CamPanSfx) AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICameraPan, transform.position, false);
        // else Invoke("EnableCamPanSfx", 1);
    }

    // bool CamPanSfx;

    // void EnableCamPanSfx() // Invoked
    // {
    //     CamPanSfx=true;
    // }

    public bool IsCurrentCamera(CinemachineVirtualCamera camera)
    {
        return currentCamera==camera;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<CinemachineBasicMultiChannelPerlin> allNoises = new();

    // Dictionary to store defAmp and defFreq values with CinemachineBasicMultiChannelPerlin as key
    Dictionary<CinemachineBasicMultiChannelPerlin, Vector2> defaultAmpFreqDict = new();
    // amp will be x, freq will be y

    public void RefreshAllNoises()
    {
        allNoises.Clear();
        allNoises = GetAllNoises();
    }

    public List<CinemachineBasicMultiChannelPerlin> GetAllNoises()
    {
        List<CinemachineBasicMultiChannelPerlin> noisesList = new List<CinemachineBasicMultiChannelPerlin>(FindObjectsOfType<CinemachineBasicMultiChannelPerlin>());

        return noisesList;
    }

    public void RecordDefaultNoises()
    {
        foreach(CinemachineBasicMultiChannelPerlin noise in allNoises)
        {
            defaultAmpFreqDict[noise] = new Vector2(noise.m_AmplitudeGain, noise.m_FrequencyGain);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Vector3 currentShake;

    public void Shake(float time=.2f, float amp=1, float freq=2)
    {
        if(time<currentShake.x || amp<currentShake.y || freq<currentShake.z) return;

        if(haptics) Vibrator.Vibrate();

        if(shakingRt!=null) StopCoroutine(shakingRt);
        shakingRt = StartCoroutine(Shaking(time, amp, freq));
    }

    Coroutine shakingRt;
    IEnumerator Shaking(float t, float amp, float freq)
    {
        currentShake = new Vector3(t, amp, freq);

        EnableShake(amp, freq);
        yield return new WaitForSecondsRealtime(t);
        DisableShake();

        currentShake = Vector3.zero;
    }

    public void EnableShake(float amp=0, float freq=0)
    {
        foreach(CinemachineBasicMultiChannelPerlin noise in allNoises)
        {
            noise.m_AmplitudeGain = amp;
            noise.m_FrequencyGain = freq;
        }
    }
    public void DisableShake()
    {
        foreach(CinemachineBasicMultiChannelPerlin noise in allNoises)
        {
            if(defaultAmpFreqDict.ContainsKey(noise))
            {
                noise.m_AmplitudeGain = defaultAmpFreqDict[noise].x;
                noise.m_FrequencyGain = defaultAmpFreqDict[noise].y;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int tweenFovId=0;
    public void TweenFOV(float newFov, float time)
    {
        LeanTween.cancel(tweenFovId);
        tweenFovId = LeanTween.value(currentCamera.m_Lens.FieldOfView, newFov, time)
            .setEaseInOutSine()
            .setOnUpdate( (float value)=>{currentCamera.m_Lens.FieldOfView=value;} )
            .id;

        // if(CamPanSfx) AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICameraPan, transform.position, false);
        // else Invoke("EnableCamPanSfx", 1);
    }
    
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int tweenOrthoId=0;
    public void TweenOrthoSize(float newCamSize, float time)
    {
        LeanTween.cancel(tweenOrthoId);
        tweenOrthoId = LeanTween.value(currentCamera.m_Lens.OrthographicSize, newCamSize, time)
            .setEaseInOutSine()
            .setOnUpdate( (float value)=>{currentCamera.m_Lens.OrthographicSize=value;} )
            .id;

        // if(CamPanSfx) AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICameraPan, transform.position, false);
        // else Invoke("EnableCamPanSfx", 1);
    }
        
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool haptics=true;

    void OnToggleHaptics(bool toggle)
    {
        haptics=toggle;
    }
}   
