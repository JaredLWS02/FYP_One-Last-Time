using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar2D : MonoBehaviour
{
    public float range=5;
    public LayerMask layers;
    public List<GameObject> targets = new();

    void Update()
    {
        RemoveNulls(targets);
        Scan();
    }

    public void Scan()
    {
        targets.Clear();

        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, range, layers);
        
        foreach(var other in others)
        {
            if(other.isTrigger) continue;
            Rigidbody2D rb = other.attachedRigidbody;
            if(!rb) continue;

            targets.Add(rb.gameObject);
        }
    }

    // ============================================================================

    public GameObject GetClosest(List<GameObject> objects)
    {
        if(objects.Count==0) return null;

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach(var obj in objects) // go through all detected colliders
        {
            float distance = Vector3.Distance(obj.transform.position, transform.position);

            if(distance<closestDistance) // find and replace with the nearer one
            {
                closestDistance = distance;
                closest = obj;
            }
        }
        return closest;
    }

    public List<GameObject> GetTargetsWithTag(string tag)
    {
        List<GameObject> matches = new();

        foreach(var target in targets)
        {
            if(target && target.tag==tag)
            {
                matches.Add(target);
            }
        }
        return matches;
    }

    public GameObject GetClosestTargetWithTag(string tag)
    {
        List<GameObject> targets = GetTargetsWithTag(tag);
        return GetClosest(targets);
    }

    // ============================================================================

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;
    public Color gizmoColor = new(0, 1, 1, .5f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
