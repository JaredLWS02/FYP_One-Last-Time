using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ColliderLayerToggler))]

public class DashScript : MonoBehaviour
{
    public GameObject owner;
    public Rigidbody rb;

    // ============================================================================
    
    ColliderLayerToggler toggler;

    void Awake()
    {
        toggler = GetComponent<ColliderLayerToggler>();
    }

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

        DoDashTimer();

        dashAnim.Play(owner);

        EventM.OnDashed(owner);
    }

    // During Dash ============================================================================

    void Update()
    {
        UpdateDashTimer();
        UpdateLayerToggler();
        UpdateCooldown();
    }

    // ============================================================================

    [Header("Dashing")]
    public float dashSeconds=.2f;
    float dashLeft;
    
    void DoDashTimer() => dashLeft = dashSeconds;

    bool IsDashing() => dashLeft>0;

    void UpdateDashTimer()
    {
        if(!IsDashing()) return;

        dashLeft -= Time.deltaTime;

        if(dashLeft<=0) FinishDash();
    }

    void CancelDashTimer() => dashLeft=0;

    // ============================================================================
    
    void FixedUpdate()
    {
        UpdateDashing();
    }

    // ============================================================================
    
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
    
    bool wasDashing;

    void UpdateLayerToggler()
    {
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

    void FinishDash()
    {
        CancelDashTimer();

        dashAnim.Cancel(owner);

        isRecovering=true;

        dashRecoverAnim.Play(owner);
    }

    // Parry Anim Events ============================================================================

    public void DashRecover()
    {
        isRecovering=false;
    }
    
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

        FinishDash();

        DashRecover();

        dashRecoverAnim.Cancel(owner);

        EventM.OnDashCancelled(owner);
    }
}
