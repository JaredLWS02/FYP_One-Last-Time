using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMeter : MonoBehaviour
{
    public GameObject owner;

    [Header("Debug")]
    public Vector3 velocity;
    public float velocityMagnitude;
    public Vector3 velocityDirection;

    Vector3 prevPos;

    void FixedUpdate()
    {
        velocity = (owner.transform.position - prevPos) / Time.fixedDeltaTime;
        velocityMagnitude = velocity.magnitude;
        velocityDirection = velocity.normalized;

        prevPos = owner.transform.position;
    }
}
