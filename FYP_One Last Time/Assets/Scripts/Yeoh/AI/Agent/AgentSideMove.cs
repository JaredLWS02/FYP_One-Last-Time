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

    // Facing ============================================================================

    public void FaceMoveDir()
    {
        float dot_x = Vector3.Dot(Vector3.right, vehicle.velocity);

        EventM.OnAgentTryFlip(owner, dot_x);
    }

    public void FaceTarget(GameObject target)
    {
        if(!target) return;

        Vector3 owner_to_target = (target.transform.position - owner.transform.position).normalized;

        float dot_x = Vector3.Dot(Vector3.right, owner_to_target);

        EventM.OnAgentTryFlip(owner, dot_x);
    }

    public void FaceGoal()
    {
        FaceTarget(vehicle.goal.gameObject);
    }
}
