using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHurtbox : BaseHurtbox
{
    [Header("Raycast Hurtbox")]
    public BaseRaycast ray;

    // ============================================================================
    
    protected override void OnBaseEnable()
    {
        ray.HitEnterEvent += OnHitEnter;
        ray.HitStayEvent += OnHitStay;
        ray.HitExitEvent += OnHitExit;
    }
    protected override void OnBaseDisable()
    {
        ray.HitEnterEvent -= OnHitEnter;
        ray.HitStayEvent -= OnHitStay;
        ray.HitExitEvent -= OnHitExit;
    }

    // ============================================================================
    
    public enum HitMethod
    {
        OnEnter,
        OnStay,
        OnExit,
    }
    [Header("OnHit")]
    public HitMethod hitMethod = HitMethod.OnStay;

    void OnHitEnter(GameObject target, BaseRaycast.RayHit rayHit)
    {
        if(hitMethod != HitMethod.OnEnter) return;

        RayHit(target, rayHit);
    }
    
    void OnHitStay(GameObject target, BaseRaycast.RayHit rayHit)
    {
        if(hitMethod != HitMethod.OnStay) return;
    
        RayHit(target, rayHit);
    }
    
    void OnHitExit(GameObject target, BaseRaycast.RayHit rayHit)
    {
        if(hitMethod != HitMethod.OnExit) return;

        RayHit(target, rayHit);
    }

    // ============================================================================
    
    void RayHit(GameObject target, BaseRaycast.RayHit rayHit)
    {
        contactPoint = rayHit.point;
        Hit(target);
    }
}
