using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Jump2D : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Event Manager ============================================================================

    void OnEnable()
    {
        EventManager.Current.JumpEvent += OnJump;
    }
    void OnDisable()
    {
        EventManager.Current.JumpEvent -= OnJump;
    }

    // Events ============================================================================

    void OnJump(GameObject jumper, float input)
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
    public float jumpForce=8;

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

        rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);

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
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMult);
            rb.AddForce(Vector2.down * rb.velocity.y * (1-jumpCutMult), ForceMode2D.Impulse);
        }
    }

    // Falling ============================================================================

    [Header("Falling")]
    public float minVelocityBeforeFastFall = -.1f;
    public float fastFallForce=15f;
    public float maxFallVelocity = -20f;

    void CheckFallVelocity()
    {
        // only if going down
        if(rb.velocity.y>=0) return;
        
        if(maxFallVelocity>=0) return;

        if(rb.velocity.y < maxFallVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallVelocity);
            return;
        }
        
        if(rb.velocity.y < minVelocityBeforeFastFall)
        {
            rb.AddForce(Vector2.down * fastFallForce);
        }
    }

    // Ground Check ============================================================================

    [Header("Ground Check")]
    public Vector2 boxSize = new Vector2(.2f, .05f);
    public Vector2 boxCenterOffset = Vector2.zero;
    public LayerMask groundLayer;

    public bool IsGrounded()
    {
        Collider2D[] colliders =  Physics2D.OverlapBoxAll((Vector2)transform.position + boxCenterOffset, boxSize, 0f, groundLayer);

        foreach(var coll in colliders)
        {
            if(!coll.isTrigger) return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + boxCenterOffset, boxSize);
    }
}
