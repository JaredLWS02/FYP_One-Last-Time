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

    void RemoveNulls()
    {
        collidersToIgnore.RemoveAll(item => !item);
    }

    void OnTriggerStay(Collider other)
    {
        if(collidersToIgnore.Contains(other)) return;

        Rigidbody rb = other.attachedRigidbody;
        if(!rb) return;

        Vector3 rb_dir = rb.velocity.normalized;

        float dot = Vector3.Dot(PassthroughDirection(), rb_dir);

        bool shouldPass = dot > 0;

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

    // ============================================================================

    void OnDrawGizmosSelected()
    {
        DrayEntryGizmos();
    }
}
