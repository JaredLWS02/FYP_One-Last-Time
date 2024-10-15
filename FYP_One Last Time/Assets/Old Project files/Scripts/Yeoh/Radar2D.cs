using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar2D : MonoBehaviour
{
    void Update()
    {
        RemoveNulls(targets);
        Scan();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public float range=5;
    public LayerMask layers;

    public List<GameObject> targets = new();
    public GameObject closest;

    public void Scan()
    {
        targets.Clear();

        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, range, layers);
        
        foreach(Collider2D other in others)
        {
            if(other.attachedRigidbody) //if collider has rigidbody
            {
                targets.Add(other.attachedRigidbody.gameObject);
            }
            //else targets.Add(other.gameObject); 
        }

        closest = GetClosest(targets);
    }

    public GameObject GetClosest(List<GameObject> objects)
    {
        if(objects.Count==0) return null;

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject obj in objects) // go through all detected colliders
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

        foreach(GameObject target in targets)
        {
            if(target && target.tag==tag)
            {
                matches.Add(target);
            }
        }

        return matches;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
