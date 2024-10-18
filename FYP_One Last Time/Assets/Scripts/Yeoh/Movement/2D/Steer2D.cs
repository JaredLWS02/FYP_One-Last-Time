using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Move2D))]
[RequireComponent(typeof(Turn2D))]

public class Steer2D : MonoBehaviour
{
    Move2D move;
    Turn2D turn;

    void Awake()
    {
        move = GetComponent<Move2D>();
        turn = GetComponent<Turn2D>();
    }

    // ============================================================================

    public void UpdateSteer(Vector3 vector)
    {
        turn.UpdateTurn(vector.normalized);

        // never go past max speed
        float speed = Mathf.Clamp(vector.magnitude, 0, move.speed);

        move.UpdateMove(speed, transform.forward);
        move.UpdateMove(0, transform.right);
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
