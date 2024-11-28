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

    // ============================================================================
    
    public void ChangeCameraToSelected() => CamM.ChangeCamera(selectedCam);

    // ============================================================================
    
    public void SetDefaultCameraToSelected() => CamM.SetDefaultCamera(selectedCam);

    public void ChangeCameraToDefault() => CamM.ChangeCameraToDefault();
    
    // ============================================================================

    [Header("Shake")]
    public float shakeTime=.2f;
    public float amplitud=2;
    public float frequency=2;

    public void SetShakeTime(float t) => shakeTime=t;
    public void SetShakeAmp(float amp) => amplitud=amp;
    public void SetShakeFreq(float freq) => frequency=freq;

    public void Shake() => CamM.Shake(shakeTime, amplitud, frequency);

    public void ToggleShake(bool toggle)
    {
        if(toggle)
        CamM.EnableShake(amplitud, frequency);
        else
        CamM.DisableShake();
    }

    public void CancelShake() => CamM.CancelShake();

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
