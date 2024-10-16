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

    public void UpdateSteer(Vector3 vector)
    {
        turn.UpdateTurn(vector.normalized);

        // never go past max speed
        float speed = Mathf.Clamp(vector.magnitude, 0, move.maxSpeed);

        move.UpdateMove(speed, transform.forward);
        move.UpdateMove(0, transform.right);
    }

    // ============================================================================

    public float GetMoveSpeed()
    {
        return move.maxSpeed;
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
