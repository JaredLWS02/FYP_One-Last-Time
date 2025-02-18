using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================    

    public Transform follow;
    public Transform lookAt;

    void Start()
    {
        if(follow) EventM.OnCameraFollow(follow);
        if(lookAt) EventM.OnCameraLookAt(lookAt);
    }
}
