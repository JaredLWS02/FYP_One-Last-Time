using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class PlatformEffector3D : MonoBehaviour
{
    Collider myColl;

    // ============================================================================
    
    void Awake()
    {
        myColl = GetComponent<Collider>();

        MakeTrigger();
    }
    
    void OnValidate()
    {
        myColl = GetComponent<Collider>();
        myColl.isTrigger = false;
    }

    void Update()
    {
        ResizeTrigger();
        RemoveNulls();
    }

    // ============================================================================
    
    Collider GetColliderCopy(bool trigger)
    {   
        Collider cloneColl = gameObject.AddComponent(myColl.GetType()) as Collider;
        
        cloneColl.isTrigger = trigger;

        if(myColl is BoxCollider box)
        {
            BoxCollider coll = cloneColl as BoxCollider;
            coll.center = box.center;
            coll.size = box.size;
        }

        else if(myColl is SphereCollider sphere)
        {
            SphereCollider coll = cloneColl as SphereCollider;
            coll.center = sphere.center;
            coll.radius = sphere.radius;
        }

        else if(myColl is CapsuleCollider capsule)
        {
            CapsuleCollider coll = cloneColl as CapsuleCollider;
            coll.center = capsule.center;
            coll.radius = capsule.radius;
            coll.height = capsule.height;
            coll.direction = capsule.direction;
        }

        return cloneColl;
    }

    // ============================================================================

    [Header("Trigger")]
    public float triggerExpansion=2;

    Collider trigger;

    void MakeTrigger()
    {
        trigger = GetColliderCopy(true);

        RecordDefaultTriggerDimensions();
    }

    class DefaultTriggerDimension
    {
        public Vector3 size = Vector3.zero;
        public float radius=0;
        public float height=0;
    }

    DefaultTriggerDimension defaultTriggerDimension = new();

    void RecordDefaultTriggerDimensions()
    {
        if(trigger is BoxCollider box)
        {
            defaultTriggerDimension.size = box.size;
        }
        else if(trigger is SphereCollider sphere)
        {
            defaultTriggerDimension.radius = sphere.radius;
        }
        else if(trigger is CapsuleCollider capsule)
        {
            defaultTriggerDimension.radius = capsule.radius;
            defaultTriggerDimension.height = capsule.height;
        }
    }

    void ResizeTrigger()
    {
        if(trigger is BoxCollider box)
        {
            Vector3 size = defaultTriggerDimension.size + Vector3.one * triggerExpansion;
            box.size = size;
        }
        else if(trigger is SphereCollider sphere)
        {
            float radius = defaultTriggerDimension.radius + triggerExpansion;
            sphere.radius = radius;
        }
        else if(trigger is CapsuleCollider capsule)
        {
            float radius = defaultTriggerDimension.radius + triggerExpansion;
            float height = defaultTriggerDimension.height + triggerExpansion;
            capsule.radius = radius;
            capsule.height = height;
        }
    }

    // ============================================================================

    [HideInInspector]
    public List<Collider> collidersToIgnore = new();

    public void TryAddColliderToIgnore(Collider coll)
    {
        if(!collidersToIgnore.Contains(coll))
            collidersToIgnore.Add(coll);
    }

    public void TryRemoveColliderToIgnore(Collider coll)
    {
        if(collidersToIgnore.Contains(coll))
            collidersToIgnore.Remove(coll);
    }

    bool ShouldIgnoreCollider(Collider coll)
    {
        return collidersToIgnore.Contains(coll);
    }

    void RemoveNulls()
    {
        collidersToIgnore.RemoveAll(item => !item);
    }

    // ============================================================================

    void OnTriggerStay(Collider other)
    {
        if(ShouldIgnoreCollider(other)) return;

        Rigidbody rb = other.attachedRigidbody;
        if(!rb) return;

        bool shouldPass = CanPass(rb); // && !HasPassed(rb.gameObject);

        Physics.IgnoreCollision(myColl, other, shouldPass);
    }

    // ============================================================================

    [Header("One Way")]
    public Vector3 entryDirection = Vector3.up;
    public bool localDirection=true;

    Vector3 PassthroughDirection()
    {
        return localDirection ?
            transform.TransformDirection(entryDirection.normalized):
            entryDirection.normalized;
    }

    bool CanPass(Rigidbody passer_rb)
    {
        Vector3 rb_dir = passer_rb.velocity.normalized;

        float dot = Vector3.Dot(rb_dir, PassthroughDirection());

        return dot>0;
    }

    bool HasPassed(GameObject passer)
    {
        Vector3 center = myColl.bounds.center;
        Vector3 passer_center = ColliderManager.Current.GetCenter(passer);

        Vector3 dir_to_passer = (passer_center - center).normalized;

        float dot = Vector3.Dot(dir_to_passer, PassthroughDirection());

        return dot>0;
    }

    // ============================================================================

    void OnDrawGizmosSelected()
    {
        DrayEntryGizmos();
    }

    void DrayEntryGizmos()
    {
        if(!myColl) return;
        Vector3 center = myColl.bounds.center;

        Vector3 ray_length = PassthroughDirection() * 2;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(center, ray_length);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(center, -ray_length);
    }
}
