using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyviAgent : EnemyAgent
{
    // [Header("Spyvi Seek")]
    // public RandomPicker randomSeekBehaviour;

    // ============================================================================

    // [Header("Spyvi Flee")]
    // public HPManager hpM;
    // public float fleeHPPercent=25;
    // public bool ShouldFlee() => hpM.GetHPPercent() <= fleeHPPercent;
    public bool ShouldFlee() => false;
    // public RandomPicker randomFleeBehaviour;
}
