using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class DashScript : BaseAction
{
    [Header("Dash Script")]
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

        Dash();
    }

    void Dash()
    {
        if(IsCooling()) return;
        DoCooldown();
        
        // action cancelling
        EventM.OnCancelFlipDelay(owner);
        EventM.OnCancelAttack(owner);
        //EventM.OnCancelCast(owner);

        TweenDashSpeed(dashSpeed, accelTime);

        foreach(var toggler in togglers)
        {
            toggler.ToggleIgnoreLayers(true);
        }

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
    
    public float dashSpeed=50;
    float currentDashSpeed;
    [Space]
    public float accelTime=0;
    public float decelTime=.5f;
    [Space]
    public Vector3 dashDir = new(0, .02f, 1);
    public bool localDir=true;

    void FixedUpdate()
    {
        UpdateDashing();
        UpdateGroundCheck();
    }
    
    void UpdateDashing()
    {
        if(dashSpeed==0) return;
        if(currentDashSpeed==0) return;
        if(dashDir==Vector3.zero) return;

        Vector3 dash_dir = localDir ? owner.transform.TransformDirection(dashDir.normalized) : dashDir.normalized;

        Vector3 dash_velocity = currentDashSpeed * dash_dir;

        bool isFullSpeed = currentDashSpeed >= dashSpeed;

        dash_velocity.y = isFullSpeed ? dash_velocity.y : rb.velocity.y;

        // setting velocity instead of using AddForce to takeover gravity
        rb.velocity = dash_velocity;
    }

    // ============================================================================
    
    [Header("After Dash")]
    public AnimSO dashRecoverAnim;
    
    void OnDashingFinished()
    {
        foreach(var toggler in togglers)
        {
            toggler.ToggleIgnoreLayers(false);
        }

        Anim3_ReleaseEnd();

        if(dashRecoverAnim)
        {
            Perform(dashRecoverAnim);
        }
        else OnAnimRecover();

        TweenDashSpeed(0, decelTime);
    }

    // ============================================================================

    Tween dashSpeedTween;

    void TweenDashSpeed(float to, float time)
    {
        dashSpeedTween.Stop();
        if(time>0) dashSpeedTween = Tween.Custom(currentDashSpeed, to, time, onValueChange: newVal => currentDashSpeed=newVal, Ease.OutSine);
        else currentDashSpeed = to;
    }
        
    // ============================================================================
    
    [Header("After Recover")]
    public Timer cooldown;
    public float cooldownTime=.5f;

    void Update()
    {
        cooldown.canTick = !IsPerforming();
    }

    void DoCooldown() => cooldown?.StartTimer(cooldownTime);
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();

    // Cancel ============================================================================

    void OnCancelDash(GameObject who)
    {
        if(who!=owner) return;
        if(!IsPerforming()) return;

        CancelDashing();
        OnDashingFinished();

        CancelAnim();

        EventM.OnDashCancelled(owner);
    }

    // ============================================================================
    
    [Header("Optional")]
    public List<GlobalLayerCollisionToggler> togglers = new();

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
