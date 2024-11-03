using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : BaseAction
{
    public Rigidbody rb;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.DashEvent += OnDash;
        dashingTimer.TimerFinishedEvent += OnDashingFinished;
        EventM.CancelDashEvent += OnCancelDash;
    }
    void OnDisable()
    {
        EventM.DashEvent -= OnDash;
        dashingTimer.TimerFinishedEvent -= OnDashingFinished;
        EventM.CancelDashEvent -= OnCancelDash;
    }

    // ============================================================================

    [Header("On Dash")]
    public AnimSO dashAnim;

    void OnDash(GameObject who)
    {
        if(who!=owner) return;
        
        if(IsDashing()) return;

        if(ground && dashesLeft<=0) return;

        if(IsCooling()) return;

        Dash();
    }

    void Dash()
    {
        // action cancelling
        EventM.OnCancelAttack(owner);
        EventM.OnCancelCast(owner);

        toggler?.ToggleIgnoreLayers(true);

        if(ground) dashesLeft--;

        DoDashing();

        Perform(dashAnim);
        Anim2_ReleaseStart();

        EventM.OnDashed(owner);
    }
    
    // During Dash ============================================================================

    [Header("Dashing")]
    public Timer dashingTimer;
    public float dashingSeconds=.2f;
    
    void DoDashing() => dashingTimer.StartTimer(dashingSeconds);
    public bool IsDashing() => dashingTimer.IsTicking();
    void CancelDashing() => dashingTimer.FinishTimer();

    // ============================================================================
    
    public float dashVelocity=50;
    public Vector3 dashDir = Vector3.forward;
    public bool localDir=true;

    void FixedUpdate()
    {
        UpdateDashing();
    }
    
    void UpdateDashing()
    {
        if(!IsDashing()) return;

        if(dashVelocity==0) return;
        if(dashDir==Vector3.zero) return;

        Vector3 direction = dashDir.normalized;

        if(localDir)
        direction = transform.TransformDirection(direction);

        rb.velocity = direction * dashVelocity;
        // setting velocity instead of using AddForce
        // to make sure its not affected by gravity
    }

    // ============================================================================
    
    [Header("After Dash")]
    public AnimSO dashRecoverAnim;
    
    void OnDashingFinished()
    {
        toggler?.ToggleIgnoreLayers(false);

        Anim3_ReleaseEnd();

        if(dashRecoverAnim)
        {
            Perform(dashRecoverAnim);
        }
        else OnAnimRecover();
    }
    
    // ============================================================================

    // Anim Event
    public override void OnAnimRecover()
    {
        DoCooldown();
    }
    
    // ============================================================================
    
    [Header("After Recover")]
    public Timer cooldown;
    public float cooldownTime=.5f;

    void DoCooldown() => cooldown?.StartTimer(cooldownTime);
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();

    // Cancel ============================================================================

    void OnCancelDash(GameObject who)
    {
        if(who!=owner) return;

        if(!IsDashing()) return;

        CancelAnim();

        EventM.OnDashCancelled(owner);
    }

    // ============================================================================
    
    [Header("Optional")]
    public ColliderLayerToggler toggler;

    public GroundCheck ground;
    public int dashCount=1;
    int dashesLeft;

    void Start()
    {
        dashesLeft = dashCount;
    }

    void Update()
    {
        UpdateGroundCheck();
    }

    void UpdateGroundCheck()
    {
        if(!ground) return;
        // Only replenish if grounded and not cooling
        if(ground.IsGrounded() && !IsCooling())
        {
            dashesLeft = dashCount;
        }
    }
}
