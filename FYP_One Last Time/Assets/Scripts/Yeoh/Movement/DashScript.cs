using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;    

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.DashEvent += OnDash;
        EventM.CancelDashEvent += OnCancelDash;
    }
    void OnDisable()
    {
        EventM.DashEvent -= OnDash;
        EventM.CancelDashEvent -= OnCancelDash;
    }

    // ============================================================================

    [Header("On Dash")]
    public AnimPreset dashAnim;

    void OnDash(GameObject who)
    {
        if(who!=owner) return;
        
        if(IsDashing()) return;

        if(IsCooling()) return;
        DoCooldown();

        Dash();
    }

    void Dash()
    {
        rb.velocity = Vector3.zero;

        DoDashingTimer();

        dashAnim.Play(owner);

        EventM.OnDashed(owner);
    }

    // During Dash ============================================================================

    void Update()
    {
        UpdateDashingTimer();
        UpdateLayerToggler();
        UpdateCooldown();
    }

    // ============================================================================

    [Header("Dashing")]
    public float dashingSeconds=.2f;
    float dashingLeft;
    
    void DoDashingTimer() => dashingLeft = dashingSeconds;

    bool IsDashing() => dashingLeft>0;

    void UpdateDashingTimer()
    {
        if(!IsDashing()) return;

        dashingLeft -= Time.deltaTime;

        if(dashingLeft<=0)
        {
            StopDashing();
            StartRecovery();
        }
    }

    void CancelDashingTimer() => dashingLeft=0;

    // ============================================================================
    
    void FixedUpdate()
    {
        UpdateDashing();
    }

    // Dashing Force ============================================================================
    
    public float dashForce=10;
    public Vector3 dashDir = Vector3.forward;
    public bool localDir=true;

    void UpdateDashing()
    {
        if(!IsDashing()) return;

        if(dashForce==0) return;
        if(dashDir==Vector3.zero) return;

        Vector3 direction = dashDir.normalized;

        if(localDir)
        direction = transform.TransformDirection(direction);

        rb.AddForce(dashForce * direction, ForceMode.Impulse);
    }
    
    // ============================================================================
    
    [Header("Optional")]
    public ColliderLayerToggler toggler;
    bool wasDashing;

    void UpdateLayerToggler()
    {
        if(!toggler) return;

        if(wasDashing != IsDashing())
        {
            wasDashing = IsDashing();
            toggler.ToggleIgnoreLayers(IsDashing());
        }
    }

    // ============================================================================
    
    [Header("After Dash")]
    public AnimPreset dashRecoverAnim;
    bool isRecovering;

    void StopDashing()
    {
        CancelDashingTimer();

        dashAnim.Cancel(owner);
    }

    void StartRecovery()
    {
        isRecovering=true;

        dashRecoverAnim.Play(owner);
    }

    // ============================================================================

    // Anim Event
    public void DashRecover()
    {
        isRecovering=false;
    }
    // Note: DO NOT PLAY/CANCEL ANY ANIMATIONS IN ON EXIT
    // OTHER ANIMATIONS MIGHT TRY TO TAKE OVER, THUS TRIGGERING ON EXIT,
    // IF GOT ANY PLAY/CANCEL ANIM ON EXIT, IT WILL REPLACE IT
    
    // ============================================================================
    
    public bool IsDashingOrRecovering()
    {
        return IsDashing() || isRecovering;
    }
    
    // ============================================================================
    
    [Header("After Recover")]
    public float cooldownTime=.5f;
    float cooldownLeft;

    void DoCooldown() => cooldownLeft = cooldownTime;

    void UpdateCooldown()
    {
        // only tick down if not busy
        if(IsDashingOrRecovering()) return;
        
        cooldownLeft -= Time.deltaTime;

        if(cooldownLeft<0) cooldownLeft=0;
    }

    bool IsCooling() => cooldownLeft>0;

    void CancelCooldown() => cooldownLeft=0;

    // Cancel ============================================================================

    void OnCancelDash(GameObject who)
    {
        if(who!=owner) return;

        if(!IsDashing()) return;

        StopDashing();

        DashRecover();

        dashRecoverAnim.Cancel(owner);

        EventM.OnDashCancelled(owner);
    }
}
