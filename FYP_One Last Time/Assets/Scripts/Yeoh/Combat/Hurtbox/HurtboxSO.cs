using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Attack", menuName="SO/Combat/HurtboxSO")]

public class HurtboxSO : ScriptableObject
{
    public string Name;
    [TextArea]
    public string description;

    [Header("Damage")]
    public float damage=10;
    public float knockback=5;

    [Header("Extra")]
    public float damageBlock=5;
    public bool parryable=true;
    [Min(1)]
    public int pierceCount=1;

    [Header("Stun")]
    public float stunSeconds=1;
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