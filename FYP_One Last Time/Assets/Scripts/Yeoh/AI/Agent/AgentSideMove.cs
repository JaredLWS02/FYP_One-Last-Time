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

    public float? yInputOverride = null;

    void FixedUpdate()
    {
        Vector2 input = vehicle.velocity == Vector3.zero ? 
                        Vector2.zero : vehicle.velocity.normalized;

        if(yInputOverride.HasValue)
            input.y = yInputOverride.Value;

        EventM.OnAgentTryMove(owner, input);
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
