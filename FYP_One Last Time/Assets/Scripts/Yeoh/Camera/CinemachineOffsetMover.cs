using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineOffsetMover : MonoBehaviour
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.CinemachineOffsetMoveEvent += Move;
    }
    void OnDisable()
    {
        EventM.CinemachineOffsetMoveEvent -= Move;
    }

    // ============================================================================

    public CinemachineCameraOffset camOffset;

    public Vector3 offset = Vector3.zero;
    
    public Vector3 maxDistance = Vector3.one*5;

    public float moveTime = .5f;

    Vector3 velocity = Vector3.zero;

    // ============================================================================

    public void Move(Vector3 axis)
    {
        if(!camOffset) return;

        var finalPos = Vector3.Scale(axis, maxDistance) + offset;

        camOffset.m_Offset = Vector3.SmoothDamp(camOffset.m_Offset, finalPos, ref velocity, moveTime);
    }
}
