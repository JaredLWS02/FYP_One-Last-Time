using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentVelocity))]

public class AgentSideMove : MonoBehaviour
{
    AgentVelocity agentV;

    void Awake()
    {
        agentV = GetComponent<AgentVelocity>();
    }

    // ============================================================================

    void FixedUpdate()
    {
        float dot_x = Vector3.Dot(Vector3.right, agentV.velocity);
        float dot_y = Vector3.Dot(Vector3.up, agentV.velocity);

        EventManager.Current.OnTryMoveX(gameObject, dot_x);
        EventManager.Current.OnTryMoveY(gameObject, dot_y);
    }
}
