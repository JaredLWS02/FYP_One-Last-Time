using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSideMove : MonoBehaviour
{
    public GameObject owner;
    public AgentVehicle vehicle;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
    }

    // ============================================================================

    void FixedUpdate()
    {
        float dot_x = Vector3.Dot(Vector3.right, vehicle.velocity);
        float dot_y = Vector3.Dot(Vector3.up, vehicle.velocity);

        Vector2 moveInput = new(dot_x, dot_y);

        EventM.OnAgentTryMove(owner, moveInput);
    }
}
