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

    List<GameObject> previous_colliders = new();
    List<GameObject> current_colliders = new();

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

            Rigidbody rb = coll.attachedRigidbody;
            if(onlyRigidbodies && !rb) continue;

            GameObject obj = rb ? rb.gameObject : coll.gameObject;

            current_colliders.Add(obj);

            // if present in current but missing in previous
            if(!previous_colliders.Contains(obj))
            {
                OnOverlapFirstEnter(obj);
                OverlapFirstEnterEvent?.Invoke(obj);
                uEvents.OverlapFirstEnter?.Invoke(obj);
            }

            OnOverlapEnter(obj);
            OverlapEnterEvent?.Invoke(obj);
            uEvents.OverlapEnter?.Invoke(obj);
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
            // or its null because got destroyed
            if(!current_colliders.Contains(prev) || !prev)
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

    public virtual void OnOverlapFirstEnter(GameObject who){}
    public virtual void OnOverlapEnter(GameObject who){}
    public virtual void OnOverlapStay(List<GameObject> whos){}
    public virtual void OnOverlapExit(GameObject who){}
    public virtual void OnOverlapLastExit(GameObject who){}

    // ============================================================================

    public event Action<GameObject> OverlapFirstEnterEvent;
    public event Action<GameObject> OverlapEnterEvent;
    public event Action<List<GameObject>> OverlapStayEvent;
    public event Action<GameObject> OverlapExitEvent;
    public event Action<GameObject> OverlapLastExitEvent;
    
    // ============================================================================

    [Serializable]
    public struct UEvents
    {
        public UnityEvent<GameObject> OverlapFirstEnter;
        public UnityEvent<GameObject> OverlapEnter;
        public UnityEvent<List<GameObject>> OverlapStay;
        public UnityEvent<GameObject> OverlapExit;
        public UnityEvent<GameObject> OverlapLastExit;
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
