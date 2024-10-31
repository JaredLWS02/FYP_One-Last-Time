using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check")]
    public Rigidbody rb;
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    public Vector3 boxSize = new(.5f, .05f, .5f);
    public Vector3 boxOffset = Vector3.zero;

    public LayerMask groundLayer;

    Collider[] GetOverlap()
    {
        return Physics.OverlapBox(transform.position + boxOffset, boxSize, transform.rotation, groundLayer);
    }

    // ============================================================================

    List<Collider> previous_colliders = new();
    List<Collider> current_colliders = new();

    void FixedUpdate()
    {
        Check();
    }

    void Check()
    {
        current_colliders.Clear();

        CheckOnEnter();
        CheckOnExit();

        // Update old to new to prepare for the next round
        previous_colliders = new(current_colliders);
    }

    void CheckOnEnter()
    {
        Collider[] colliders = GetOverlap();

        foreach(var coll in colliders)
        {
            // must not be trigger
            if(coll.isTrigger) continue;

            current_colliders.Add(coll);

            // if present in current but missing in previous
            if(!previous_colliders.Contains(coll))
            {
                OnBoxEnter(coll);
            }   
        }
    }

    void CheckOnExit()
    {
        foreach(var coll in previous_colliders)
        {
            // if present in previous but missing in current
            if(!current_colliders.Contains(coll))
            {
                OnBoxExit(coll);
            }
        }
    }

    // ============================================================================

    //public float minLandVelocity = -1;

    void OnBoxEnter(Collider other)
    {
        if(previous_colliders.Count==0 && current_colliders.Count > 0)
        {
            // going down only
            //if(rb.velocity.y <= minLandVelocity)
            if(rb.velocity.y <= 0)
            {
                EventM.OnLandGround(gameObject);
            }
        }
    }
    
    void OnBoxExit(Collider other)
    {
        if(previous_colliders.Count > 0 && current_colliders.Count==0)
        {
            EventM.OnLeaveGround(gameObject);
        }
    }

    // ============================================================================

    public bool IsGrounded()
    {
        return current_colliders.Count > 0;
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColor = Color.blue;

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        
        Gizmos.color = gizmoColor;

        Vector3 boxCenter = transform.position + boxOffset;

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);

        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        // Reset the Gizmos matrix to default
        Gizmos.matrix = Matrix4x4.identity;
    }
}
