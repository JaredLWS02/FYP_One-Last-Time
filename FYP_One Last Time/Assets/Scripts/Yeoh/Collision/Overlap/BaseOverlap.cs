using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseOverlap : SlowUpdate
{   
    [Header("Base Overlap")]
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
    Dictionary<GameObject, Collider> currentOverlaps = new();
    Dictionary<GameObject, Collider> previousOverlaps = new();

    protected override void OnSlowUpdate()
    {
        Check();
    }

    public void Check()
    {
        currentOverlaps = new();

        CheckOnEnter();
        CheckOnStay();
        CheckOnExit();

        // Update old to new to prepare for the next round
        previousOverlaps = new(currentOverlaps);
    }

    void CheckOnEnter()
    {
        Collider[] colliders = GetOverlap();

        foreach(var coll in colliders)
        {
            if(!coll) continue;
            if(!IsColliderValid(coll, out var obj)) continue;
            
            currentOverlaps[obj] = coll;

            // if present in current but missing in previous
            if(!previousOverlaps.ContainsKey(obj))
            {
                // if none previously, this is the first
                if(previousOverlaps.Count==0)
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
            OnOverlapStay(currentOverlaps);
            OverlapStayEvent?.Invoke(currentOverlaps);
            overlapEvents.Stay?.Invoke(currentOverlaps);
        }
    }

    public bool IsOverlapping() => currentOverlaps.Count > 0;

    void CheckOnExit()
    {
        foreach(var prev_obj in previousOverlaps.Keys)
        {
            // if present in previous but missing in current
            // or its null because got destroyed
            if(!currentOverlaps.ContainsKey(prev_obj) || prev_obj==null)
            {
                Collider coll = previousOverlaps[prev_obj];

                OnOverlapExit(prev_obj, coll);
                OverlapExitEvent?.Invoke(prev_obj, coll);
                overlapEvents.Exit?.Invoke(prev_obj, coll);

                if(currentOverlaps.Count==0)
                {
                    OnOverlapLastExit(prev_obj, coll);
                    OverlapLastExitEvent?.Invoke(prev_obj, coll);
                    overlapEvents.LastExit?.Invoke(prev_obj, coll);
                }
            }
        }
    }

    // ============================================================================

    public bool IsOverlappingWho(GameObject who) => currentOverlaps.ContainsKey(who);
    
    // ============================================================================

    public List<GameObject> GetCurrentOverlaps()
    {
        List<GameObject> overlaps = new();

        foreach(var overlap in currentOverlaps.Keys)
        {
            overlaps.Add(overlap);
        }
        return overlaps;
    }

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
