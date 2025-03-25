using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGoomba : MonoBehaviour
{
    public float bounceForce;
    public float slipForce = 3;

    // ============================================================================

    void OnCollisionStay(Collision other)
    {
        if(other.collider.isTrigger) return;
        var other_rb = other.rigidbody;
        if(!other_rb) return;

        Vector3 contact_point = other.contacts[0].point;
        if(!IsAbove(contact_point)) return;

        if(bounceForce!=0)
        {
            other_rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }

        if(slipForce!=0)
        {
            Vector3 vec_to_target = contact_point - transform.position;
            vec_to_target.y = 0;
            if(vec_to_target == Vector3.zero) vec_to_target = transform.forward;

            Vector3 dir_to_target = vec_to_target.normalized;

            other_rb.AddForce(dir_to_target * slipForce, ForceMode.Impulse);
        }
    }

    // ============================================================================

    bool IsAbove(Vector3 pos) => pos.y > transform.position.y;
}
