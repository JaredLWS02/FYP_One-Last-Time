using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocitySpinAnim : MonoBehaviour
{
    public Rigidbody rb;

    public Vector3 axisMult = new(50, 0, 0);

    void FixedUpdate()
    {
        var velocity = Vector3.Scale(rb.velocity, axisMult);

        transform.Rotate(Time.fixedDeltaTime * velocity);
    }
}
