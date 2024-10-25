using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentVelocity))]

public class AgentSideMove : MonoBehaviour
{
    AgentVelocity agentVel;

    void Awake()
    {
        agentVel = GetComponent<AgentVelocity>();
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    void FixedUpdate()
    {
        float dot_x = Vector3.Dot(Vector3.right, agentVel.velocity);
        float dot_y = Vector3.Dot(Vector3.up, agentVel.velocity);

        EventM.OnAgentTryMove(gameObject, new Vector2(dot_x, dot_y));
    }
}
