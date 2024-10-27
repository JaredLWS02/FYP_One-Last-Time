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
    public float blockDamage=25;
    public float blockKnockback=10;
    [Min(1)]
    public int pierceCount=1;
    
    [Header("Parry")]
    public bool isParryable=true;
    public bool parryStunsAttacker=true;

    public static HurtboxSO CreateInstance(HurtboxSO so)
    {
        HurtboxSO new_SO = ScriptableObject.CreateInstance<HurtboxSO>();

        new_SO.Name = so.Name;
        new_SO.description = so.description;
        new_SO.damage = so.damage;
        new_SO.knockback = so.knockback;
        new_SO.blockDamage = so.blockDamage;
        new_SO.pierceCount = so.pierceCount;
        new_SO.isParryable = so.isParryable;
        new_SO.parryStunsAttacker = so.parryStunsAttacker;

        return new_SO;
    }

    [HideInInspector]
    public int ID => GetInstanceID();
}