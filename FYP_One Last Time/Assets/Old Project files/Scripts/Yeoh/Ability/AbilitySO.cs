using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilitySO : ScriptableObject
{
    public AnimatorOverrideController iconAnimOV;

    public string Name;

    [TextArea]
    public string description;

    public float magnitude=20;
    public float cost=10;
    public float castingTime=1;
    public float cooldown=3;
    
    [HideInInspector]
    public int ID => GetInstanceID();

    [Header("Animations")]
    public AnimatorOverrideController castingAnimOV;
    public AnimatorOverrideController castAnimOV;
}
