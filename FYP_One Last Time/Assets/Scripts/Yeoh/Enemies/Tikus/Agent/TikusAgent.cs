using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TikusAgent : EnemyAgent
{
    [Header("Tikus Seek")]
    public RandomPicker randomSeekBehaviour;

    // ============================================================================

    [Header("Tikus Flee")]
    public HPManager hpM;
    public float fleeHPPercent=25;
    public bool ShouldFlee() => hpM.GetHPPercent() <= fleeHPPercent;
    public RandomPicker randomFleeBehaviour;
}
