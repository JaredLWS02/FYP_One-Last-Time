using System.Collections;
using System.Collections.Generic;
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
    }

    // ============================================================================
    
    void Start()
    {
        EventM.OnSpawned(owner);
    }
    
    // ============================================================================

    [Header("Goal/Range Manager")]
    public AgentVehicle vehicle;

    public GameObject GetCurrentGoal()
    {
        return vehicle.goal.gameObject;
    }

    public void SetGoal(Transform target)
    {
        vehicle.goal = target;
    }

    public void SetGoal(GameObject target)
    {
        if(target)
        SetGoal(target.transform);
    }

    public void SetGoal(GameObject target, float range)
    {
        SetRange(range);
        SetGoal(target);
    }

    public void SetGoalToSelf()
    {
        SetRange(wanderArrivalRange);
        SetGoal(owner);
    }

    // ============================================================================
    
    public float GetCurrentRange()
    {
        return vehicle.stoppingRange;
    }

    public void SetRange(float to)
    {        
        vehicle.stoppingRange = to;
    }

    public bool InRange(Vector3 from, Vector3 target, float range)
    {
        return Vector2.Distance(from, target) <= range;
    }

    public bool InRange(GameObject target, float range)
    {
        if(!target) return false;
        return InRange(owner.transform.position, target.transform.position, range);
    }

    public bool InRange(GameObject target)
    {
        return InRange(target, GetCurrentRange());
    }

    public bool InRange()
    {
        return InRange(GetCurrentGoal(), GetCurrentRange());
    }

    // Facing ============================================================================

    public void FaceMoveDir()
    {
        float dot_x = Vector3.Dot(Vector3.right, vehicle.velocity);

        EventM.OnAgentTryFlip(owner, dot_x);
    }

    public void FaceTarget(GameObject target)
    {
        if(!target) return;

        Vector3 owner_to_target = (target.transform.position - owner.transform.position).normalized;

        float dot_x = Vector3.Dot(Vector3.right, owner_to_target);

        EventM.OnAgentTryFlip(owner, dot_x);
    }

    public void FaceGoal()
    {
        FaceTarget(GetCurrentGoal());
    }

    // ============================================================================

    [Header("Wander")]
    public bool allowWander=true;
    public AgentWander wander;
    public float wanderArrivalRange=1;

    public void SetGoalToWander()
    {
        SetRange(wanderArrivalRange);
        SetGoal(wander.wanderGoal);
    }

    // ============================================================================

    [Header("Target")]
    public Radar radar;
    public string targetTag = "Player";

    public GameObject GetClosest(string tag)
    {
        return radar.GetClosestTargetWithTag(tag);
    }

    public GameObject GetTarget()
    {
        return GetClosest(targetTag);
    }

    // ============================================================================
    
    public float expandRadarRangeMult = 1.5f;

    public void ExpandRadarRange()
    {
        radar.MultiplyRadarRange(expandRadarRangeMult);
    }

    public void RevertRadarRange()
    {
        radar.RevertRadarRange();
    }

    // ============================================================================
    
    [Header("Seek")]
    public bool allowSeek=true;
    public float seekArrivalRange=3;

    public void SetGoalToSeek()
    {
        SetRange(seekArrivalRange);
        SetGoal(GetTarget());
    }

    public void FaceTarget()
    {
        FaceTarget(GetTarget());
    }

    public float maintainDistance=2;

    public bool IsTargetTooClose()
    {
        return InRange(GetTarget(), maintainDistance);
    }

    public RandomPicker randomSeekBehaviour;

    public string GetRandomSeekBehaviour()
    {
        randomSeekBehaviour.UpdateManualTimer();

        return randomSeekBehaviour.currentOption;
    }

    // ============================================================================
    
    [Header("Flee")]
    public bool allowFlee=true;
    public AgentFlee flee;
    public HPManager hpM;
    public float fleeHPPercent=25;

    public bool ShouldFlee()
    {
        return hpM.GetHPPercent() <= fleeHPPercent;
    }

    public float fleeArrivalRange=1;

    public void SetGoalToFlee()
    {
        SetRange(fleeArrivalRange);
        SetGoal(flee.fleeGoal);
    }

    public RandomPicker randomFleeBehaviour;

    public string GetRandomFleeBehaviour()
    {
        randomFleeBehaviour.UpdateManualTimer();

        return randomFleeBehaviour.currentOption;
    }

    public GameObject GetThreat()
    {
        return flee.threat.gameObject;
    }

    public void SetThreat(Transform target)
    {
        if(!target) return;

        flee.threat = target;
        SetGoalToFlee();
    }

    public void SetThreat(GameObject target)
    {
        if(!target) return;
        
        SetThreat(target.transform);
    }

    public void SetThreatToTarget()
    {
        SetThreat(GetTarget());
    }

    // ============================================================================

    [Header("Return")]
    public bool allowReturn=true;
    public AgentReturn returner;

    public bool ShouldReturn()
    {
        GameObject target = GetTarget();
        if(!target) return false;

        returner.UpdateCheck(target.transform.position);

        return returner.shouldReturn;
    }

    public void SetGoalToReturn()
    {
        SetRange(wanderArrivalRange);
        SetGoal(returner.spawnpoint);
    }

    public bool IsAtSpawnpoint()
    {
        return returner.IsAtSpawnpoint(vehicle.stoppingRange);
    }
    
    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;
    public Color gizmoColor = new(1, 1, 1, .25f);

    public bool showWanderArrivalRangeGizmo = true;
    public bool showSeekArrivalRangeGizmo = true;
    public bool showMaintainDistanceGizmo = true;
    public bool showFleeArrivalRangeGizmo = true;

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        Gizmos.color = gizmoColor;

        if(showWanderArrivalRangeGizmo)
        Gizmos.DrawWireSphere(owner.transform.position, wanderArrivalRange);

        if(showSeekArrivalRangeGizmo)
        Gizmos.DrawWireSphere(owner.transform.position, seekArrivalRange);

        if(showMaintainDistanceGizmo)
        Gizmos.DrawWireSphere(owner.transform.position, maintainDistance);

        if(showFleeArrivalRangeGizmo)
        Gizmos.DrawWireSphere(owner.transform.position, fleeArrivalRange);
    }
}
