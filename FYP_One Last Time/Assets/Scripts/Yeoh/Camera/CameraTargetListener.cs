using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTargetListener : MonoBehaviour
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.CameraFollowEvent += OnCameraFollow;
        EventM.CameraLookAtEvent += OnCameraLookAt;
    }
    void OnDisable()
    {
        EventM.CameraFollowEvent -= OnCameraFollow;
        EventM.CameraLookAtEvent -= OnCameraLookAt;
    }

    // ============================================================================    

    public CinemachineVirtualCamera cineCam;

    void OnCameraFollow(Transform target)
    {
        if(!cineCam) return;
        cineCam.Follow = target;
    }

    void OnCameraLookAt(Transform target)
    {
        if(!cineCam) return;
        cineCam.LookAt = target;
    }
    
}
