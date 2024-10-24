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

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    void FixedUpdate()
    {
        float dot_x = Vector3.Dot(Vector3.right, agentV.velocity);
        float dot_y = Vector3.Dot(Vector3.up, agentV.velocity);

        EventM.OnTryInputMove(gameObject, new Vector2(dot_x, dot_y));
    }
}
