using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Ability", menuName="SO/Ability/AbilitySO")]

public class AbilitySO : ScriptableObject
{
    public AnimatorOverrideController iconAnimOV;

    public string Name;
    [TextArea]
    public string description;

    [Header("Ability")]
    public float castingTime=1;
    public float magnitude=25;
    public float mpCost=10;
    public float cooldownTime=3;
    
    [Header("Anim")]
    public AnimPreset castingAnim;
    [Space]
    public AnimPreset castAnim;

    [Header("Optional")]
    public bool noCastAnim;

    // ============================================================================
    
    public bool HasEnoughMP(float budget)
    {
        return budget >= mpCost;
    }
    
    // ============================================================================

    [HideInInspector]
    public int ID => GetInstanceID();
}
