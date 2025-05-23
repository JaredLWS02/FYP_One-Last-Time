using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    CameraManager CamM;

    void OnEnable()
    {
        CamM = CameraManager.Current;

        events.OnEnable?.Invoke();
    }

    void OnDisable() => events.OnDisable?.Invoke();

    // ============================================================================

    public CinemachineVirtualCamera selectedCam;

    public void SelectCameraName(string cam_name)
        => selectedCam = CamM.GetCameraByName(cam_name);

    public void SelectDefaultCamera() => selectedCam = CamM.defaultCamera;

    // ============================================================================
    
    public void ChangeCameraToSelected() => CamM.ChangeCamera(selectedCam);

    // ============================================================================
    
    public void SetDefaultCameraToSelected() => CamM.SetDefaultCamera(selectedCam);

    public void ChangeCameraToDefault() => CamM.ChangeCameraToDefault();
    
    // ============================================================================

    [Header("Shake")]
    public float shakeTime=.2f;
    public float amplitude4=4;
    public float frequency2=2;

    public void SetShakeTime(float t) => shakeTime=t;
    public void SetShakeAmp(float amp) => amplitude4=amp;
    public void SetShakeFreq(float freq) => frequency2=freq;

    public void Shake() => CamM.Shake(shakeTime, amplitude4, frequency2);

    public void ToggleShake(bool toggle)
    {
        if(toggle)
        CamM.EnableShake(amplitude4, frequency2);
        else
        CamM.DisableShake();
    }

    public void CancelShake() => CamM.CancelShake();

    // ============================================================================

    [Header("FOV")]
    public float tweenFovTime=.5f;
    public void SetTweenFOVTime(float t) => tweenFovTime=t;

    public void TweenFOV(float to) => CamM.TweenFOV(selectedCam, to, tweenFovTime);

    public void TweenDefaultFOV() => CamM.TweenDefaultFOV(selectedCam, tweenFovTime);

    public void CancelFOVTween() => CamM.CancelFOVTween();

    // ============================================================================
    
    [Header("Dutch")]
    public float angle=2;
    public float dutchIn=.025f;
    public float dutchOut=.1f;

    public void ValveDutch() => CamM.ValveDutch(angle, dutchIn, dutchOut);

    public void CancelValveDutch() => CamM.CancelValveDutch();
    
    // ============================================================================
    
    public void ToggleHaptics(bool toggle) => CamM.haptics=toggle;
    
    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnEnable;
        public UnityEvent OnDisable;
    }
    [Space]
    public Events events;
}
