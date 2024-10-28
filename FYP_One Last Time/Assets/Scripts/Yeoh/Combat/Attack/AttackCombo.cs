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
        public AnimPreset attackAnim;
        [Space]
        public AttackSO attackSO;
        [Space]
        public PrefabSpawn attackSpawn;
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
    }
    void OnDisable()
    {
        EventM.ComboEvent -= OnCombo;
        EventM.AttackReleasedEvent -= OnAttackReleased;
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
        RefillResetTime();

        if(randomCombo)
        ChooseCombo(Random.Range(0, comboSteps.Count));
        else
        ChooseCombo(comboIndex);

        attack.TryAttack();
    }

    void ChooseCombo(int i)
    {
        ComboStep step = comboSteps[i];

        attack.attackAnim = step.attackAnim;
        attack.attackSO = step.attackSO;
        attack.attackSpawn = step.attackSpawn;
    }

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
        
    // ============================================================================

    void Update()
    {
        UpdateResetTime();
        UpdateComboCooldown();
    }

    // ============================================================================
    
    [Header("During Combo")]
    public float comboResetTime=.25f;
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
        return attack.IsAttacking();
    }

    // ============================================================================

    [Header("After Combo")]
    public float comboCooldownTime=.25f;
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
}
