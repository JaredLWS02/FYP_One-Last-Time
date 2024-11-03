using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiposteScript : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.ParryEvent += OnParry;
        EventM.CancelParryEvent += OnCancelParry;
    }
    void OnDisable()
    {
        EventM.ParryEvent -= OnParry;
        EventM.CancelParryEvent -= OnCancelParry;
    }

    // On Parry Success ============================================================================

    void OnParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender!=owner) return;

        StartRiposte();
    }

    // ============================================================================
    
    [Header("Riposte Window")]
    public Timer riposteTimer;
    public float riposteTime=.5f;

    void StartRiposte() => riposteTimer.StartTimer(riposteTime);
    public bool IsRiposteActive() => riposteTimer.IsTicking();
    void CancelRiposte() => riposteTimer.FinishTimer();

    // Cancel ============================================================================

    void OnCancelParry(GameObject who)
    {
        if(who!=owner) return;

        if(!IsRiposteActive()) return;

        CancelRiposte();
    }
}
