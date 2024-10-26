using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Hurtbox", menuName="SO/Combat/HurtboxSO")]

public class HurtboxSO : ScriptableObject
{
    public string Name;
    [TextArea]
    public string description;

    [Header("Hit")]
    public float damage=25;
    public float knockback=10;
    public float damageBlock=25;
    [Min(1)]
    public int pierceCount=1;
    
    [Header("Parry")]
    public bool parryable=true;
    public bool parryStunsAttacker=true;

    [Header("Stun")]
    public float stunSeconds=.5f;
    public float stunSpeedMult=.3f;      

    // ctor
    public HurtboxSO(HurtboxSO so)
    {
        Name = so.Name;
        description = so.description;
        damage = so.damage;
        knockback = so.knockback;
        damageBlock = so.damageBlock;
        parryable = so.parryable;
        pierceCount = so.pierceCount;
        stunSeconds = so.stunSeconds;
        stunSpeedMult = so.stunSpeedMult;
    }

    [HideInInspector]
    public int ID => GetInstanceID();
}