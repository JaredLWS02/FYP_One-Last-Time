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

    float dirX;

    public void Move(float input_x)
    {
        dirX = input_x;
    }
    
    // ============================================================================

    void FixedUpdate()
    {
        dirX = move.Round(dirX, 1);

        move.UpdateMoveMult(dirX, Vector3.right);
    }
}
