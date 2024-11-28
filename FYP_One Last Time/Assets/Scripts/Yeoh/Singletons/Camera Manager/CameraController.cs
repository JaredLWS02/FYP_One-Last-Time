using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    CameraManager camM;

    void OnEnable()
    {
        camM = CameraManager.Current;

        events.OnEnable?.Invoke();
    }

    void OnDisable() => events.OnDisable?.Invoke();

    // ============================================================================

    public CinemachineVirtualCamera selectedCam;

    public void SelectCameraName(string cam_name)
        => selectedCam = camM.GetCameraByName(cam_name);

    // ============================================================================
    
    public void ChangeCameraToSelected() => camM.ChangeCamera(selectedCam);

    // ============================================================================
    
    public void SetDefaultCameraToSelected() => camM.SetDefaultCamera(selectedCam);

    public void ChangeCameraToDefault() => camM.ChangeCameraToDefault();
    
    // ============================================================================

    [Header("Shake")]
    public float shakeTime=.2f;
    public float amplitude=1;
    public float frequency=2;

    public void SetShakeTime(float t) => shakeTime=t;
    public void SetShakeAmp(float amp) => amplitude=amp;
    public void SetShakeFreq(float freq) => frequency=freq;

    public void Shake() => camM.Shake(shakeTime, amplitude, frequency);

    public void ToggleShake(bool toggle)
    {
        if(toggle)
        camM.EnableShake(amplitude, frequency);
        else
        camM.DisableShake();
    }

    public void CancelShake() => camM.CancelShake();

    // ============================================================================
    
    [Header("Dutch")]
    public float angle=2;
    public float dutchIn=.025f;
    public float dutchOut=.1f;

    public void ValveDutch() => camM.ValveDutch(angle, dutchIn, dutchOut);

    public void CancelValveDutch() => camM.CancelValveDutch();
    
    // ============================================================================
    
    public void ToggleHaptics(bool toggle) => camM.haptics=toggle;
    
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
