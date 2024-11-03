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
        [Space]
        public PrefabPreset attackPrefab;
    }

    [Space]
    public List<ComboStep> comboSteps = new();

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.ComboEvent += OnCombo;
        EventM.AttackReleasedEvent += OnAttackReleased;
        comboResetTimer.TimerFinishedEvent += OnComboResetTimerFinished;
    }
    void OnDisable()
    {
        EventM.ComboEvent -= OnCombo;
        EventM.AttackReleasedEvent -= OnAttackReleased;
        comboResetTimer.TimerFinishedEvent -= OnComboResetTimerFinished;
    }

    // ============================================================================
    
    public bool randomCombo;

    int comboIndex=0;

    void OnCombo(GameObject who, string combo_name)
    {
        if(who!=owner) return;

        if(combo_name != comboName) return;

        if(IsAttacking()) return;

        if(IsComboCooling()) return;
        
        DoCombo();
    }

    void DoCombo()
    {
        RefillComboResetTime();

        if(randomCombo)
        ChooseCombo(Random.Range(0, comboSteps.Count));
        else
        ChooseCombo(comboIndex);

        attack.TryAttack();
    }

    void ChooseCombo(int i)
    {
        ComboStep step = comboSteps[i];

        attack.attackSO = step.attackSO;
        attack.attackPrefab = step.attackPrefab;
    }
    
    // ============================================================================
    
    bool IsAttacking() => attack.IsPerforming();

    // During Combo ============================================================================

    void OnAttackReleased(GameObject attacker, AttackSO attackSO)
    {
        if(attacker!=owner) return;

        AttackSO currentSO = comboSteps[comboIndex].attackSO;

        if(attackSO!=currentSO) return;

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

    void ResetCombo() => comboIndex=0;
        
    // ============================================================================

    void Update()
    {
        comboResetTimer.canTick = !IsAttacking();
        cooldown.canTick = !IsAttacking();
    }

    // ============================================================================
    
    [Header("During Combo")]
    public Timer comboResetTimer;
    public float comboResetTime=.25f;
    
    void RefillComboResetTime() => comboResetTimer?.StartTimer(comboResetTime);
    void OnComboResetTimerFinished() => ResetCombo();

    // ============================================================================

    [Header("After Combo")]
    public Timer cooldown;
    public float comboCooldownTime=.25f;

    void DoComboCooldown() => cooldown?.StartTimer(comboCooldownTime);
    bool IsComboCooling() => cooldown?.IsTicking() ?? false;
    void CancelComboCooldown() => cooldown?.FinishTimer();
}
