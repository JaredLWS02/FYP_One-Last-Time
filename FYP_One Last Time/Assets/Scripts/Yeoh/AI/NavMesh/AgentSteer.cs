using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AgentVelocity))]
[RequireComponent(typeof(SteerScript))]

public class AgentSteer : MonoBehaviour
{
    AgentVelocity move;
    SteerScript steer;

    void Awake()
    {
        move = GetComponent<AgentVelocity>();
        steer = GetComponent<SteerScript>();
    }

    // ============================================================================

    void FixedUpdate()
    {
        //steer.UpdateSteer(move.get);
    }
}
