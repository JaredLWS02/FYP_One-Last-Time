using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentTargeting : SlowUpdate
{
    [Header("Agent Targeting")]
    public GameObject owner;
    public AgentVehicle vehicle;

    // ============================================================================
    
    [Header("Target")]
    public GameObject forceTarget;
    public GameObject target {get; private set;}

    protected override void Update()
    {
        base.Update();
        
        if(forceTarget) target = forceTarget;
    }

    public void ForceTarget(GameObject who) => forceTarget = who;

    // ============================================================================
    
    [Header("Radar")]
    public Radar radar;
    public string targetTag = "Player";

    protected override void OnSlowUpdate()
    {
        if(forceTarget) return;
        if(!radar) return;

        target = radar.GetClosestTargetWithTag(targetTag);
    }

    // ============================================================================
    
    [Header("Sight")]
    public BaseRaycast sight;

    public bool CanSeeTarget()
    {
        if(!target) return false;
        if(!sight) return true;

        return sight.IsHitting(out var obj) && obj==target;
    }

    // ============================================================================
    
    public float expandRadarRangeMult = 1.5f;

    public void ExpandRadar() => radar?.MultiplyRadarRange(expandRadarRangeMult);
    public void RevertRadar() => radar?.RevertRadarRange();
    
    // ============================================================================

    [Header("Seek")]
    public AgentSideMove move;

    public void FaceTarget() => move?.FaceTarget(target);

    public float targetArrivalRange=4;

    public void SetGoalToTarget()
    {
        vehicle.SetRange(targetArrivalRange);
        vehicle.SetGoal(target);
    }

    public bool IsTargetTooClose() => vehicle?.IsTooClose(target) ?? false;

    // ============================================================================
    
    [Header("Flee")]
    public AgentFlee flee;

    public void SetThreatToTarget() => flee?.SetThreat(target);

    // ============================================================================
    
    [Header("Return")]
    public AgentReturn returner;

    public bool ShouldReturn()
    {
        if(!returner) return false;
        if(!target) return false;

        returner.UpdateCheck(target.transform.position);

        return returner.shouldReturn;
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;
    public Color gizmoColor = new(1, 1, 1, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;
        
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(owner.transform.position, targetArrivalRange);
    }

}
