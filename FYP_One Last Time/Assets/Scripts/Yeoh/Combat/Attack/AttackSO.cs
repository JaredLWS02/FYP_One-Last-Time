using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Attack", menuName="SO/Combat/AttackSO")]

public class AttackSO : ScriptableObject
{
    public string Name;
    [TextArea]
    public string description;

    [Header("Anim")]
    public AnimSO anim;

    [Header("Dash")]
    public bool dashOnWindUp = false;
    public float dashOnWindUpForce = 30;
    public Vector3 dashOnWindUpDir = Vector3.forward;
    [Space]
    public bool dashOnRelease = true;
    public float dashOnReleaseForce = 30;
    public Vector3 dashOnReleaseDir = Vector3.forward;
    [Space]
    public bool localDir = true;

    [Header("Optional")]
    public float cooldownTime = 0;

    // ============================================================================
    
    [HideInInspector]
    public int ID => GetInstanceID();
}