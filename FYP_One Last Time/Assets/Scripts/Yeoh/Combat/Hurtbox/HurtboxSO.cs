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
    public float knockback=25;
    public float blockDamage=25;
    public float blockKnockback=25;
    [Min(1)]
    public int pierceCount=1;
    
    [Header("Parry")]
    public bool isParryable=true;
    public bool parryStunsOwner=true;

    [Header("Stun Victim")]
    public bool canStun=true;
    public AnimSO customStunAnim;
    public bool ignorePoise;

    // ============================================================================

    public static HurtboxSO CreateInstance(HurtboxSO so)
    {
        HurtboxSO new_SO = ScriptableObject.CreateInstance<HurtboxSO>();

        new_SO.Name = so.Name;
        new_SO.description = so.description;
        new_SO.damage = so.damage;
        new_SO.knockback = so.knockback;
        new_SO.blockDamage = so.blockDamage;
        new_SO.blockKnockback = so.blockKnockback;
        new_SO.pierceCount = so.pierceCount;
        new_SO.isParryable = so.isParryable;
        new_SO.parryStunsOwner = so.parryStunsOwner;
        new_SO.canStun = so.canStun;
        new_SO.customStunAnim = so.customStunAnim;
        new_SO.ignorePoise = so.ignorePoise;

        return new_SO;
    }

    // ============================================================================

    [HideInInspector]
    public int ID => GetInstanceID();
}