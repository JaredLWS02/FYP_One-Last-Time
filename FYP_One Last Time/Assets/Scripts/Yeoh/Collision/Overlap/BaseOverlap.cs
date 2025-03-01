using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseOverlap : SlowUpdate
{    
    public Transform origin;
    public Vector3 posOffset = Vector3.zero;
    public LayerMask layers;
    
    public virtual Collider[] GetOverlap()
    {
        return new Collider[0];
    }

    // ============================================================================

    public bool ignoreTriggers=true;
    public bool onlyRigidbodies=true;

    bool IsColliderValid(Collider target_coll, out GameObject obj)
    {
        if(ignoreTriggers && target_coll.isTrigger)
        {
            obj = null;
            return false;
        }

        Rigidbody rb = target_coll.attachedRigidbody;

        if(onlyRigidbodies && !rb)
        {
            obj = null;
            return false;
        }

        GameObject obj_ = rb ? rb.gameObject : target_coll.gameObject;

        obj = obj_;
        return true;
    }  

    // ============================================================================

    //[Header("Debug")]
    Dictionary<GameObject, Collider> current_overlaps = new();
    Dictionary<GameObject, Collider> previous_overlaps = new();

    public override void OnSlowUpdate()
    {
        current_overlaps = new();

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
            if(!coll) continue;
            if(!IsColliderValid(coll, out var obj)) continue;
            
            current_overlaps[obj] = coll;

            // if present in current but missing in previous
            if(!previous_overlaps.ContainsKey(obj))
            {
                // if none previously, this is the first
                if(previous_overlaps.Count==0)
                {
                    OnOverlapFirstEnter(obj, coll);
                    OverlapFirstEnterEvent?.Invoke(obj, coll);
                    overlapEvents.FirstEnter?.Invoke(obj, coll);
                }

                OnOverlapEnter(obj, coll);
                OverlapEnterEvent?.Invoke(obj, coll);
                overlapEvents.Enter?.Invoke(obj, coll);
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
        foreach(var prev_obj in previous_overlaps.Keys)
        {
            // if present in previous but missing in current
            // or its null because got destroyed
            if(!current_overlaps.ContainsKey(prev_obj) || prev_obj==null)
            {
                Collider coll = previous_overlaps[prev_obj];

                OnOverlapExit(prev_obj, coll);
                OverlapExitEvent?.Invoke(prev_obj, coll);
                overlapEvents.Exit?.Invoke(prev_obj, coll);

                if(current_overlaps.Count==0)
                {
                    OnOverlapLastExit(prev_obj, coll);
                    OverlapLastExitEvent?.Invoke(prev_obj, coll);
                    overlapEvents.LastExit?.Invoke(prev_obj, coll);
                }
            }
        }
    }

    // ============================================================================

    public bool IsOverlappingWho(GameObject who) => current_overlaps.ContainsKey(who);
    
    // ============================================================================

    public virtual void OnOverlapFirstEnter(GameObject obj, Collider coll){}
    public virtual void OnOverlapEnter(GameObject obj, Collider coll){}
    public virtual void OnOverlapStay(Dictionary<GameObject, Collider> dict){}
    public virtual void OnOverlapExit(GameObject obj, Collider coll){}
    public virtual void OnOverlapLastExit(GameObject obj, Collider coll){}

    // ============================================================================

    public event Action<GameObject, Collider> OverlapFirstEnterEvent;
    public event Action<GameObject, Collider> OverlapEnterEvent;
    public event Action<Dictionary<GameObject, Collider>> OverlapStayEvent;
    public event Action<GameObject, Collider> OverlapExitEvent;
    public event Action<GameObject, Collider> OverlapLastExitEvent;
    
    // ============================================================================

    [Serializable]
    public struct OverlapEvents
    {
        public UnityEvent<GameObject, Collider> FirstEnter;
        public UnityEvent<GameObject, Collider> Enter;
        public UnityEvent<Dictionary<GameObject, Collider>> Stay;
        public UnityEvent<GameObject, Collider> Exit;
        public UnityEvent<GameObject, Collider> LastExit;
    }
    [Header("Unity Events")]
    public OverlapEvents overlapEvents;
}
