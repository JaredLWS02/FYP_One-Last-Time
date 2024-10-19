using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public static ColliderManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    // Getters ============================================================================

    public List<Collider> GetColliders(GameObject target)
    {
        List<Collider> colliders = new();

        Collider[] colls = target.GetComponents<Collider>();
        Collider[] childColls = target.GetComponentsInChildren<Collider>();

        foreach(var coll in colls)
        {
            if(!coll.isTrigger) colliders.Add(coll);
        }
        foreach(var coll in childColls)
        {
            if(!coll.isTrigger) colliders.Add(coll);
        }

        return colliders;
    }

    public Vector3 GetTop(GameObject target)
    {
        List<Collider> colliders = GetColliders(target);
        
        if(colliders.Count==0)
        {
            Debug.LogError($"{name}: Couldn't find any Collider on {target.name}");
            return Vector3.zero;
        }

        float highestPoint = float.MinValue;

        foreach(var coll in colliders)
        {
            Vector3 topPoint = coll.bounds.max;

            if(highestPoint < topPoint.y)
                highestPoint = topPoint.y;
        }

        return new Vector3(target.transform.position.x, highestPoint, target.transform.position.z);
    }

    public Vector3 GetCenter(GameObject target)
    {
        List<Collider> colliders = GetColliders(target);

        Vector3 center = Vector3.zero;

        // Calculate the average position of all colliders' centers
        foreach(var coll in colliders)
        {
            center += coll.bounds.center;
        }

        center /= colliders.Count;

        return center;
    }

    // Bounding Box ============================================================================

}
