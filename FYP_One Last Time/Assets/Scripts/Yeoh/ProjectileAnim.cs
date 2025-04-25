using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnim : MonoBehaviour
{
    public Rigidbody rb;

    void FixedUpdate()
    {
        Turn();
        Spin();
    }

    // ============================================================================

    public TurnScript turn;

    void Turn()
    {
        if(!turn) return;
        if(rb.velocity == Vector3.zero) return;

        Vector3 dir = rb.velocity.normalized;

        turn.UpdateTurn(dir);
    }

    // ============================================================================

    public Transform spinnerTr;
    public Vector3 spinAxisMult = new(40, 0, 0);

    void Spin()
    {
        if(!spinnerTr) return;
        if(rb.velocity == Vector3.zero) return;

        float speed = rb.velocity.magnitude;

        spinnerTr.Rotate(speed * Time.fixedDeltaTime * spinAxisMult);
    }
}
