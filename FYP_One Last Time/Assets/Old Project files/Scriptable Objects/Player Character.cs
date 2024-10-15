using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PakAliStats")]
public class PlayerCharacter : ScriptableObject
{
    public float maxhealth;
    public float atkdmg;
    public float atkcooldown;
    public float atkrange;
    public float energy;
}
