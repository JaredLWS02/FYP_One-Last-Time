using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseRaycast : SlowUpdate
{
    [Header("Base Raycast")]
    public Transform origin;
    [Header("Optional")]
    public Transform target;
    
    public Vector3 GetRayDir()
    {
        return target ?
            (target.position - origin.position).normalized :
            origin.forward;
    }

    // ============================================================================

    [Header("Cast")]
    public float range=5;

    public Vector3 GetEndPos(Vector3 startPos, Vector3 dir) => startPos + dir * range;
    public Vector3 GetEndPos(Vector3 startPos) => GetEndPos(startPos, GetRayDir());
    public Vector3 GetEndPos() => GetEndPos(origin.position);

    // ============================================================================
    
    protected RaycastHit rayHit;
    
    public virtual bool RayHit(out GameObject target)
    {
        if(Physics.Raycast(origin.position, GetRayDir(), out rayHit, range, hitLayers, QueryTriggerInteraction.Ignore))
        {
            if(IsHitValid(out var hitObj))
            {
                target = hitObj;
                return true;
            }
        }
        target=null;
        return false;
    }

    bool RayHit() => RayHit(out var target);

    // ============================================================================

    [Header("Layers")]
    public LayerMask hitLayers;
    public LayerMask targetLayers;

    public bool IsInTargetLayer(GameObject obj)
    {
        return (targetLayers & (1 << obj.layer)) != 0;
    }

    // ============================================================================
    
    [Header("Check")]
    public bool ignoreTriggers=true;
    public bool onlyRigidbodies;
    public bool reverseDoubleCheck=true;
    
    protected bool IsHitValid(RaycastHit hit, out GameObject hitObj)
    {
        Collider coll = hit.collider;

        if(ignoreTriggers && coll.isTrigger)
        {
            hitObj=null;
            return false;
        }

        Rigidbody rb = coll.attachedRigidbody;

        if(onlyRigidbodies && !rb)
        {
            hitObj=null;
            return false;
        }

        GameObject obj = rb ? rb.gameObject : coll.gameObject;

        if(!IsInTargetLayer(obj))
        {
            hitObj=null;
            return false;
        }

        // double check forward cast using a reverse cast
        if(reverseDoubleCheck)
        {
            // if it hits, then the ray origin is inside a collider, getting blocked
            if(ReverseDoubleCheckHit())
            {
                hitObj=null;
                return false;
            }
        }

        hitObj=obj;
        return true;
    }

    protected bool IsHitValid(out GameObject hitObj) => IsHitValid(rayHit, out hitObj);

    // ============================================================================

    bool ReverseDoubleCheckHit()
    {
        Vector3 start = rayHit.point;
        Vector3 dir = (origin.position - rayHit.point).normalized;
        float range = Vector3.Distance(origin.position, rayHit.point);

        return Physics.Raycast(start, dir, range, hitLayers, QueryTriggerInteraction.Ignore);
    }

    // ============================================================================
    
    GameObject previous_hit = null;
    GameObject current_hit = null;
    bool hasExited;
        
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
        if(!origin) return;
        if(range<=0) return;

        if(!RayHit(out var target)) return;

        current_hit = target;

        if(previous_hit != current_hit)
        {
            OnHitEnter(target, rayHit);
            HitEnterEvent?.Invoke(target, rayHit);
            raycastEvents.OnHitEnter?.Invoke(target, rayHit);
            //Debug.Log("Hit Enter");

            hasExited=false;
        }
    }

    public bool IsHitting(out GameObject obj)
    {
        obj = current_hit;
        return obj!=null;
    }
    public bool IsHitting() => current_hit!=null;
    
    void CheckHitStay()
    {
        if(IsHitting())
        {
            OnHitStay(current_hit, rayHit);
            HitStayEvent?.Invoke(current_hit, rayHit);
            raycastEvents.OnHitStay?.Invoke(current_hit, rayHit);
            //Debug.Log("Hit Stay");
        }
    }

    void CheckHitExit()
    {
        if(previous_hit != current_hit && !hasExited)
        {
            hasExited=true;
            
            OnHitExit(previous_hit, rayHit);
            HitExitEvent?.Invoke(previous_hit, rayHit);
            raycastEvents.OnHitExit?.Invoke(previous_hit, rayHit);
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
    public struct RaycastEvents
    {
        public UnityEvent<GameObject, RaycastHit> OnHitEnter;
        public UnityEvent<GameObject, RaycastHit> OnHitStay;
        public UnityEvent<GameObject, RaycastHit> OnHitExit;
    }
    [Header("Raycast Events")]
    public RaycastEvents raycastEvents;

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
    public Color gizmoColor = new(1, 1, 1, .5f);
    public Color gizmoHitColor = new(0, 1, 0, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        if(!origin) return;

        Vector3 start = origin.position;
        Vector3 end = start + GetRayDir() * range;

        Gizmos.color = RayHit() ? gizmoHitColor : gizmoColor;
        Gizmos.DrawLine(start, end);

        OnBaseDrawGizmos(start, end);
    }

    public virtual void OnBaseDrawGizmos(Vector3 start, Vector3 end){}

}
