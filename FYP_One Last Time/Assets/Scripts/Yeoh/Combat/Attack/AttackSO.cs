using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Attack", menuName="SO/Combat/AttackSO")]

public class AttackSO : ScriptableObject
{
    public string animName;

    [Header("Dash")]
    public bool dashBeforeAttack=false;
    public bool dashOnAttack=true;
    public float dashForce=5;    
    public Vector3 dashDirection=Vector3.forward;
    public bool localDirection=true;

    // ============================================================================
    
    [HideInInspector]
    public int ID => GetInstanceID();
}