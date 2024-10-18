using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Move2D))]

public class SideMove2D : MonoBehaviour
{
    Move2D move;
    SideTurn2D turn; // optional

    void Awake()
    {
        move = GetComponent<Move2D>();
        turn = GetComponent<SideTurn2D>();
    }

    // ============================================================================

    float dirX;

    public void OnMoveX(GameObject mover, float input_x)
    {
        if(mover!=gameObject) return;

        dirX = input_x;

        isMoving=true;
    }

    // ============================================================================
    
    bool isMoving;

    void FixedUpdate()
    {
        if(!isMoving) dirX=0;

        Move();

        isMoving=false;
    }

    void Move()
    {
        dirX = move.Round(dirX, 1);
        dirX = Mathf.Clamp(dirX, -1, 1);

        move.UpdateMove(move.speed * dirX, Vector3.right);

        if(turn)
        turn.TryTurn(dirX);
    }    
}
