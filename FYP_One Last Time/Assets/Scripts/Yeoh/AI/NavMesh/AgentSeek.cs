using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SteerScript))]

public class AgentSeek : MonoBehaviour
{
    NavMeshAgent agent;
    SteerScript steer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        steer = GetComponent<SteerScript>();
    }

    // ============================================================================
    
    void Update()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.speed = steer.GetMoveSpeed();
        agent.acceleration = steer.GetMoveAcceleration();
        agent.angularSpeed = steer.GetTurnSpeed();
        agent.stoppingDistance = stoppingRange;        
    }

    void FixedUpdate()
    {
        agent.destination = goal ? goal.position : agent.transform.position;

        Vector3 velocity = GetArrivalVelocity(agent.desiredVelocity);

        steer.UpdateSteer(velocity);

        // set agent virtual pos to rigidbody pos
        agent.nextPosition = transform.position;
    }

    // ============================================================================

    public Transform goal;

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
