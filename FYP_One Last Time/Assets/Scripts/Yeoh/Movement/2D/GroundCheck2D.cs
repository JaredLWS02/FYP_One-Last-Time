using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCheck2D : MonoBehaviour
{
    public Vector3 boxSize = new(.5f, .05f, .5f);
    public Vector3 boxOffset = Vector3.zero;

    public LayerMask groundLayer;

    Collider2D[] GetOverlap()
    {
        return Physics2D.OverlapBoxAll(transform.position + boxOffset, boxSize, transform.eulerAngles.z, groundLayer);
    }

    // ============================================================================

    List<Collider2D> previous_colliders = new();
    List<Collider2D> current_colliders = new();

    void FixedUpdate()
    {
        Check();
    }

    void Check()
    {
        current_colliders.Clear();

        Collider2D[] colliders = GetOverlap();

        // Check OnEnter
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
        
        // Check OnExit
        foreach(var coll in previous_colliders)
        {
            // if present in previous but missing in current
            if(!current_colliders.Contains(coll))
            {
                OnBoxExit(coll);
            }
        }

        // Update old to new to prepare for the next round
        previous_colliders = new(current_colliders);
    }    

    // ============================================================================

    void OnBoxEnter(Collider2D other)
    {
        if(previous_colliders.Count==0 && current_colliders.Count > 0)
        {
            EventManager.Current.OnLandGround(gameObject);
            OnLandGround.Invoke();
            Debug.Log("Landed");
        }
    }
    
    void OnBoxExit(Collider2D other)
    {
        if(previous_colliders.Count > 0 && current_colliders.Count==0)
        {
            EventManager.Current.OnLeaveGround(gameObject);
            OnLeaveGround.Invoke();
            Debug.Log("LeaveGround");
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

    // ============================================================================
    
    [Header("Events")]
    public UnityEvent OnLandGround;
    public UnityEvent OnLeaveGround;
}