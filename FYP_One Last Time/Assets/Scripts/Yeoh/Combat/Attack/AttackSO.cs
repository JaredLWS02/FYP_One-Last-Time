using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Attack", menuName="SO/Combat/AttackSO")]

public class AttackSO : ScriptableObject
{
    public string Name;
    [TextArea]
    public string description;

    [Header("Animator")]
    public bool hasAttackAnim = true;
    public string animName = "Melee1";
    public int animLayer = 0;
    public float animBlendTime = 0;

    [Header("Dash")]
    public bool dashOnWindUp = false;
    public bool dashOnRelease = true;
    public float dashForce = 10;    
    public Vector3 dashDirection = Vector3.forward;
    public bool localDirection = true;

    [Header("Misc")]
    public float cooldownTime = 0;

    // ============================================================================
    
    [HideInInspector]
    public int ID => GetInstanceID();
}