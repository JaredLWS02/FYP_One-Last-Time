using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnParryScript : BaseAction
{
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

    // ============================================================================

    [Header("On Parry")]
    public AnimSO parryAnim;
    public bool parryIFrame=true;
    public float iframeSeconds=.5f;
    public bool selfKnockback=true;

    [Header("Parry Counter")]
    public bool cancelAttackersAttack=true;

    void OnParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(defender!=owner) return;

        Perform(parryAnim);
        Anim3_ReleaseEnd();

        parryEvents.OnParry?.Invoke(contactPoint);

        if(parryIFrame)
        EventM.OnTryIFrame(owner, iframeSeconds);

        if(selfKnockback)
        EventM.OnTryKnockback(owner, hurtbox.blockKnockback, contactPoint, hurtbox.killsMomentum);

        if(cancelAttackersAttack)
        EventM.OnCancelAttack(attacker);

        SpawnStunbox(hurtbox, contactPoint);
    }

    // ============================================================================

    public bool spawnStunbox=true;
    public PrefabPreset parryStunbox;
    GameObject stunbox;

    void SpawnStunbox(HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(!spawnStunbox) return;
        if(!hurtbox.parryStunsOwner) return;

        parryStunbox.spawnPos = contactPoint;
        stunbox = parryStunbox.Spawn();

        TryAssignHurtboxOwner(stunbox);
    }

    void TryAssignHurtboxOwner(GameObject target)
    {
        if(target.TryGetComponent(out BaseHurtbox hurtbox))
        {
            hurtbox.owner = owner;
        }
    }

    void DespawnStunbox()
    {
        if(stunbox) Destroy(stunbox);
    }

    // Cancel ============================================================================

    void OnCancelParry(GameObject who)
    {
        if(who!=owner) return;

        if(!IsPerforming()) return;

        CancelAnim();

        DespawnStunbox();

        EventM.OnParryCancelled(owner);
    }

    // ============================================================================

    [System.Serializable]
    public struct ParryEvents
    {
        public UnityEvent<Vector3> OnParry;
    }
    [Space]
    public ParryEvents parryEvents;
}
