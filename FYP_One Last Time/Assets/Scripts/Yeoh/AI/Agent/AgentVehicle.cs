using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentVehicle : MonoBehaviour
{
    public GameObject owner;
    public NavMeshAgent agent;
    public MoveScript move;
    public TurnScript turn;

    // ============================================================================

    void Awake()
    {
        defaultArrivalRange = arrivalRange;
    }

    // ============================================================================
    
    void Update()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.speed = move.speed;
        agent.acceleration = move.acceleration;
        agent.angularSpeed = turn.turnSpeed;
        agent.stoppingDistance = arrivalRange;        
    }

    // ============================================================================
    
    public Vector3 velocity {get; private set;} = Vector3.zero;

    void FixedUpdate()
    {
        if(!goal) SetGoalToSelf();

        agent.destination = goal.position;

        velocity = GetVelocity();

        // set agent virtual pos to rigidbody pos
        agent.nextPosition = owner.transform.position;
    }

    // ============================================================================

    public Transform goal;

    [Header("Arrival")]
    public bool arrival=true;
    public float arrivalRange=1;
    float defaultArrivalRange;
    public float arrivalSlowingRangeOffset=3;

    // ============================================================================
    
    Vector3 GetVelocity()
    {
        if(!goal) return Vector3.zero;

        float distance = Vector3.Distance(goal.position, owner.transform.position);

        float speed = GetArrivalSpeed(distance);

        Vector3 dir = agent.desiredVelocity.normalized;

        return speed * dir;
    }

    // ============================================================================
    
    float GetArrivalSpeed(float distance)
    {    
        float max_speed = agent.desiredVelocity.magnitude;
        
        if(!arrival) return max_speed;

        if(distance <= arrivalRange) return 0;

        if(arrivalSlowingRangeOffset == 0)
            arrivalSlowingRangeOffset = Mathf.Epsilon;

        float ramped_speed = max_speed * (distance - arrivalRange) / arrivalSlowingRangeOffset;

        float clipped_speed = Mathf.Min(ramped_speed, max_speed);

        return clipped_speed;
    }
    
    // ============================================================================

    public GameObject GetCurrentGoal() => goal.gameObject;

    public void SetGoal(Transform target) => goal = target;

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

    // ============================================================================
    
    public float GetCurrentRange() => arrivalRange;

    public void SetRange(float to) => arrivalRange = to;

    public void RevertRange() => arrivalRange = defaultArrivalRange;

    public bool InRange(Vector3 from, Vector3 target, float range)
    {
        return Vector2.Distance(from, target) <= range;
    }

    public bool InRange(GameObject target, float range)
    {
        if(!target) return false;
        return InRange(owner.transform.position, target.transform.position, range);
    }

    public bool InRange(GameObject target) => InRange(target, GetCurrentRange());

    public bool InRange() => InRange(GetCurrentGoal(), GetCurrentRange());
    
    // ============================================================================

    public float maintainRange=2;

    public bool IsTooClose(GameObject target)
    {
        return InRange(target, maintainRange);
    }

    // ============================================================================

    [Header("Wait")]
    public float selfStoppingRange=.5f;

    public void SetGoalToSelf()
    {
        SetRange(selfStoppingRange);
        SetGoal(owner);
    }

    // ============================================================================

    [Header("Debug")]
    public bool showGizmos;

    public bool showStoppingRangeGizmo=true;
    public Color stoppingRangeGizmoColor = new(0, 1, 0, .25f);

    public bool showSlowingRangeGizmo=true;
    public Color slowingRangeGizmoColor = new(1, 1, 0, .25f);

    public bool showMaintainRangeGizmo=true;
    public Color maintainRangeGizmoColor = new(1, 0, 0, .25f);

    void OnDrawGizmosSelected()
    {
        if(!showGizmos) return;

        if(showStoppingRangeGizmo)
        {
            Gizmos.color = stoppingRangeGizmoColor;
            Gizmos.DrawWireSphere(owner.transform.position, arrivalRange);
        }

        if(showSlowingRangeGizmo)
        {
            Gizmos.color = slowingRangeGizmoColor;
            Gizmos.DrawWireSphere(owner.transform.position, arrivalRange+arrivalSlowingRangeOffset);
        }

        if(showMaintainRangeGizmo)
        {
            Gizmos.color = maintainRangeGizmoColor;
            Gizmos.DrawWireSphere(owner.transform.position, maintainRange);
        }
    }
}
