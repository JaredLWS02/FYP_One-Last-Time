using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : JumpScript
{
    [Header("Wall Jump")]
    public WallCling cling;

    protected override bool IsGrounded() => !ground.IsGrounded() && cling.isClinging;

    // ============================================================================

    public float jumpAwayForce=50;

    protected override void OnBaseJump()
    {
        Rigidbody rb = cling.rb;
        Vector3 ray_dir = cling.ray.origin.forward;

        rb.AddForce(jumpAwayForce * -ray_dir, ForceMode.Impulse);
    }
}
