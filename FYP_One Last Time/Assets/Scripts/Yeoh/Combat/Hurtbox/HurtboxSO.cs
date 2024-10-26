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

    public static HurtboxSO CreateInstance(HurtboxSO so)
    {
        HurtboxSO new_SO = ScriptableObject.CreateInstance<HurtboxSO>();

        new_SO.Name = so.Name;
        new_SO.description = so.description;
        new_SO.damage = so.damage;
        new_SO.knockback = so.knockback;
        new_SO.damageBlock = so.damageBlock;
        new_SO.pierceCount = so.pierceCount;
        new_SO.parryable = so.parryable;
        new_SO.parryStunsAttacker = so.parryStunsAttacker;
        new_SO.stunSeconds = so.stunSeconds;
        new_SO.stunSpeedMult = so.stunSpeedMult;

        return new_SO;
    }

    [HideInInspector]
    public int ID => GetInstanceID();
}