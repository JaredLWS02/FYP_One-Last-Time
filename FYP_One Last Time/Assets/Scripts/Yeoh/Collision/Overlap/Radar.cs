using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public OverlapScript overlap;

    void OnEnable()
    {
        overlap.EnterEvent += OnEnter;
        overlap.ExitEvent += OnExit;
    }
    void OnDisable()
    {
        overlap.EnterEvent -= OnEnter;
        overlap.ExitEvent -= OnExit;
    }

    // ============================================================================

    public List<GameObject> targets = new();

    void OnEnter(Collider other)
    {
        if(!other) return;
        
        Rigidbody rb = other.attachedRigidbody;

        GameObject target = rb ? rb.gameObject : other.gameObject;

        if(targets.Contains(target)) return;

        targets.Add(target);
    }

    void OnExit(Collider other)
    {
        if(!other) return;

        Rigidbody rb = other.attachedRigidbody;

        GameObject target = rb ? rb.gameObject : other.gameObject;

        if(targets.Contains(target))
        targets.Remove(rb ? rb.gameObject : other.gameObject);
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

    void Update()
    {
        RemoveNulls(targets);
    }

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }
}
