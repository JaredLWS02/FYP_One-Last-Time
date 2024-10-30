using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunScript : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================
    
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.StunEvent += OnStun;
        EventM.CancelStunEvent += OnCancelStun;
    }
    void OnDisable()
    {
        EventM.StunEvent -= OnStun;
        EventM.CancelStunEvent -= OnCancelStun;
    }

    // ============================================================================

    public AnimPreset stunAnim;

    public bool isStunned {get; private set;}

    void OnStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        // action cancelling
        EventM.OnCancelDash(owner);
        EventM.OnCancelAttack(owner);
        EventM.OnCancelParry(owner);
        EventM.OnCancelCast(owner);
        EventM.OnCancelStun(owner);

        isStunned=true;

        stunAnim = hurtbox.stunAnim;

        stunAnim.Play(owner);

        EventM.OnStunned(owner, attacker, hurtbox, contactPoint);
    }
    
    // Stun Anim Events ============================================================================

    public void StunRecover()
    {
        isStunned=false;
    }

    // Cancel ============================================================================

    void OnCancelStun(GameObject who)
    {
        if(who!=owner) return;
        
        if(!isStunned) return;

        StunRecover();

        stunAnim.Cancel(owner);
    }
}
