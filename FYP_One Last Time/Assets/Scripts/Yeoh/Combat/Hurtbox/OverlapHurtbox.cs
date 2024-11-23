using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapHurtbox : BaseHurtbox
{
    [Header("Overlap Hurtbox")]
    public BaseOverlap overlap;

    // ============================================================================
    
    protected override void OnBaseEnable()
    {
        overlap.OverlapEnterEvent += OnOverlapEnter;
        overlap.OverlapStayEvent += OnOverlapStay;
        overlap.OverlapExitEvent += OnOverlapExit;
    }
    protected override void OnBaseDisable()
    {
        overlap.OverlapEnterEvent -= OnOverlapEnter;
        overlap.OverlapStayEvent -= OnOverlapStay;
        overlap.OverlapExitEvent -= OnOverlapExit;
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

    void OnOverlapEnter(OverlapHit overlap)
    {
        if(hitMethod != HitMethod.OnEnter) return;

        Overlap(overlap);
    }
    
    void OnOverlapStay(List<OverlapHit> overlaps)
    {
        if(hitMethod != HitMethod.OnStay) return;

        foreach(var overlap in overlaps)
        {
            Overlap(overlap);
        }
    }
    
    void OnOverlapExit(OverlapHit overlap)
    {
        if(hitMethod != HitMethod.OnExit) return;

        Overlap(overlap);
    }

    // ============================================================================
    
    public Transform hurtboxOrigin;

    void Overlap(OverlapHit overlap)
    {
        contactPoint = overlap.coll.ClosestPoint(hurtboxOrigin.position);

        Hit(overlap.obj);
    }
}
