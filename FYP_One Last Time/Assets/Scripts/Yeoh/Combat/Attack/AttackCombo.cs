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
    }

    // ============================================================================

    int comboIndex=0;

    public void Attack()
    {
        if(IsAttacking()) return;

        RefillResetTimer();

        DoCombo(comboIndex++);

        if(comboIndex >= comboSteps.Count)
            ResetCombo();
    }
    
    // ============================================================================
    
    [Header("Combo Reset")]
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
}
