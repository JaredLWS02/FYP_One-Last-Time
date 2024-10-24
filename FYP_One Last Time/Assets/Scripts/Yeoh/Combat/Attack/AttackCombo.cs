using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCombo : MonoBehaviour
{
    public GameObject owner;

    public AttackScript attack;

    public string comboName = "Normal Combo";

    // ============================================================================

    [System.Serializable]
    public class ComboStep
    {
        public AttackSO attackSO;
        public PrefabSpawn attackSpawn;
    }
    
    [Space]
    public List<ComboStep> comboSteps = new();
    
    // ============================================================================

    void Update()
    {
        UpdateBuffer();
        TryCombo();

        UpdateResetTime();
        UpdateComboCooldown();
    }

    // ============================================================================

    [Header("Before Combo")]
    public float bufferTime=.2f;
    float bufferLeft;

    public void DoBuffer()
    {
        bufferLeft = bufferTime;
    }

    void UpdateBuffer()
    {
        bufferLeft -= Time.deltaTime;

        if(bufferLeft<0) bufferLeft=0;
    }

    bool HasBuffer()
    {
        return bufferLeft>0;
    }

    void ResetBuffer()
    {
        bufferLeft=0;
    }

    // ============================================================================

    public bool randomCombo;

    int comboIndex=0;

    void TryCombo()
    {
        if(IsAttacking()) return;

        if(!HasBuffer()) return;

        if(IsComboCooling()) return;
        
        DoCombo();
    }

    void DoCombo()
    {
        ResetBuffer();

        RefillResetTime();

        if(randomCombo)
        comboIndex = Random.Range(0, comboSteps.Count);

        ChooseCombo(comboIndex);
    }

    void ChooseCombo(int i)
    {
        ComboStep step = comboSteps[i];

        attack.attackSO = step.attackSO;
        attack.attackSpawn = step.attackSpawn;

        attack.DoBuffer();
    }

    // During Combo ============================================================================

    void OnEnable()
    {
        EventManager.Current.AttackEvent += OnAttack;
    }
    void OnDisable()
    {
        EventManager.Current.AttackEvent -= OnAttack;
    }

    // ============================================================================

    void OnAttack(GameObject attacker, AttackSO attackSO)
    {
        if(attacker!=owner) return;

        AttackSO currentAttackSO = comboSteps[comboIndex].attackSO;

        if(attackSO!=currentAttackSO) return;

        StepComboUp();
    }

    void StepComboUp()
    {
        comboIndex++;

        if(comboIndex >= comboSteps.Count)
        {
            ResetCombo();
            // cooldown before next combo
            DoComboCooldown();
        }
    }    
    
    // ============================================================================
    
    [Header("During Combo")]
    public float comboResetTime=1;
    float comboResetTimeLeft;
    
    void RefillResetTime()
    {
        comboResetTimeLeft = comboResetTime;
    }

    void UpdateResetTime()
    {
        // only tick down if not busy
        if(IsAttacking()) return;

        comboResetTimeLeft -= Time.deltaTime;

        if(comboResetTimeLeft<=0)
        {
            comboResetTimeLeft=0;
            ResetCombo();
        }
    }

    void ResetCombo()
    {
        comboIndex=0;
    }

    // ============================================================================

    bool IsAttacking()
    {
        return attack.isAttacking;
    }

    // ============================================================================

    [Header("After Combo")]
    float comboCooldownTime = 1;
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

        if(comboCooldownLeft<0) comboCooldownLeft=0;
    }

    bool IsComboCooling()
    {
        return comboCooldownLeft>0;
    }

    void CancelComboCooldown()
    {
        comboCooldownLeft=0;
    }
    
    // Cancel ============================================================================

    public void CancelAttackAnim()
    {
        attack.CancelAttackAnim();
    }
}
