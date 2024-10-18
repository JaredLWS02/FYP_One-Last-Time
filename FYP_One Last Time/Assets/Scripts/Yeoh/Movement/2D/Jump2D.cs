using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundCheck2D))]

public class Jump2D : MonoBehaviour
{
    Rigidbody2D rb;
    GroundCheck2D ground;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck2D>();
    }

    // ============================================================================

    public void OnJump(GameObject jumper, float input)
    {
        if(jumper!=gameObject) return;

        if(input>0) //press
        {
            JumpBuffer();
        }
        else //release
        {
            JumpCut();
        }
    }

    // ============================================================================

    void Update()
    {
        UpdateExtraJumps();
        UpdateJumpBuffer();
        UpdateCoyoteTime();
        
        TryJump();
    }    

    // ============================================================================
    
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
        rb.velocity = new(rb.velocity.x, 0);
        
        rb.AddForce(Vector3.up*jumpForce, ForceMode2D.Impulse);

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
        if(ground.IsGrounded() && !isJumpCooling)
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
        if(ground.IsGrounded() && !isJumpCooling)
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
            rb.AddForce(Vector3.down * rb.velocity.y * (1-jumpCutMult), ForceMode2D.Impulse);
        }
    }
}
