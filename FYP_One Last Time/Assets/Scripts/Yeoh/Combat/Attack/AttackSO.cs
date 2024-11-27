using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Attack", menuName="SO/Combat/AttackSO")]

public class AttackSO : ScriptableObject
{
    public string Name;
    [TextArea]
    public string description;

    [Header("Attack Anim")]
    public AnimSO anim;

    [Header("Attack Dash")]
    public bool dashOnWindUp = false;
    public float dashOnWindUpForce = 30;
    public Vector3 dashOnWindUpDir = Vector3.forward;
    [Space]
    public bool dashOnRelease = true;
    public float dashOnReleaseForce = 30;
    public Vector3 dashOnReleaseDir = Vector3.forward;
    [Space]
    public bool localDir = true;

    [Header("Attack Move")]
    public RangeAssistCfg rangeAssistCfg;
    public bool windUpRangeAssist;
    public bool releaseRangeAssist=true;

    [Header("Optional")]
    [SerializeField]
    Vector2 cooldown = new(0, .1f);
    public float GetRandomCooldown() => Random.Range(cooldown.x, cooldown.y);

    // ============================================================================
    
    [HideInInspector]
    public int ID => GetInstanceID();
}