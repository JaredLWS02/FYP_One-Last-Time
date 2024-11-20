using System.Collections;
using System.Collections.Generic;
using PrimeTween;
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
        //EventM.OnCancelCast(owner);

        TweenDashVelocity(dashVelocity, accelTime);

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
    public float accelTime=0;
    public float decelTime=1;
    
    public Vector3 dashDir = new(0, .02f, 1);
    public bool localDir=true;

    void FixedUpdate()
    {
        UpdateDashing();
        UpdateGroundCheck();
    }
    
    float currentDashVelocity;

    void UpdateDashing()
    {
        if(currentDashVelocity==0) return;
        if(dashDir==Vector3.zero) return;

        Vector3 direction = dashDir.normalized;
        if(localDir)
        direction = owner.transform.TransformDirection(direction);

        // setting velocity instead of using AddForce
        // to takeover gravity
        rb.velocity = new
        (
            direction.x * currentDashVelocity,
            // only no gravity when full velocity
            currentDashVelocity >= dashVelocity ? direction.y * currentDashVelocity : rb.velocity.y,
            direction.z * currentDashVelocity
        );
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

        TweenDashVelocity(0, decelTime);
    }

    // ============================================================================

    Tween dashVelocityTween;

    void TweenDashVelocity(float to, float time)
    {
        dashVelocityTween.Stop();
        if(time>0) dashVelocityTween = Tween.Custom(currentDashVelocity, to, time, onValueChange: newVal => currentDashVelocity=newVal, Ease.OutSine);
        else currentDashVelocity = to;
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

        CancelDashing();

        CancelAnim();

        EventM.OnDashCancelled(owner);
    }

    // ============================================================================
    
    [Header("Optional")]
    public ColliderLayerToggler toggler;

    public GroundCheck ground;
    public int dashCount=1;
    int dashesLeft;

    void Awake()
    {
        dashesLeft = dashCount;
    }

    void UpdateGroundCheck()
    {
        if(!ground) return;
        // Only replenish if grounded
        if(ground.IsGrounded())// && !IsCooling())
        {
            dashesLeft = dashCount;
        }
    }
}
