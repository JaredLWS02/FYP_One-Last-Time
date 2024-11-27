using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AgentVehicle))]
[RequireComponent(typeof(SteerScript))]

public class AgentSteer : MonoBehaviour
{
    AgentVehicle move;
    SteerScript steer;

    void Awake()
    {
        move = GetComponent<AgentVehicle>();
        steer = GetComponent<SteerScript>();
    }

    // ============================================================================

    void FixedUpdate()
    {
        //steer.UpdateSteer(move.get);
    }
}
