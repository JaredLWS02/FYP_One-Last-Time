using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackScript : BaseAction
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.CancelAttackEvent += OnCancelAttack;
    }
    void OnDisable()
    {
        EventM.CancelAttackEvent -= OnCancelAttack;

        DespawnHurtbox();
    }

    // ============================================================================
    
    [Header("On Attack")]
    public AttackSO attackSO;
    [Space]
    public PrefabPreset hurtboxPrefab;
    
    public void TryAttack()
    {
        if(IsPerforming()) return;

        if(IsCooling()) return;

        EventM.OnCancelFlipDelay(owner);

        Perform(attackSO.anim);

        EventM.OnAttacked(owner, attackSO);
    }

    // ============================================================================

    [Header("Attack Move")]
    public RangeAssist rangeAssist;

    // Anim Event
    public override void OnAnimWindUp()
    {
        if(attackSO.dashOnWindUp)
        Dash(attackSO.dashOnWindUpForce, attackSO.dashOnWindUpDir);

        if(attackSO.windUpRangeAssist)
        rangeAssist?.CheckRange(attackSO.rangeAssistCfg);

        EventM.OnAttackWindedUp(owner, attackSO);

        attackEvents.WindUp?.Invoke($"{attackSO.Name} Wind Up");
    }  
    // Anim Event
    public override void OnAnimReleaseStart()
    {
        if(attackSO.dashOnRelease)
        Dash(attackSO.dashOnReleaseForce, attackSO.dashOnReleaseDir);

        if(attackSO.releaseRangeAssist)
        rangeAssist?.CheckRange(attackSO.rangeAssistCfg);

        SpawnHurtbox();

        EventM.OnAttackReleased(owner, attackSO);

        attackEvents.ReleaseStart?.Invoke($"{attackSO.Name} Release Start");
    }
    // Anim Event
    public override void OnAnimReleaseEnd()
    {
        DespawnHurtbox();

        attackEvents.ReleaseEnd?.Invoke($"{attackSO.Name} Release End");
    }
    // Anim Event
    public override void OnAnimRecover()
    {
        DoCooldown();

        DespawnHurtbox();

        attackEvents.Recover?.Invoke($"{attackSO.Name} Recover");
    }  

    // ============================================================================

    GameObject hurtbox;

    public void SpawnHurtbox()
    {
        hurtbox = hurtboxPrefab.Spawn();

        TryAssignHurtboxOwner(hurtbox);
    }

    void TryAssignHurtboxOwner(GameObject target)
    {
        if(target.TryGetComponent<Hurtbox>(out var hurtbox))
        {
            hurtbox.owner = owner;
        }
        if(target.TryGetComponent<ExplosionHurtbox>(out var e_hurtbox))
        {
            e_hurtbox.owner = owner;
        }
    }

    void DespawnHurtbox()
    {
        if(hurtbox) Destroy(hurtbox);
    }

    // ============================================================================
    
    [Header("Attack Dash")]
    public Rigidbody rb;

    void Dash(float force, Vector3 dir)
    {
        if(!rb) return;
        
        if(force==0) return;
        if(dir==Vector3.zero) return;

        Vector3 direction = dir.normalized;

        if(attackSO.localDir)
        direction = transform.TransformDirection(direction);

        rb.AddForce(force * direction, ForceMode.Impulse);
    }

    // ============================================================================

    [Header("After Attack")]
    public Timer cooldown;

    void DoCooldown() => cooldown?.StartTimer(attackSO.GetRandomCooldown());
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();

    // Cancel ============================================================================
    
    void OnCancelAttack(GameObject who)
    {
        if(who!=owner) return;

        if(!IsPerforming()) return;

        CancelAnim();

        EventM.OnAttackCancelled(owner);

        attackEvents.Cancel?.Invoke($"{attackSO.Name} Cancel");
    }

    // ============================================================================

    [System.Serializable]
    public struct AttackEvents
    {
        public UnityEvent<string> WindUp;
        public UnityEvent<string> ReleaseStart;
        public UnityEvent<string> ReleaseEnd;
        public UnityEvent<string> Recover;
        public UnityEvent<string> Cancel;
    }
    [Space]
    public AttackEvents attackEvents;
}
