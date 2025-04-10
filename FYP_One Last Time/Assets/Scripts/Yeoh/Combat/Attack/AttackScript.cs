using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ============================================================================

[System.Serializable]
public struct AttackPresetEvents
{
    public UnityEvent WindUp;
    public UnityEvent ReleaseStart;
    public UnityEvent ReleaseEnd;
    public UnityEvent Recover;
    public UnityEvent Cancel;
}

// ============================================================================

[System.Serializable]
public class AttackPreset
{
    public AttackSO attackSO;
    [Space]
    public PrefabPreset hurtboxPrefab;
    [Space]
    public RangeAssist rangeAssist;
    [Space]
    public AttackPresetEvents events;
}

// ============================================================================

public class AttackScript : BaseAction
{
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.CancelAttackEvent += OnCancelAttack;
        EventM.InterruptAttackEvent += OnInterruptAttack;
    }
    void OnDisable()
    {
        EventM.CancelAttackEvent -= OnCancelAttack;
        EventM.InterruptAttackEvent -= OnInterruptAttack;

        DespawnHurtbox();
    }

    // ============================================================================
    
    void Update()
    {
        if(IsPerforming())
        EventM.OnCancelFlipDelay(owner);
    }
    
    // ============================================================================
    
    [Header("On Attack")]
    public AttackPreset attackPreset;

    public void ChoosePreset(AttackPreset preset) => attackPreset = preset;
    
    public void TryAttack()
    {
        if(IsPerforming()) return;

        if(IsCooling()) return;
        if(IsPostInterruptAttackCooling()) return;

        EventM.OnCancelFlipDelay(owner);

        Perform(attackPreset.attackSO.anim);

        EventM.OnAttacked(owner, attackPreset.attackSO);
    }

    // ============================================================================

    // Anim Event
    public override void OnAnimWindUp()
    {
        AttackSO atk = attackPreset.attackSO;

        if (atk.dashOnWindUp)
        Dash(atk.dashOnWindUpForce, atk.dashOnWindUpDir);

        if(atk.windUpRangeAssist)
        attackPreset.rangeAssist?.CheckRange(atk.rangeAssistCfg);

        EventM.OnAttackWindedUp(owner, atk);

        attackEvents.WindUp?.Invoke($"{atk.Name} Wind Up");
        attackPreset.events.WindUp?.Invoke();
    }  
    // Anim Event
    public override void OnAnimReleaseStart()
    {
        AttackSO atk = attackPreset.attackSO;

        if(atk.dashOnRelease)
        Dash(atk.dashOnReleaseForce, atk.dashOnReleaseDir);

        if(atk.releaseRangeAssist)
        attackPreset.rangeAssist?.CheckRange(atk.rangeAssistCfg);

        SpawnHurtbox();

        EventM.OnAttackReleased(owner, atk);

        attackEvents.ReleaseStart?.Invoke($"{atk.Name} Release Start");
        attackPreset.events.ReleaseStart?.Invoke();
    }
    // Anim Event
    public override void OnAnimReleaseEnd()
    {
        DespawnHurtbox();

        AttackSO atk = attackPreset.attackSO;

        attackEvents.ReleaseEnd?.Invoke($"{atk.Name} Release End");
        attackPreset.events.ReleaseEnd?.Invoke();
    }
    // Anim Event
    public override void OnAnimRecover()
    {
        DoCooldown();

        DespawnHurtbox();

        AttackSO atk = attackPreset.attackSO;

        attackEvents.Recover?.Invoke($"{atk.Name} Recover");
        attackPreset.events.Recover?.Invoke();
    }  

    // ============================================================================

    GameObject hurtbox;

    public void SpawnHurtbox()
    {
        if(!attackPreset.hurtboxPrefab.HasPrefabs()) return;
        
        hurtbox = attackPreset.hurtboxPrefab.Spawn();

        TryAssignHurtboxOwner(hurtbox);
    }

    void TryAssignHurtboxOwner(GameObject target)
    {
        if(target.TryGetComponent(out BaseHurtbox hurtbox))
        {
            hurtbox.owner = owner;
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

        AttackSO atk = attackPreset.attackSO;

        if(atk.localDir)
        direction = owner.transform.TransformDirection(direction);

        rb.AddForce(force * direction, ForceMode.Impulse);
    }

    // ============================================================================

    [Header("After Attack")]
    public Timer cooldown;

    void DoCooldown() => cooldown?.StartTimer(attackPreset.attackSO.GetRandomCooldown());
    bool IsCooling() => cooldown?.IsTicking() ?? false;
    void CancelCooldown() => cooldown?.FinishTimer();

    // Cancel ============================================================================
    
    void OnCancelAttack(GameObject who)
    {
        if(who != owner) return;
        if(!IsPerforming()) return;

        CancelAnim();

        // events
        EventM.OnAttackCancelled(owner);
        AttackSO atk = attackPreset.attackSO;
        attackEvents.Cancel?.Invoke($"{atk.Name} Cancel");
        attackPreset.events.Cancel?.Invoke();
    }

    // ============================================================================

    [Header("After Cancel")]
    public float postInterruptAttackCooldown = 1;
    public Timer postInterruptAttackCooldownTimer;

    void OnInterruptAttack(GameObject attacker)
    {
        if(attacker != owner) return;
        DoPostInterruptAttackCooldown();
    }

    void DoPostInterruptAttackCooldown() => postInterruptAttackCooldownTimer?.StartTimer(postInterruptAttackCooldown);
    bool IsPostInterruptAttackCooling() => postInterruptAttackCooldownTimer?.IsTicking() ?? false;
    void CancelPostInterruptAttackCooldown() => postInterruptAttackCooldownTimer?.FinishTimer();

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
