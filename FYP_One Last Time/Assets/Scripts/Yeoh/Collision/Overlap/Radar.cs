using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public SphereOverlap overlap;

    void OnEnable()
    {
        overlap.OverlapEnterEvent += OnOverlapEnter;
        overlap.OverlapExitEvent += OnOverlapExit;
    }
    void OnDisable()
    {
        overlap.OverlapEnterEvent -= OnOverlapEnter;
        overlap.OverlapExitEvent -= OnOverlapExit;
    }

    // ============================================================================

    public List<GameObject> targets = new();

    void OnOverlapEnter(GameObject obj, Collider coll)
    {
        if(!obj) return;
        
        if(targets.Contains(obj)) return;

        targets.Add(obj);
    }

    void OnOverlapExit(GameObject obj, Collider coll)
    {
        if(!obj) return;

        if(!targets.Contains(obj)) return;

        targets.Remove(obj);

        if(obj==closest) closest=null;
    }

    // ============================================================================

    GameObject closest;

    public GameObject GetClosest(List<GameObject> objects)
    {
        closest=null;

        if(objects.Count==0) return null;

        float closestDistance = Mathf.Infinity;

        foreach(var obj in objects)
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

    List<GameObject> matches = new();

    public List<GameObject> GetTargetsWithTag(string tag)
    {
        matches.Clear();

        foreach(var target in targets)
        {
            if(target && target.CompareTag(tag))
            {
                matches.Add(target);
            }
        }
        return matches;
    }

    public GameObject GetClosestTargetWithTag(string tag)
    {
        GetTargetsWithTag(tag);
        return GetClosest(matches);
    }

    // ============================================================================

    void Update()
    {
        RemoveNulls(targets);
        RemoveNulls(matches);
    }

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }

    // ============================================================================

    public void SetRadarRange(float to)
    {
        overlap.range = to;
    }

    public void MultiplyRadarRange(float mult)
    {
        SetRadarRange(overlap.base_range * mult);
    }

    public void RevertRadarRange()
    {
        overlap.range = overlap.base_range;
    }
}
