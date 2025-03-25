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

    void OnParry(GameObject defender, GameObject attacker, HurtboxSO hurtbox, Vector3 contact_point)
    {
        if(defender!=owner) return;

        Perform(parryAnim);
        Anim3_ReleaseEnd();

        parryEvents.OnParry?.Invoke(contact_point);

        if(parryIFrame)
        EventM.OnTryIFrame(owner, iframeSeconds);

        if(selfKnockback)
        EventM.OnTryKnockback(owner, hurtbox.blockKnockback, contact_point, hurtbox.killsMomentum);

        if(hurtbox.parryStunsOwner)
        {
            EventM.OnParryCountered(defender, attacker, contact_point);
        }

        ParryAOE(defender, attacker);
        SpawnStunbox(contact_point);
    }

    // ============================================================================

    [Header("Parry AOE")]
    public BaseOverlap aoeOverlap;

    void ParryAOE(GameObject defender, GameObject main_attacker)
    {
        if(!aoeOverlap) return;

        aoeOverlap.Check();
        
        var other_attackers = aoeOverlap.GetCurrentOverlaps();

        foreach(var other_attacker in other_attackers)
        {
            if(other_attacker == main_attacker) continue;

            EventM.OnParryCountered(defender, other_attacker, other_attacker.transform.position);
        }
    }

    // ============================================================================

    [Header("Parry Hurtbox")]
    public bool spawnStunbox;
    public PrefabPreset parryStunbox;
    GameObject stunbox;

    void SpawnStunbox(Vector3 contactPoint)
    {
        if(!spawnStunbox) return;

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
