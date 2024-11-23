using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct OverlapHit
{
    public GameObject obj;
    public Collider coll;
}

// ============================================================================

public class BaseOverlap : SlowUpdate
{    
    public Transform origin;
    public Vector3 posOffset = Vector3.zero;
    public LayerMask layers;
    
    public virtual Collider[] GetOverlap()
    {
        return null;
    }

    // ============================================================================

    public bool ignoreTriggers=true;
    public bool onlyRigidbodies=true;

    bool IsColliderValid(Collider target_coll, out OverlapHit overlap)
    {
        overlap = default;

        if(ignoreTriggers && target_coll.isTrigger)
        {
            return false;
        }

        Rigidbody rb = target_coll.attachedRigidbody;

        if(onlyRigidbodies && !rb)
        {
            return false;
        }

        GameObject obj_ = rb ? rb.gameObject : target_coll.gameObject;

        overlap = new();
        overlap.obj = obj_;
        overlap.coll = target_coll;
        return true;
    }  

    // ============================================================================

    //[Header("Debug")]
    List<OverlapHit> current_overlaps = new();
    List<OverlapHit> previous_overlaps = new();

    public override void OnSlowUpdate()
    {
        current_overlaps.Clear();

        CheckOnEnter();
        CheckOnStay();
        CheckOnExit();

        // Update old to new to prepare for the next round
        previous_overlaps = new(current_overlaps);
    }

    void CheckOnEnter()
    {
        Collider[] colliders = GetOverlap();

        foreach(var coll in colliders)
        {
            if(!IsColliderValid(coll, out var overlap)) continue;
            
            current_overlaps.Add(overlap);

            // if present in current but missing in previous
            if(!previous_overlaps.Contains(overlap))
            {
                // if none previously, this is the first
                if(previous_overlaps.Count==0)
                {
                    OnOverlapFirstEnter(overlap);
                    OverlapFirstEnterEvent?.Invoke(overlap);
                    overlapEvents.FirstEnter?.Invoke(overlap);
                }

                OnOverlapEnter(overlap);
                OverlapEnterEvent?.Invoke(overlap);
                overlapEvents.Enter?.Invoke(overlap);
            }
        }
    }

    void CheckOnStay()
    {
        if(IsOverlapping())
        {
            OnOverlapStay(current_overlaps);
            OverlapStayEvent?.Invoke(current_overlaps);
            overlapEvents.Stay?.Invoke(current_overlaps);
        }
    }

    public bool IsOverlapping() => current_overlaps.Count > 0;

    void CheckOnExit()
    {
        foreach(var prev in previous_overlaps)
        {
            // if present in previous but missing in current
            // or its null because got destroyed
            if(!current_overlaps.Contains(prev)) // || prev==null)
            {
                OnOverlapExit(prev);
                OverlapExitEvent?.Invoke(prev);
                overlapEvents.Exit?.Invoke(prev);

                if(current_overlaps.Count==0)
                {
                    OnOverlapLastExit(prev);
                    OverlapLastExitEvent?.Invoke(prev);
                    overlapEvents.LastExit?.Invoke(prev);
                }
            }
        }
    }

    // ============================================================================

    public virtual void OnOverlapFirstEnter(OverlapHit overlap){}
    public virtual void OnOverlapEnter(OverlapHit overlap){}
    public virtual void OnOverlapStay(List<OverlapHit> overlaps){}
    public virtual void OnOverlapExit(OverlapHit overlap){}
    public virtual void OnOverlapLastExit(OverlapHit overlap){}

    // ============================================================================

    public event Action<OverlapHit> OverlapFirstEnterEvent;
    public event Action<OverlapHit> OverlapEnterEvent;
    public event Action<List<OverlapHit>> OverlapStayEvent;
    public event Action<OverlapHit> OverlapExitEvent;
    public event Action<OverlapHit> OverlapLastExitEvent;
    
    // ============================================================================

    [Serializable]
    public struct OverlapEvents
    {
        public UnityEvent<OverlapHit> FirstEnter;
        public UnityEvent<OverlapHit> Enter;
        public UnityEvent<List<OverlapHit>> Stay;
        public UnityEvent<OverlapHit> Exit;
        public UnityEvent<OverlapHit> LastExit;
    }
    [Header("Unity Events")]
    public OverlapEvents overlapEvents;
}
