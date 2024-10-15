using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MobStats")]
public class MobStats : ScriptableObject
{
    public Sprite skin;
    public float maxhp;
    public float dmg;
    public float chargeSpd;
    public float minatkcooldown;
    public float maxatkcooldown;
    public float XknockbackForce;
    public float YknockbackForce;
    public LayerMask playerLayers;
}
