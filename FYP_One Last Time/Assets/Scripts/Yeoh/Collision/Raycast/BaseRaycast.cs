using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseRaycast : SlowUpdate
{
    [Header("Ray")]
    public Transform rayOrigin;
    public float range=5;
    
    public LayerMask hitLayers;
    public LayerMask targetLayers;
    //public List<string> targetLayerNames = new();
    protected RaycastHit rayHit;

    public bool ignoreTriggers=true;
    public bool onlyRigidbodies;

    [Header("Optional")]
    public Transform target;

    // ============================================================================
    
    public virtual bool HasHit()
    {
        return Physics.Raycast(rayOrigin.position, GetRayDir(), out rayHit, range, hitLayers, QueryTriggerInteraction.Ignore);
    }

    public Vector3 GetRayDir()
    {
        return target ?
            (target.position - rayOrigin.position).normalized :
            rayOrigin.forward;
    }

    // public bool IsInTargetLayer(GameObject who)
    // {
    //     // foreach(var layer_name in targetLayerNames)
    //     // {
    //     //     if(LayerMask.NameToLayer(layer_name) == who.layer)
    //     //     {
    //     //         return true;
    //     //     }
    //     // }
    //     // return false;
    // }
    
    public bool IsInTargetLayer(GameObject who)
    {
        return (targetLayers & (1 << who.layer)) != 0;
    }

    // ============================================================================
    
    GameObject previous_hit = null;
    GameObject current_hit = null;
        
    public override void OnSlowUpdate()
    {
        current_hit = null;

        CheckHitEnter();
        CheckHitStay();
        CheckHitExit();

        previous_hit = current_hit;
    }

    void CheckHitEnter()
    {
        if(!HasHit()) return;

        Collider coll = rayHit.collider;
        if(ignoreTriggers && coll.isTrigger) return;

        Rigidbody rb = coll.attachedRigidbody;
        if(onlyRigidbodies && !rb) return;

        GameObject hitObj = rb ? rb.gameObject : coll.gameObject;
        if(!IsInTargetLayer(hitObj)) return;

        current_hit = hitObj;

        if(previous_hit != current_hit)
        {
            OnHitEnter(hitObj, rayHit);
            HitEnterEvent?.Invoke(hitObj, rayHit);
            uEvents.OnHitEnter?.Invoke(hitObj, rayHit);
            //Debug.Log("Hit Enter");
        }
    }

    public bool IsHitting() => current_hit!=null;
    
    void CheckHitStay()
    {
        if(IsHitting())
        {
            OnHitStay(current_hit, rayHit);
            HitStayEvent?.Invoke(current_hit, rayHit);
            uEvents.OnHitStay?.Invoke(current_hit, rayHit);
            //Debug.Log("Hit Stay");
        }
    }

    void CheckHitExit()
    {
        if(!previous_hit) return;

        if(previous_hit != current_hit)
        {
            OnHitExit(previous_hit, rayHit);
            HitExitEvent?.Invoke(previous_hit, rayHit);
            uEvents.OnHitExit?.Invoke(previous_hit, rayHit);
            //Debug.Log("Hit Exit");
        }
    }

    // ============================================================================

    public virtual void OnHitEnter(GameObject target, RaycastHit rayHit){}
    public virtual void OnHitStay(GameObject target, RaycastHit rayHit){}
    public virtual void OnHitExit(GameObject target, RaycastHit rayHit){}

    // ============================================================================

    public event Action<GameObject, RaycastHit> HitEnterEvent;
    public event Action<GameObject, RaycastHit> HitStayEvent;
    public event Action<GameObject, RaycastHit> HitExitEvent;

    // ============================================================================

    [Serializable]
    public struct UEvents
    {
        public UnityEvent<GameObject, RaycastHit> OnHitEnter;
        public UnityEvent<GameObject, RaycastHit> OnHitStay;
        public UnityEvent<GameObject, RaycastHit> OnHitExit;
    }
    [Header("Unity Events")]
    public UEvents uEvents;

    // ============================================================================

    float base_range;

    void Awake()
    {
        base_range = range;
        OnBaseAwake();
    }

    public virtual void OnBaseAwake(){}

    public void SetDefault()
    {
        range = base_range;
        OnSetDefault();
    }
    
    public virtual void OnSetDefault(){}

    // ============================================================================
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmosColor = new(0, 1, 0, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!rayOrigin) return;

        Vector3 start = rayOrigin.position;
        Vector3 end = start + GetRayDir() * range;

        Gizmos.color = gizmosColor;
        Gizmos.DrawLine(start, end);

        OnBaseDrawGizmos(start, end);
    }

    public virtual void OnBaseDrawGizmos(Vector3 start, Vector3 end){}

}
