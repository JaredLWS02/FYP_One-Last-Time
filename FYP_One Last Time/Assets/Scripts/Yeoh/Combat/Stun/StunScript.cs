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

        EventM.StunRecoverEvent += OnStunRecover;

        EventM.CancelStunEvent += OnCancelStun;
    }
    void OnDisable()
    {
        EventM.StunRecoverEvent -= OnStunRecover;

        EventM.CancelStunEvent -= OnCancelStun;
    }

    // ============================================================================

    public bool isStunned {get; private set;}

    public void Stun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        isStunned=true;

        EventM.OnCancelAttack(gameObject);
        EventM.OnCancelCast(gameObject);
        
        EventM.OnStun(owner, attacker, hurtbox, contactPoint);

        PlayStunAnim();
    }

    // ============================================================================
    
    [Header("Stun Anim")]
    public List<string> stunAnimNames = new();
    public int stunAnimLayer;
    public float stunAnimBlendTime; 

    void PlayStunAnim()
    {
        string stunAnimName = stunAnimNames[Random.Range(0, stunAnimNames.Count)];

        EventM.OnPlayAnim(owner, stunAnimName, stunAnimLayer, stunAnimBlendTime);
    }
    
    // Stun Anim Events ============================================================================

    void OnStunRecover(GameObject who)
    {
        if(who!=owner) return;

        isStunned=false;
    }

    // Cancel ============================================================================

    void OnCancelStun(GameObject who)
    {
        if(who!=owner) return;
        
        CancelAnim();
    }

    [Header("Cancel")]
    public string cancelAnimName = "Cancel Stun";

    void CancelAnim()
    {
        if(!isStunned) return;

        EventM.OnStunRecover(owner);

        EventM.OnPlayAnim(owner, cancelAnimName, stunAnimLayer, stunAnimBlendTime);
    }
}
