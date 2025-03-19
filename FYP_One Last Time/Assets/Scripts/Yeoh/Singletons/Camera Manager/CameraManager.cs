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

        AwakeCurCam();
    }

    // ==================================================================================================================

    CinemachineVirtualCamera _curCam;
    CinemachineFramingTransposer _framingTransposer;

    Coroutine _panCamCoroutine;
    Vector2 _startingTrackedObjectOffset;

    void AwakeCurCam()
    {
        _curCam = Camera.main?.GetComponentInParent<CinemachineVirtualCamera>();
        if(!_curCam) { Debug.LogWarning("No CinemachineVirtualCamera found on the main camera."); return; }

        _framingTransposer = _curCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
    }

    // ==================================================================================================================

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
        RecordDefaultFovs();
    }

    // ==================================================================================================================

    #region Pan Camera

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        if (_panCamCoroutine != null) StopCoroutine(_panCamCoroutine);
        _panCamCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }


    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
                default:
                    break;
            }
            endPos *= panDistance;
            startingPos = _startingTrackedObjectOffset;
            endPos += startingPos;
        }
        else
        {
            startingPos = _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;
            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            _framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }

    #endregion

    // ==================================================================================================================

    public List<CinemachineVirtualCamera> allCameras = new();

    public void RefreshAllCameras() => allCameras = GetAllCameras();

    public List<CinemachineVirtualCamera> GetAllCameras() => new(FindObjectsOfType<CinemachineVirtualCamera>());

    public CinemachineVirtualCamera GetCameraByName(string cam_name)
    {
        if(string.IsNullOrEmpty(cam_name))
        {
            Debug.LogWarning("Camera name is null or empty.");
            return null;
        }
        
        var cam = allCameras.Find(item => item.gameObject.name == cam_name);
        
        if(cam==null) Debug.LogWarning($"Camera with name '{cam_name}' not found.");
        return cam;
    }
    
    // ==================================================================================================================
    
    public CinemachineVirtualCamera defaultCamera = null;

    public void SetDefaultCamera(CinemachineVirtualCamera cam) => defaultCamera=cam;

    public void SetDefaultCamera()
    {
        Camera cam = Camera.main;
        if(!cam) return;
        Transform cam_parent = cam.transform.parent;
        if(!cam_parent) return;

        var virtualCamera = cam_parent.GetComponent<CinemachineVirtualCamera>();
        
        if(virtualCamera)
        {
            defaultCamera = virtualCamera;
        }
        else // if Camera.main is in a freelook camera instead
        {
            var freelook = cam_parent.GetComponent<CinemachineFreeLook>();
            if(!freelook) return;
            
            defaultCamera = freelook.GetRig(1);
        }

        ChangeCameraToDefault();
    }

    public void ChangeCameraToDefault() => ChangeCamera(defaultCamera);

    public bool IsDefaultCamera(CinemachineVirtualCamera camera) => defaultCamera==camera;

    // ==================================================================================================================
    
    public CinemachineVirtualCamera currentCamera;

    public void ChangeCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;

        var freelook = GetFreeLookCamera(camera);
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

    public bool IsCurrentCamera(CinemachineVirtualCamera camera) => currentCamera==camera;

    // bool CamPanSfx;

    // void EnableCamPanSfx() // Invoked
    // {
    //     CamPanSfx=true;
    // }

    // ==================================================================================================================
    
    public CinemachineFreeLook GetFreeLookCamera(CinemachineVirtualCamera camera) => camera.ParentCamera as CinemachineFreeLook;

    // ==================================================================================================================

    public List<CinemachineBasicMultiChannelPerlin> allNoises = new();

    // Dictionary to store defAmp and defFreq values with CinemachineBasicMultiChannelPerlin as key
    Dictionary<CinemachineBasicMultiChannelPerlin, Vector2> defaultAmpFreqDict = new();
    // amp will be x, freq will be y

    public void RefreshAllNoises() => allNoises = GetAllNoises();

    public List<CinemachineBasicMultiChannelPerlin> GetAllNoises() => new(FindObjectsOfType<CinemachineBasicMultiChannelPerlin>());

    public void RecordDefaultNoises()
    {
        foreach(var noise in allNoises)
        {
            defaultAmpFreqDict[noise] = new Vector2(noise.m_AmplitudeGain, noise.m_FrequencyGain);
        }
    }

    // ==================================================================================================================

    Vector3 currentShake;

    bool IsCurrentShakeWeaker(float time, float amp, float freq)
    {
        return time < currentShake.x
            || amp < currentShake.y
            || freq < currentShake.z;
    }

    public void Shake(float time=.2f, float amp=2, float freq=2)
    {
        if(IsCurrentShakeWeaker(time, amp, freq)) return;

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

    Dictionary<CinemachineVirtualCamera, float> defaultFovs = new();

    public void RecordDefaultFovs()
    {
        foreach(var cam in allCameras)
        {
            defaultFovs[cam] = cam.m_Lens.FieldOfView;
        }
    }

    Tween fovTween;

    public void TweenFOV(CinemachineVirtualCamera cam, float to, float time)
    {
        fovTween.Stop();
        if(time>0) fovTween = Tween.Custom(cam.m_Lens.FieldOfView, to, time, onValueChange: newVal => cam.m_Lens.FieldOfView=newVal, Ease.InOutSine);
        else cam.m_Lens.FieldOfView = to;

        // if(CamPanSfx) AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICameraPan, transform.position, false);
        // else Invoke("EnableCamPanSfx", 1);
    }

    public void TweenFOV(float to, float time) => TweenFOV(currentCamera, to, time);

    public void TweenDefaultFOV(CinemachineVirtualCamera cam, float time) => TweenFOV(cam, defaultFovs[cam], time);

    public void TweenDefaultFOV(float time) => TweenFOV(currentCamera, defaultFovs[currentCamera], time);

    public void CancelFOVTween() => fovTween.Complete();
    
    // ==================================================================================================================

    Tween orthoTween;

    public void TweenOrthoSize(float to, float time)
    {
        orthoTween.Stop();
        if(time>0) orthoTween = Tween.Custom(currentCamera.m_Lens.OrthographicSize, to, time, onValueChange: newVal => currentCamera.m_Lens.OrthographicSize=newVal, Ease.InOutSine);
        else currentCamera.m_Lens.OrthographicSize = to;

        // if(CamPanSfx) AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICameraPan, transform.position, false);
        // else Invoke("EnableCamPanSfx", 1);
    }

    public void CancelOrthoTween() => orthoTween.Complete();
        
    // ==================================================================================================================

    Tween dutchTween;

    public void TweenDutch(float angle, float time)
    {
        dutchTween.Stop();
        if(time>0) dutchTween = Tween.Custom(currentCamera.m_Lens.Dutch, angle, time, onValueChange: newVal => currentCamera.m_Lens.Dutch=newVal, Ease.InOutSine);
        else currentCamera.m_Lens.Dutch = angle;
    }

    public void CancelDutchTween() => dutchTween.Complete();

    public void ValveDutch(float angle=2, float dutch_in=.025f, float dutch_out=.1f)
    {
        if(valveDutching_crt!=null) StopCoroutine(valveDutching_crt);
        valveDutching_crt = StartCoroutine(ValveDutching(angle, dutch_in, dutch_out));
    }

    Coroutine valveDutching_crt;

    IEnumerator ValveDutching(float angle, float tweenIn, float tweenOut)
    {
        angle = Random.Range(0, 2)==0 ? angle : -angle;
        TweenDutch(angle, tweenIn);
        yield return new WaitForSeconds(tweenIn);
        TweenDutch(0, tweenOut);
    }

    public void CancelValveDutch()
    {
        if(!currentCamera) return;

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

    void OnToggleHaptics(bool toggle) => haptics=toggle;
}   
