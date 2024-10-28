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

    public bool isStunned {get; private set;}

    void OnStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        EventM.OnCancelAttack(owner);
        EventM.OnCancelParry(owner);
        EventM.OnCancelCast(owner);
        EventM.OnCancelStun(owner);

        isStunned=true;

        EventM.OnStunned(owner, attacker, hurtbox, contactPoint);

        PlayStunAnim();
    }
    
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

        PlayCancelAnim();
    }

    [Header("Cancel")]
    public string cancelAnimName = "Cancel Stun";

    void PlayCancelAnim()
    {
        EventM.OnPlayAnim(owner, cancelAnimName, stunAnimLayer, stunAnimBlendTime);
    }
}
