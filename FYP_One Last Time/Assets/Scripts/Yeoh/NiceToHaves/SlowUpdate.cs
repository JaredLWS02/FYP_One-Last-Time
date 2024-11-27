using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlowUpdate : MonoBehaviour
{
    public bool enableSlowUpdate=true;
    [Min(0)]
    public float checkInterval=.5f;
    public bool fixedUpdate=true;

    void Update()
    {
        OnUpdate_su();

        if(fixedUpdate) return;

        if(HasInterval()) return;
        
        DoSlowUpdate();
    }

    public virtual void OnUpdate_su(){}

    void FixedUpdate()
    {
        OnFixedUpdate_su();
        
        if(!fixedUpdate) return;

        if(HasInterval()) return;
        
        DoSlowUpdate();
    }

    public virtual void OnFixedUpdate_su(){}

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
        if(!enableSlowUpdate) return;
        
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
