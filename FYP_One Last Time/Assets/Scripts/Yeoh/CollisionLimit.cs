using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]

public class CollisionLimit : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        Hit();
    }
    void OnTriggerEnter(Collider other)
    {
        Hit();
    }

    // ============================================================================

    public int maxCollisions = 5;

    void Hit()
    {
        maxCollisions--;

        events.OnHit?.Invoke();
        
        if(maxCollisions <= 0)
        events.OnLastHit?.Invoke();
    }

    // ============================================================================

    [System.Serializable]
    public struct Events
    {
        public UnityEvent OnHit;
        public UnityEvent OnLastHit;
    }
    [Space]
    public Events events;
}
