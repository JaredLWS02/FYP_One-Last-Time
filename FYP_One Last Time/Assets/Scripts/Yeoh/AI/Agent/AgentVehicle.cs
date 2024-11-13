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
        def_stoppingRange = stoppingRange;
    }

    // ============================================================================
    
    void Update()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.speed = move.speed;
        agent.acceleration = move.acceleration;
        agent.angularSpeed = turn.turnSpeed;
        agent.stoppingDistance = stoppingRange;        
    }

    // ============================================================================
    
    public Vector3 velocity {get; private set;} = Vector3.zero;

    void FixedUpdate()
    {
        if(!goal) SetGoalToSelf();

        agent.destination = goal.position;

        velocity = GetArrivalVelocity(agent.desiredVelocity);

        // set agent virtual pos to rigidbody pos
        agent.nextPosition = owner.transform.position;
    }

    // ============================================================================

    [Header("Arrival")]
    public Transform goal;
    public bool arrival=true;
    public float stoppingRange=1;
    float def_stoppingRange;
    public float slowingRangeOffset=3;

    Vector3 GetArrivalVelocity(Vector3 velocity)
    {
        if(!goal) return Vector3.zero;

        if(!arrival) return velocity;

        float distance = Mathf.Abs(goal.position.x - owner.transform.position.x);

        if(distance <= stoppingRange) return Vector3.zero;

        float max_speed = velocity.magnitude;

        float ramped_speed = max_speed * distance / (stoppingRange+slowingRangeOffset);

        float clipped_speed = Mathf.Min(ramped_speed, max_speed);

        return velocity.normalized * clipped_speed;
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
    
    public float GetCurrentRange() => stoppingRange;

    public void SetRange(float to) => stoppingRange = to;

    public void RevertRange() => stoppingRange = def_stoppingRange;

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
            Gizmos.DrawWireSphere(owner.transform.position, stoppingRange);
        }

        if(showSlowingRangeGizmo)
        {
            Gizmos.color = slowingRangeGizmoColor;
            Gizmos.DrawWireSphere(owner.transform.position, stoppingRange+slowingRangeOffset);
        }

        if(showMaintainRangeGizmo)
        {
            Gizmos.color = maintainRangeGizmoColor;
            Gizmos.DrawWireSphere(owner.transform.position, maintainRange);
        }
    }
}
