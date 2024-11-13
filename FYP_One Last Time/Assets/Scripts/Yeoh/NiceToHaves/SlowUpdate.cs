using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlowUpdate : MonoBehaviour
{
    [Min(0)]
    public float checkInterval=.5f;
    public bool fixedUpdate=true;

    void Update()
    {
        if(fixedUpdate) return;

        if(HasInterval()) return;
        
        DoSlowUpdate();
    }

    void FixedUpdate()
    {
        if(!fixedUpdate) return;

        if(HasInterval()) return;
        
        DoSlowUpdate();
    }

    // ============================================================================

    bool HasInterval()
    {
        if(checkInterval>0)
        {
            if(checking_crt==null)
            {
                checking_crt = StartCoroutine(Checking());
            }
            return true;
        }
        else
        {
            if(checking_crt!=null)
            {
                StopCoroutine(checking_crt);
                checking_crt = null;
            }
            return false;
        }
    }

    Coroutine checking_crt;

    IEnumerator Checking()
    {
        while(checkInterval>0)
        {
            yield return new WaitForSeconds(checkInterval);
            DoSlowUpdate();
        }
    }

    // ============================================================================
    
    void DoSlowUpdate()
    {
        OnSlowUpdate();
        SlowUpdateEvent?.Invoke();
        uEvents_su.SlowUpdate?.Invoke();
    }
    
    // ============================================================================

    public virtual void OnSlowUpdate(){}

    // ============================================================================

    public event Action SlowUpdateEvent;

    // ============================================================================

    [Serializable]
    public struct UEvents_su
    {
        public UnityEvent SlowUpdate;
    }
    
    public UEvents_su uEvents_su;
}
