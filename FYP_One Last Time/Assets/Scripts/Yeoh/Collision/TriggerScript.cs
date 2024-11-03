using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class TriggerScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger) return;
        Rigidbody otherRb = other.attachedRigidbody;
        if(!otherRb) return;

        OnEnter.Invoke();
    }
    void OnTriggerStay(Collider other)
    {
        if(other.isTrigger) return;
        Rigidbody otherRb = other.attachedRigidbody;
        if(!otherRb) return;

        OnStay.Invoke();

    }
    void OnTriggerExit(Collider other)
    {
        if(other.isTrigger) return;
        Rigidbody otherRb = other.attachedRigidbody;
        if(!otherRb) return;

        OnExit.Invoke();
    }

    // ============================================================================

    [Header("Events")]
    public UnityEvent OnEnter;
    public UnityEvent OnStay;
    public UnityEvent OnExit;
}
