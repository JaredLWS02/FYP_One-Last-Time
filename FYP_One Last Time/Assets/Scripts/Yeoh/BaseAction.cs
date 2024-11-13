using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseAction : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    public enum State
    {
        None,
        WindingUp,
        Releasing,
        Recovering,
    }

    [Header("Action")]
    public State currentState = State.None;

    public bool IsWindingUp() => currentState == State.WindingUp;
    public bool IsReleasing() => currentState == State.Releasing;
    public bool IsRecovering() => currentState == State.Recovering;
    public bool IsPerforming() => IsWindingUp() || IsReleasing() || IsRecovering();
    public bool HasReleased() => IsReleasing() || IsRecovering();
    
    // ============================================================================

    [Header("Anim")]
    [HideInInspector]
    public AnimSO anim;

    public void Perform()
    {
        if(anim)
        {
            anim.Play(owner);
        }
        else
        {
            Anim1_WindUp();
            Anim2_ReleaseStart();
            Anim3_ReleaseEnd();
            Anim4_Recover();
        }
    }

    public void Perform(AnimSO animSO)
    {
        anim = animSO;
        Perform();
    }

    // ============================================================================

    // Anim Event
    public void Anim1_WindUp()
    {
        currentState = State.WindingUp;

        OnAnimWindUp();
        WindUpEvent?.Invoke();
        uEvents.WindUp?.Invoke();
    }

    // Anim Event
    public void Anim2_ReleaseStart()
    {
        currentState = State.Releasing;

        OnAnimReleaseStart();
        ReleaseStartEvent?.Invoke();
        uEvents.ReleaseStart?.Invoke();
    }

    // Anim Event
    public void Anim3_ReleaseEnd()
    {
        currentState = State.Recovering;

        OnAnimReleaseEnd();
        ReleaseEndEvent?.Invoke();
        uEvents.ReleaseEnd?.Invoke();
    }

    // Anim Event
    public void Anim4_Recover()
    {
        currentState = State.None;

        OnAnimRecover();
        RecoverEvent?.Invoke();
        uEvents.Recover?.Invoke();
    }  
    // Note: DO NOT PLAY/CANCEL ANY ANIMATIONS IN ON EXIT
    // OTHER ANIMATIONS MIGHT TRY TO TAKE OVER, THUS TRIGGERING ON EXIT,
    // IF GOT ANY PLAY/CANCEL ANIM ON EXIT, IT WILL REPLACE IT

    // ============================================================================
    
    public void CancelAnim()
    {
        if(!IsPerforming()) return;

        Anim3_ReleaseEnd();
        Anim4_Recover();

        anim?.Cancel(owner);

        OnActionCancel();
        CancelEvent?.Invoke();
        uEvents.Cancel.Invoke();
    }
    
    // ============================================================================
    
    public virtual void OnAnimWindUp(){}
    public virtual void OnAnimReleaseStart(){}
    public virtual void OnAnimReleaseEnd(){}
    public virtual void OnAnimRecover(){}
    public virtual void OnActionCancel(){}
        
    // ============================================================================
    
    public event Action WindUpEvent;
    public event Action ReleaseStartEvent;
    public event Action ReleaseEndEvent;
    public event Action RecoverEvent;
    public event Action CancelEvent;

    // ============================================================================

    [Serializable]
    public struct UEvents
    {
        public UnityEvent WindUp;
        public UnityEvent ReleaseStart;
        public UnityEvent ReleaseEnd;
        public UnityEvent Recover;
        public UnityEvent Cancel;
    }
    
    public UEvents uEvents;
}
