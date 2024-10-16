using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]

public class JumpScript : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }

    // Input ============================================================================
    
    void OnInputJump(InputValue value)
    {
        float input = value.Get<float>();

        if(input>0) //press
        {
            JumpBuffer();
        }
        else //release
        {
            JumpCut();
        }
    }
    
    // Updates ============================================================================

    void Update()
    {
        UpdateExtraJumps();
        UpdateJumpBuffer();
        UpdateCoyoteTime();
        
        TryJump();
    }    

    void FixedUpdate()
    {
        CheckFallVelocity();
    }

    // Jump ============================================================================
    
    public bool canJump=true;
    public float jumpForce=10;

    void TryJump()
    {
        if(!canJump) return;

        if(!HasJumpBuffer()) return;

        if(HasCoyoteTime())
        {
            Jump();            
        }
        else
        {
            DoExtraJump();
        }
    }

    void Jump()
    {
        if(isJumpCooling) return;
        StartCoroutine(JumpCooling());

        if(!rb.isKinematic)
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        
        rb.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);

        jumpBufferLeft = -1;
        coyoteTimeLeft = -1;
    }

    // Cooldown ============================================================================

    public float jumpCooldown=.2f;
    bool isJumpCooling;

    IEnumerator JumpCooling()
    {
        isJumpCooling=true;
        yield return new WaitForSeconds(jumpCooldown);
        isJumpCooling=false;
    }

    // Extra Jump ============================================================================

    public int extraJumps=1;
    int extraJumpsLeft;

    void UpdateExtraJumps()
    {
        // Only replenish extra jumps if grounded and jump not cooling
        if(IsGrounded() && !isJumpCooling)
        {
            extraJumpsLeft = extraJumps;
        }
    }

    void DoExtraJump()
    {
        if(extraJumpsLeft<=0) return;

        extraJumpsLeft--;
        Jump();
    }

    // Jump Buffer ============================================================================

    [Header("Assist")]
    public float jumpBufferTime=.2f;
    float jumpBufferLeft;

    public void JumpBuffer()
    {
        jumpBufferLeft = jumpBufferTime;
    }

    void UpdateJumpBuffer()
    {
        jumpBufferLeft -= Time.deltaTime;
    }

    bool HasJumpBuffer()
    {
        return jumpBufferLeft>0;
    }

    // Coyote Time ============================================================================

    public float coyoteTime=.2f;
    float coyoteTimeLeft;

    void UpdateCoyoteTime()
    {
        coyoteTimeLeft -= Time.deltaTime;

        // Only replenish coyote time if grounded and jump not cooling
        if(IsGrounded() && !isJumpCooling)
        {
            coyoteTimeLeft = coyoteTime;
        }
    }

    bool HasCoyoteTime()
    {
        return coyoteTimeLeft>0;
    }

    // Jump Cut ============================================================================

    public float jumpCutMult=.5f;

    public void JumpCut()
    {
        // only if going up
        if(rb.velocity.y>0)
        {
            rb.AddForce(Vector3.down * rb.velocity.y * (1-jumpCutMult), ForceMode.Impulse);
        }
    }

    // Falling ============================================================================

    [Header("Falling")]
    public float minVelocityBeforeFastFall = -.1f;
    public float fastFallForce=15f;
    public float maxFallVelocity = -30f;

    void CheckFallVelocity()
    {
        // only if going down
        if(rb.velocity.y>=0) return;
        
        if(maxFallVelocity>=0) return;

        if(rb.velocity.y < maxFallVelocity)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxFallVelocity, rb.velocity.z);
            return;
        }
        
        if(rb.velocity.y < minVelocityBeforeFastFall)
        {
            rb.AddForce(Vector3.down * fastFallForce);
        }
    }

    // Ground Check ============================================================================

    [Header("Ground Check")]
    public Vector3 boxSize = new(.5f, .05f, .5f);
    public Vector3 boxCenterOffset = Vector3.zero;
    public LayerMask groundLayer;

    public bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + boxCenterOffset, boxSize, transform.rotation, groundLayer);

        foreach(var coll in colliders)
        {
            if(!coll.isTrigger) return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Vector3 boxCenter = transform.position + boxCenterOffset;

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);

        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        // Reset the Gizmos matrix to default
        Gizmos.matrix = Matrix4x4.identity;
    }
}
