using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackScript))]

public class AttackCombo : MonoBehaviour
{
    AttackScript attack;

    void Awake()
    {
        attack = GetComponent<AttackScript>();
    }

    // ============================================================================

    [Header("Parallel Lists")]
    public List<AttackSO> comboSO_s = new();
    public List<PrefabSpawn> attackSpawns = new();

    public void Attack(int i)
    {
        AttackSO chosenSO = comboSO_s[i];
        PrefabSpawn chosenSpawn = attackSpawns[i];

        attack.attackSO = chosenSO;
        attack.attackSpawn = chosenSpawn;

        attack.PlayAttackAnim();
    }

    // ============================================================================

    bool IsAttacking()
    {
        return attack.isAttacking;
    }

    // ============================================================================

    
}
