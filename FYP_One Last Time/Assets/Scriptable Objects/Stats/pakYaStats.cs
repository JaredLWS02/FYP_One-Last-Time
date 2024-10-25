using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pak Ya Stats")]
public class pakYaStats : ScriptableObject
{
    //stats
    public float maxhp;
    public float maxMana;
    public float atk;
    //movement
    public float speed;
    public float jumpPow;
    //Dash/Dodge
    public float dashPow;
    public float dashTime;
    public float dashCooldown;
}
