using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TurnScript))]

public class FaceVelocityDirection : MonoBehaviour
{
    Rigidbody rb;
    TurnScript turn;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        turn = GetComponent<TurnScript>();
    }
    
    // ============================================================================

    public bool enable=true;
    public float minSpeed=0;

    void FixedUpdate()
    {
        if(!enable) return;
    
        if(rb.velocity.sqrMagnitude <= minSpeed) return;

        turn.UpdateTurn(rb.velocity.normalized);
    }
}
