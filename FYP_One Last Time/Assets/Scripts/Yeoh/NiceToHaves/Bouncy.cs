using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    public float bounceForce=10;

    void OnCollisionEnter(Collision other)
    {
        if(other.collider.isTrigger) return;
        Rigidbody otherRb = other.rigidbody;
        if(!otherRb) return;

        Vector3 other_pos = otherRb.transform.position;

        if(IsTargetAbove(other_pos))
        {
            Push(otherRb);
        }
    }

    bool IsTargetAbove(Vector3 target_pos)
    {
        return target_pos.y > transform.position.y;
    }

    void Push(Rigidbody rb)
    {
        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }
}
