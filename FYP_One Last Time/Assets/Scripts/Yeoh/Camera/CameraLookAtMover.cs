using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAtMover : MonoBehaviour
{
    Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }

    // ============================================================================

    public Transform follow;
    
    public Vector3 moveRange = Vector3.one*5;

    public Vector3 offsetPos = Vector3.zero;

    public void Move(Vector3 axis)
    {
        var origin = follow ? follow.position : startPos;

        transform.position = origin + Vector3.Scale(axis, moveRange) + offsetPos;
    }
}
