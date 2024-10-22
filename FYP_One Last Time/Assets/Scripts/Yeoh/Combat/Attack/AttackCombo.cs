using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCombo : MonoBehaviour
{
    public AttackScript attack;

    // ============================================================================

    bool IsAttacking()
    {
        return attack.isAttacking;
    }

    // ============================================================================
    
    [Header("Combo")]
    public string comboName = "Light Combo";
    
    [System.Serializable]
    public class ComboStep
    {
        public AttackSO attackSO;
        public PrefabSpawn attackSpawn;
    }
    
    public List<ComboStep> comboSteps = new();

    void DoCombo(int i)
    {
        ComboStep chosenStep = comboSteps[i];

        AttackSO chosenSO = chosenStep.attackSO;
        PrefabSpawn chosenSpawn = chosenStep.attackSpawn;

        attack.attackSO = chosenSO;
        attack.attackSpawn = chosenSpawn;

        attack.PlayAttackAnim();
    }

    // ============================================================================
    
    void Update()
    {
        UpdateResetTimer();
        UpdateComboCooldown();
        UpdateAttackBuffer();

        TryAttack();
    }

    // ============================================================================

    int comboIndex=0;

    void TryAttack()
    {
        if(IsAttacking()) return;

        if(IsComboCooling()) return;

        if(!HasAttackBuffer()) return;

        Attack();
    }

    void Attack()
    {
        ResetAttackBuffer();

        RefillResetTimer();

        DoCombo(comboIndex++);

        if(comboIndex >= comboSteps.Count)
        {
            ResetCombo();
            DoComboCooldown();
        }
    }
    
    // ============================================================================
    
    [Header("During Combo")]
    public float resetTimer=1;
    float resetTimerLeft;
    
    void RefillResetTimer()
    {
        resetTimerLeft = resetTimer;
    }

    void UpdateResetTimer()
    {
        // only tick down if not busy
        if(IsAttacking()) return;

        resetTimerLeft -= Time.deltaTime;

        if(ShouldResetCombo())
            ResetCombo();
    }

    bool ShouldResetCombo()
    {
        return resetTimerLeft<=0;
    }

    void ResetCombo()
    {
        comboIndex=0;
    }
    
    // ============================================================================
    
    [Header("After Combo")]
    public float comboCooldownTime=1;
    float comboCooldownLeft;
    
    void DoComboCooldown()
    {
        comboCooldownLeft = comboCooldownTime;
    }

    void UpdateComboCooldown()
    {
        // only tick down if not busy
        if(IsAttacking()) return;
        
        comboCooldownLeft -= Time.deltaTime;
    }

    bool IsComboCooling()
    {
        return comboCooldownLeft>0;
    }

    void CancelComboCooldown()
    {
        comboCooldownLeft=0;
    }

    // ============================================================================

    [Header("Assist")]
    public float attackBufferTime=.2f;
    float attackBufferLeft;

    public void AttackBuffer()
    {
        attackBufferLeft = attackBufferTime;
    }

    void UpdateAttackBuffer()
    {
        attackBufferLeft -= Time.deltaTime;
    }

    bool HasAttackBuffer()
    {
        return attackBufferLeft>0;
    }

    void ResetAttackBuffer()
    {
        attackBufferLeft = -1;
    }

    // ============================================================================

    public void CancelAttackAnim()
    {
        attack.CancelAttackAnim();
    }
}
