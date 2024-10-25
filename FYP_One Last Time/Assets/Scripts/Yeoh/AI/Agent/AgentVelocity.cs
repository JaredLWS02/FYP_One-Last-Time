using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MoveScript))]
[RequireComponent(typeof(TurnScript))]

public class AgentVelocity : MonoBehaviour
{
    NavMeshAgent agent;
    MoveScript move;
    TurnScript turn;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        move = GetComponent<MoveScript>();
        turn = GetComponent<TurnScript>();
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

    public Transform goal;
    
    public Vector3 velocity {get; private set;} = Vector3.zero;

    void FixedUpdate()
    {
        agent.destination = goal ? goal.position : agent.transform.position;

        velocity = GetArrivalVelocity(agent.desiredVelocity);

        // set agent virtual pos to rigidbody pos
        agent.nextPosition = agent.transform.position;
    }

    // ============================================================================

    [Header("Arrival")]
    public bool arrival=true;
    public float stoppingRange=1;
    public float slowingRangeOffset=3;

    Vector3 GetArrivalVelocity(Vector3 velocity)
    {
        if(!goal) return Vector3.zero;

        if(!arrival) return velocity;

        float distance = Mathf.Abs(goal.position.x - transform.position.x);

        if(distance <= stoppingRange) return Vector3.zero;

        float max_speed = velocity.magnitude;

        float ramped_speed = max_speed * distance / (stoppingRange+slowingRangeOffset);

        float clipped_speed = Mathf.Min(ramped_speed, max_speed);

        return velocity.normalized * clipped_speed;
    }
}
