using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveScript))]
[RequireComponent(typeof(TurnScript))]

public class SteerScript : MonoBehaviour
{
    MoveScript move;
    TurnScript turn;

    void Awake()
    {
        move = GetComponent<MoveScript>();
        turn = GetComponent<TurnScript>();
    }

    // ============================================================================

    [Header("Steer Direction")]
    public Vector3 forwardAxis = Vector3.forward;
    public Vector3 rightAxis = Vector3.right;
    public bool localAxis=true;

    public void UpdateSteer(Vector3 velocity)
    {
        Vector3 forward = localAxis ? transform.TransformDirection(forwardAxis) : forwardAxis;
        Vector3 right = localAxis ? transform.TransformDirection(rightAxis) : rightAxis;

        forward.Normalize();
        right.Normalize();

        move.UpdateMove(velocity.magnitude, forward);
        move.UpdateMove(0, right);

        turn.UpdateTurn(velocity.normalized);
    }

    // ============================================================================

    public float GetMoveSpeed()
    {
        return move.speed;
    }
    public float GetMoveAcceleration()
    {
        return move.acceleration;
    }
    public float GetTurnSpeed()
    {
        return turn.turnSpeed;
    }
}
