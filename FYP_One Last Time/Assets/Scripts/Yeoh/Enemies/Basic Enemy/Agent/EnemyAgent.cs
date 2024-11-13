using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    public GameObject owner;
    public Pilot pilot;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.OnSpawned(owner);
    }
    
    // ============================================================================

    public AgentVehicle vehicle;
    public AgentSideMove move; // for facing dir

    [Header("Wander")]
    public AgentWander wander;

    // ============================================================================

    [Header("Targeting")]
    public Radar radar;
    public string targetTag = "Player";
    public float expandRadarRangeMult = 1.5f;

    public GameObject GetTarget() => radar.GetClosestTargetWithTag(targetTag);

    // ============================================================================
    
    public BaseRaycast sight;

    public bool CanSeeTarget()
    {
        GameObject target = GetTarget();
        if(!target) return false;

        if(!sight) return true;

        return sight.IsHitting(out var obj) && obj==target;
    }

    // ============================================================================
    
    public void SetGoalToTarget()
    {
        vehicle.RevertRange();
        vehicle.SetGoal(GetTarget());
    }

    public void FaceTarget() => move.FaceTarget(GetTarget());

    public bool IsTargetTooClose() => vehicle.IsTooClose(GetTarget());

    public RandomPicker randomSeekBehaviour;

    public string GetRandomSeekBehaviour()
    {
        randomSeekBehaviour.UpdateManualTimer();

        return randomSeekBehaviour.currentOption;
    }

    // ============================================================================
    
    [Header("Flee")]
    public AgentFlee flee;
    public HPManager hpM;
    public float fleeHPPercent=25;

    public bool ShouldFlee() => hpM.GetHPPercent() <= fleeHPPercent;

    public void SetThreatToTarget()
    {
        flee.SetThreat(GetTarget());
    }

    public RandomPicker randomFleeBehaviour;

    public string GetRandomFleeBehaviour()
    {
        randomFleeBehaviour.UpdateManualTimer();

        return randomFleeBehaviour.currentOption;
    }

    // ============================================================================

    [Header("Return")]
    public AgentReturn returner;

    public bool ShouldReturn()
    {
        GameObject target = GetTarget();
        if(!target) return false;

        returner.UpdateCheck(target.transform.position);

        return returner.shouldReturn;
    }

}
