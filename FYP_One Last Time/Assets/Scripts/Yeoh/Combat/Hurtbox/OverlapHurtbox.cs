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

    void OnOverlapEnter(GameObject obj, Collider coll)
    {
        if(hitMethod != HitMethod.OnEnter) return;

        Overlap(obj, coll);
    }
    
    void OnOverlapStay(Dictionary<GameObject, Collider> dict)
    {
        if(hitMethod != HitMethod.OnStay) return;

        foreach(var obj in dict.Keys)
        {
            Collider coll = dict[obj];

            Overlap(obj, coll);
        }
    }
    
    void OnOverlapExit(GameObject obj, Collider coll)
    {
        if(hitMethod != HitMethod.OnExit) return;

        Overlap(obj, coll);
    }

    // ============================================================================
    
    public Transform hurtboxOrigin;

    void Overlap(GameObject obj, Collider coll)
    {
        if(!obj) return;
        if(!coll) return;

        contactPoint = coll.ClosestPoint(hurtboxOrigin.position);
        Hit(obj);
    }
}
