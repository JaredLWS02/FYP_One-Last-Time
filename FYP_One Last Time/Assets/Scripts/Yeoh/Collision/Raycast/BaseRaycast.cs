using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class BaseRaycast : SlowUpdate
{
    [Header("Base Raycast")]
    public Transform origin;

    // ============================================================================

    public Vector3 GetStartPoint()
    {
        return origin.position;
    }
    public Vector3 GetEndPoint()
    {
        return origin.forward * range + GetStartPoint();
    }
    public Vector3 GetHitEndPoint()
    {
        if(IsRayBlocked(out var hit)) return hit.point;
        return GetEndPoint();
    }

    // ============================================================================

    [Header("Optional")]
    public Transform lookAt;
    public Vector3 lookAtOffset;

    void LookAt()
    {
        if(lookAt)
        origin.LookAt(lookAt.position + lookAtOffset);
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        
        EditorApplication.update -= EditorUpdate;
    }

    void EditorUpdate()
    {
        if(!Application.isPlaying) LookAt();
    }
#endif

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(Application.isPlaying) LookAt();
    }

    // ============================================================================

    public struct RayHit
    {
        public Collider collider;
        public Vector3 point;
        public Vector3 normal;
        public float distance;
    }
    
    public RayHit rayHit {get; protected set;}

    protected RayHit GetRayHit(RaycastHit hit)
    {
        return new RayHit
        {
            collider = hit.collider,
            point = hit.point,
            normal = hit.normal,
            distance = hit.distance,
        };
    }

    // ============================================================================

    float originRadius=.01f;

    public virtual bool IsOriginHit(out Collider[] overlaps)
    {
        overlaps = Physics.OverlapSphere(origin.position, originRadius, hitLayers, QueryTriggerInteraction.Ignore);
        return overlaps.Length>0;
    }

    [Header("Ray")]
    public float range=5;

    public virtual bool IsRayBlocked(out RaycastHit hit)
    {
        return Physics.Raycast(origin.position, origin.forward, out hit, range, hitLayers, QueryTriggerInteraction.Ignore);
    }

    // mainly for ConeRaycast
    public virtual bool IsRayBlocked(Vector3 dir, out RaycastHit hit)
    {
        return Physics.Raycast(origin.position, dir, out hit, range, hitLayers, QueryTriggerInteraction.Ignore);
    }

    public virtual bool IsRayHit(out GameObject ray_obj)
    {
        if(IsRayBlocked(out var hit))
        {
            rayHit = GetRayHit(hit);

            return IsColliderValid(rayHit.collider, out ray_obj);
        }
        ray_obj=null;
        return false;
    }

    bool IsHit(out GameObject hitobj)
    {
        if(IsOriginHit(out var overlaps))
        {
            Collider overlap = overlaps[0];
            Vector3 closestPoint = overlap.ClosestPoint(origin.position);

            rayHit = new RayHit
            {
                collider = overlap,
                point = closestPoint,
                normal = (origin.position - closestPoint).normalized,
                distance = Vector3.Distance(origin.position, closestPoint),
            };

            return IsColliderValid(overlap, out hitobj);
        }
        else return IsRayHit(out hitobj);
    }

    public bool IsHit() => IsHit(out var hitobj);

    // ============================================================================

    [Header("Layers")]
    public LayerMask hitLayers;
    public LayerMask targetLayers;

    public bool IsInTargetLayer(GameObject who)
    {
        return (targetLayers & (1 << who.layer)) != 0;
    }

    // ============================================================================
    
    [Header("Check")]
    public bool ignoreTriggers=true;
    public bool onlyRigidbodies;
    
    protected bool IsColliderValid(Collider coll, out GameObject coll_obj)
    {
        if(ignoreTriggers && coll.isTrigger)
        {
            coll_obj=null;
            return false;
        }

        Rigidbody rb = coll.attachedRigidbody;

        if(onlyRigidbodies && !rb)
        {
            coll_obj=null;
            return false;
        }

        GameObject obj = rb ? rb.gameObject : coll.gameObject;

        if(!IsInTargetLayer(obj))
        {
            coll_obj=null;
            return false;
        }

        coll_obj=obj;
        return true;
    }

    // ============================================================================
    
    GameObject previous_hit = null;
    GameObject current_hit = null;
    bool hasExited;
        
    protected override void OnSlowUpdate()
    {
        Check();
    }

    public void Check()
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

        if(!IsHit(out var hitobj)) return;

        current_hit = hitobj;

        if(previous_hit != current_hit)
        {
            OnHitEnter(current_hit, rayHit);
            HitEnterEvent?.Invoke(current_hit, rayHit);
            raycastEvents.OnHitEnter?.Invoke(current_hit, rayHit);

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
        }
    }

    // ============================================================================

    public virtual void OnHitEnter(GameObject target, RayHit rayHit){}
    public virtual void OnHitStay(GameObject target, RayHit rayHit){}
    public virtual void OnHitExit(GameObject target, RayHit rayHit){}

    // ============================================================================

    public event Action<GameObject, RayHit> HitEnterEvent;
    public event Action<GameObject, RayHit> HitStayEvent;
    public event Action<GameObject, RayHit> HitExitEvent;

    // ============================================================================

    [Serializable]
    public struct RaycastEvents
    {
        public UnityEvent<GameObject, RayHit> OnHitEnter;
        public UnityEvent<GameObject, RayHit> OnHitStay;
        public UnityEvent<GameObject, RayHit> OnHitExit;
    }
    [Header("Raycast Events")]
    public RaycastEvents raycastEvents = new();

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
        OnBaseSetDefault();
    }
    
    public virtual void OnBaseSetDefault(){}
    
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
        Vector3 end = start + origin.forward * range;

        Gizmos.color = IsHit() ? gizmoHitColor : gizmoColor;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawLine(rayHit.point, rayHit.point + rayHit.normal*.5f);
        
        OnBaseDrawRayGizmos(start, end);
        OnBaseDrawOriginGizmos(start);
    }

    public virtual void OnBaseDrawRayGizmos(Vector3 start, Vector3 end){}

    public virtual void OnBaseDrawOriginGizmos(Vector3 origin)
    {
        Gizmos.DrawSphere(origin, originRadius);
    }

}
