using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using PrimeTween;

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
    
    // ==================================================================================================================

    public List<CinemachineVirtualCamera> allCameras = new();

    public void RefreshAllCameras()
    {
        allCameras.Clear();
        allCameras = GetAllCameras();
    }

    public List<CinemachineVirtualCamera> GetAllCameras()
    {
        List<CinemachineVirtualCamera> camerasList = new(FindObjectsOfType<CinemachineVirtualCamera>());

        return camerasList;
    }
    
    // ==================================================================================================================
    
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

    // ==================================================================================================================
    
    public CinemachineVirtualCamera currentCamera;

    public void ChangeCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;

        CinemachineFreeLook freelook = GetFreeLookCamera(camera);
        if(freelook) freelook.Priority = 10;

        StopAllAnimations();
        
        currentCamera = camera;

        foreach(var cam in allCameras)
        {
            if(cam != camera)
            {
                cam.Priority = 0;
            }
        }

        // if(CamPanSfx) AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICameraPan, transform.position, false);
        // else Invoke("EnableCamPanSfx", 1);
    }

    public bool IsCurrentCamera(CinemachineVirtualCamera camera)
    {
        return currentCamera==camera;
    }

    // bool CamPanSfx;

    // void EnableCamPanSfx() // Invoked
    // {
    //     CamPanSfx=true;
    // }

    // ==================================================================================================================
    
    public CinemachineFreeLook GetFreeLookCamera(CinemachineVirtualCamera camera)
    {
        CinemachineFreeLook freeLookParent = camera.ParentCamera as CinemachineFreeLook;
        return freeLookParent;
    }

    // ==================================================================================================================

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
        List<CinemachineBasicMultiChannelPerlin> noisesList = new(FindObjectsOfType<CinemachineBasicMultiChannelPerlin>());
        return noisesList;
    }

    public void RecordDefaultNoises()
    {
        foreach(var noise in allNoises)
        {
            defaultAmpFreqDict[noise] = new Vector2(noise.m_AmplitudeGain, noise.m_FrequencyGain);
        }
    }

    // ==================================================================================================================

    Vector3 currentShake;

    public void Shake(float time=.2f, float amp=1, float freq=2)
    {
        if(time<currentShake.x || amp<currentShake.y || freq<currentShake.z) return;

        if(haptics) Vibrator.Vibrate();

        if(shaking_crt!=null) StopCoroutine(shaking_crt);
        shaking_crt = StartCoroutine(Shaking(time, amp, freq));
    }

    Coroutine shaking_crt;

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
        foreach(var noise in allNoises)
        {
            noise.m_AmplitudeGain = amp;
            noise.m_FrequencyGain = freq;
        }
    }
    public void DisableShake()
    {
        foreach(var noise in allNoises)
        {
            if(defaultAmpFreqDict.ContainsKey(noise))
            {
                noise.m_AmplitudeGain = defaultAmpFreqDict[noise].x;
                noise.m_FrequencyGain = defaultAmpFreqDict[noise].y;
            }
        }
    }

    public void CancelShake()
    {
        if(shaking_crt!=null) StopCoroutine(shaking_crt);
        DisableShake();
        currentShake = Vector3.zero;
    }

    // ==================================================================================================================

    Tween fovTween;

    public void TweenFOV(float to, float time)
    {
        fovTween.Stop();
        fovTween = Tween.Custom(currentCamera.m_Lens.FieldOfView, to, time, onValueChange: newVal => currentCamera.m_Lens.FieldOfView=newVal, Ease.InOutSine);

        // if(CamPanSfx) AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICameraPan, transform.position, false);
        // else Invoke("EnableCamPanSfx", 1);
    }

    public void CancelFOVTween()
    {
        fovTween.Complete();
    }
    
    Tween orthoTween;

    public void TweenOrthoSize(float to, float time)
    {
        orthoTween.Stop();
        orthoTween = Tween.Custom(currentCamera.m_Lens.OrthographicSize, to, time, onValueChange: newVal => currentCamera.m_Lens.OrthographicSize=newVal, Ease.InOutSine);

        // if(CamPanSfx) AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICameraPan, transform.position, false);
        // else Invoke("EnableCamPanSfx", 1);
    }

    public void CancelOrthoTween()
    {
        orthoTween.Complete();
    }
        
    // ==================================================================================================================

    Tween dutchTween;

    public void TweenDutch(float angle, float time)
    {
        dutchTween.Stop();
        dutchTween = Tween.Custom(currentCamera.m_Lens.Dutch, angle, time, onValueChange: newVal => currentCamera.m_Lens.Dutch=newVal, Ease.InOutSine);
    }

    public void ValveDutch(float angle=10, float tweenIn=.1f, float tweenOut=.5f)
    {
        if(valveDutching_crt!=null) StopCoroutine(valveDutching_crt);
        valveDutching_crt = StartCoroutine(ValveDutching(angle, tweenIn, tweenOut));
    }

    public void CancelDutchTween()
    {
        dutchTween.Complete();
    }

    Coroutine valveDutching_crt;

    IEnumerator ValveDutching(float angle, float tweenIn, float tweenOut)
    {
        TweenDutch(angle, tweenIn);
        yield return new WaitForSeconds(tweenIn);
        TweenDutch(0, tweenOut);
    }

    public void CancelValveDutch()
    {
        if(valveDutching_crt!=null) StopCoroutine(valveDutching_crt);
        dutchTween.Stop();
        currentCamera.m_Lens.Dutch=0;
    }    

    // ==================================================================================================================

    public void StopAllAnimations()
    {
        CancelShake();
        CancelFOVTween();
        CancelOrthoTween();
        CancelDutchTween();
        CancelValveDutch();
    }

    // ==================================================================================================================

    public bool haptics=true;

    void OnToggleHaptics(bool toggle)
    {
        haptics=toggle;
    }
}   
