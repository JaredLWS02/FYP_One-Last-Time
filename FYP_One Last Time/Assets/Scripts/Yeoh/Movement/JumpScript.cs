using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;
    public GroundCheck ground;
    
    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.JumpEvent += OnJump;
        EventM.JumpCutEvent += OnJumpCut;
    }
    void OnDisable()
    {
        EventM.JumpEvent -= OnJump;
        EventM.JumpCutEvent -= OnJumpCut;
    }

    // ============================================================================
    
    [Header("Jump")]
    public float jumpForce=10;
    public AnimSO jumpAnim;

    void OnJump(GameObject who)
    {
        if(who!=owner) return;
        
        // do not check cooling here
        // won't cooldown if got jump buffer for some reason

        if(HasCoyoteTime())
        {
            Jump();
            jumpAnim?.Play(owner);
        }
        else
        {
            DoExtraJump();
            extraJumpAnim?.Play(owner);
        }
    }

    void Jump()
    {
        if(IsCooling()) return;
        DoCooldown();

        ResetCoyoteTime();

        if(!rb.isKinematic)
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        
        rb.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);

        EventM.OnJumped(owner);
    }

    // Jump Cut ============================================================================

    [Space]
    public float jumpCutMult=.5f;

    void OnJumpCut(GameObject who)
    {
        if(who!=owner) return;

        // only if going up
        if(rb.velocity.y>0)
        {
            rb.AddForce(Vector3.down * rb.velocity.y * (1-jumpCutMult), ForceMode.Impulse);

            EventM.OnJumpCutted(owner);
        }
    }

    // ============================================================================
    
    void Update()
    {
        UpdateExtraJumps();
        UpdateCoyoteTime();
    }

    // ============================================================================

    [Header("Extra")]
    public int extraJumps=1;
    int extraJumpsLeft;
    public AnimSO extraJumpAnim;

    void UpdateExtraJumps()
    {
        // Only replenish extra jumps if grounded and jump not cooling
        if(ground.IsGrounded() && !IsCooling())
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

    // ============================================================================

    [Header("Coyote")]
    public float coyoteTime=.2f;
    float coyoteTimeLeft;

    void UpdateCoyoteTime()
    {
        coyoteTimeLeft -= Time.deltaTime;

        // Only replenish coyote time if grounded and jump not cooling
        if(ground.IsGrounded() && !IsCooling())
        {
            coyoteTimeLeft = coyoteTime;
        }
    }

    bool HasCoyoteTime()=> coyoteTimeLeft>0;

    void ResetCoyoteTime() => coyoteTimeLeft=0;
    
    // ============================================================================

    [Header("After Jump")]
    public Timer cooldown;
    public float cooldownTime=.2f;

    void DoCooldown() => cooldown?.StartTimer(cooldownTime);
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();

    // ============================================================================
    
    bool CanJump()
    {
        if(IsCooling()) return false;
        if(extraJumpsLeft<=0) return false;
        return true;
    }

}
