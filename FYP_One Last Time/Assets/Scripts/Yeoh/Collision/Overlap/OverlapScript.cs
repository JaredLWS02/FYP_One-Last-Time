using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OverlapScript : MonoBehaviour
{    
    [Min(0)]
    public float checkInterval=.5f;

    void FixedUpdate()
    {
        if(checkInterval>0)
        {
            if(checking_crt==null)
            {
                checking_crt = StartCoroutine(Checking());
            }
        }
        else
        {
            if(checking_crt!=null)
            {
                StopCoroutine(checking_crt);
                checking_crt = null;
            }

            Check();
        }
    }

    Coroutine checking_crt;

    IEnumerator Checking()
    {
        while(checkInterval>0)
        {
            yield return new WaitForSeconds(checkInterval);
            Check();
        }
    }

    // ============================================================================
    
    [Space]
    public bool ignoreTriggers=true;
    public bool onlyRigidbodies=true;

    List<Collider> previous_colliders = new();
    List<Collider> current_colliders = new();

    void Check()
    {
        CheckOnEnter();
        CheckOnStay();
        CheckOnExit();
    }

    void CheckOnEnter()
    {
        current_colliders.Clear();

        Collider[] colliders = GetOverlap();

        foreach(var coll in colliders)
        {
            if(ignoreTriggers && coll.isTrigger) continue;
            if(onlyRigidbodies && !coll.attachedRigidbody) continue;

            current_colliders.Add(coll);

            // if present in current but missing in previous
            if(!previous_colliders.Contains(coll))
            {
                OnOverlapFirstEnter(coll);
                OnFirstEnter.Invoke();
            }

            OnOverlapEnter(coll);
            OnEnter.Invoke();
        }
    }

    void CheckOnStay()
    {
        if(IsOverlapping())
        {
            OnOverlapStay(current_colliders);
            OnStay.Invoke();
        }
    }

    void CheckOnExit()
    {
        foreach(var prev in previous_colliders)
        {
            // if present in previous but missing in current
            if(!current_colliders.Contains(prev))
            {
                OnOverlapExit(prev);
                OnExit.Invoke();

                if(current_colliders.Count==0)
                {
                    OnOverlapLastExit(prev);
                    OnLastExit.Invoke();
                }
            }
        }
        // Update old to new to prepare for the next round
        previous_colliders = new(current_colliders);
    }

    // ============================================================================

    public virtual void OnOverlapFirstEnter(Collider other)
    {
        
    }

    public virtual void OnOverlapEnter(Collider other)
    {
        
    }

    public virtual void OnOverlapStay(List<Collider> others)
    {
        
    }
    
    public virtual void OnOverlapExit(Collider other)
    {
        
    }

    public virtual void OnOverlapLastExit(Collider other)
    {
        
    }

    // ============================================================================

    [Header("Events")]
    public UnityEvent OnFirstEnter;
    public UnityEvent OnEnter;
    public UnityEvent OnStay;
    public UnityEvent OnExit;
    public UnityEvent OnLastExit;

    // ============================================================================
    
    [Header("Overlap")]
    public Transform origin;
    public Vector3 posOffset = Vector3.zero;
    public LayerMask layers;
    
    public virtual Collider[] GetOverlap()
    {
        return null;
    }

    // ============================================================================

    public bool IsOverlapping()
    {
        return current_colliders.Count > 0;
    }
}
