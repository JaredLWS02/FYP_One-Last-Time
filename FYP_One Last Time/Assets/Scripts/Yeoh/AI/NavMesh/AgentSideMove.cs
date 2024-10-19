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

    public Vector3 worldXAxis = new(1,0,0);
    public Vector3 worldYAxis = new(0,1,0);

    void FixedUpdate()
    {
        float dot_x = Vector3.Dot(worldXAxis, agentV.velocity);
        float dot_y = Vector3.Dot(worldYAxis, agentV.velocity);

        EventManager.Current.OnTryMoveX(gameObject, dot_x);
        EventManager.Current.OnTryFaceX(gameObject, dot_x);
        EventManager.Current.OnTryMoveY(gameObject, dot_y);
    }
}
