using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveScript))]

public class SideMove : MonoBehaviour
{
    MoveScript move;

    void Awake()
    {
        move = GetComponent<MoveScript>();
    }
    
    // ============================================================================

    public Vector3 sideAxis = Vector3.right;

    void FixedUpdate()
    {
        dirX = move.Round(dirX, 1);

        move.UpdateMoveMult(dirX, sideAxis);
    }

    // ============================================================================

    float dirX;

    public void UpdateMove(float input_x)
    {
        dirX = input_x;
    }
}
