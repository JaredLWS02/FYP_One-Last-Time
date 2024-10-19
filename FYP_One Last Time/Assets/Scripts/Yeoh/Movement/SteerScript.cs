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
    public Vector3 forwardAxis = new(0, 0, 1);
    public Vector3 rightAxis = new(1, 0, 0);
    public bool localAxis=true;

    public void UpdateSteer(Vector3 vector)
    {
        turn.UpdateTurn(vector.normalized);

        // never go past max speed
        float speed = Mathf.Clamp(vector.magnitude, 0, move.speed);

        Vector3 forward = localAxis ? transform.TransformDirection(forwardAxis) : forwardAxis;
        Vector3 right = localAxis ? transform.TransformDirection(rightAxis) : rightAxis;

        forward.Normalize();
        right.Normalize();

        move.UpdateMove(speed, forward);
        move.UpdateMove(0, right);
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
