using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseOverlap : SlowUpdate
{    
    [Header("Overlap Script")]
    public bool ignoreTriggers=true;
    public bool onlyRigidbodies=true;

    // ============================================================================

    List<Collider> previous_colliders = new();
    List<Collider> current_colliders = new();

    public override void OnSlowUpdate()
    {
        current_colliders.Clear();

        CheckOnEnter();
        CheckOnStay();
        CheckOnExit();

        // Update old to new to prepare for the next round
        previous_colliders = new(current_colliders);
    }

    void CheckOnEnter()
    {
        Collider[] colliders = GetOverlap();

        foreach(var coll in colliders)
        {
            if(ignoreTriggers && coll.isTrigger) continue;
            if(onlyRigidbodies && !coll.attachedRigidbody) continue;

            current_colliders.Add(coll);

            // if present in current but missing in previous
            if(!previous_colliders.Contains(coll))
            {
                OnOverlapFirstEnter(coll);
                OverlapFirstEnterEvent?.Invoke(coll);
                uEvents.OverlapFirstEnter?.Invoke(coll);
            }

            OnOverlapEnter(coll);
            OverlapEnterEvent?.Invoke(coll);
            uEvents.OverlapEnter?.Invoke(coll);
        }
    }

    void CheckOnStay()
    {
        if(IsOverlapping())
        {
            OnOverlapStay(current_colliders);
            OverlapStayEvent?.Invoke(current_colliders);
            uEvents.OverlapStay?.Invoke(current_colliders);
        }
    }

    void CheckOnExit()
    {
        foreach(var prev in previous_colliders)
        {
            // if present in previous but missing in current
            if(!current_colliders.Contains(prev))
            {
                OnOverlapExit(prev);
                OverlapExitEvent?.Invoke(prev);
                uEvents.OverlapExit?.Invoke(prev);

                if(current_colliders.Count==0)
                {
                    OnOverlapLastExit(prev);
                    OverlapLastExitEvent?.Invoke(prev);
                    uEvents.OverlapLastExit?.Invoke(prev);
                }
            }
        }
    }

    // ============================================================================

    public virtual void OnOverlapFirstEnter(Collider other){}
    public virtual void OnOverlapEnter(Collider other){}
    public virtual void OnOverlapStay(List<Collider> others){}
    public virtual void OnOverlapExit(Collider other){}
    public virtual void OnOverlapLastExit(Collider other){}

    // ============================================================================

    public event Action<Collider> OverlapFirstEnterEvent;
    public event Action<Collider> OverlapEnterEvent;
    public event Action<List<Collider>> OverlapStayEvent;
    public event Action<Collider> OverlapExitEvent;
    public event Action<Collider> OverlapLastExitEvent;
    
    // ============================================================================

    [Serializable]
    public struct UEvents
    {
        public UnityEvent<Collider> OverlapFirstEnter;
        public UnityEvent<Collider> OverlapEnter;
        public UnityEvent<List<Collider>> OverlapStay;
        public UnityEvent<Collider> OverlapExit;
        public UnityEvent<Collider> OverlapLastExit;
    }
    [Header("Unity Events")]
    public UEvents uEvents;

    // ============================================================================
    
    [Header("Overlap")]
    public Transform origin;
    public Vector3 posOffset = Vector3.zero;
    public LayerMask layers;
    
    public virtual Collider[] GetOverlap()
    {
        return null;
    }

    // ============================================================================

    public bool IsOverlapping() => current_colliders.Count > 0;
}
