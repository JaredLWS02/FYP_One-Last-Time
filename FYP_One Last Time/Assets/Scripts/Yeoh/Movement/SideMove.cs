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

    void FixedUpdate()
    {
        UpdateMove();
    }

    // ============================================================================

    float dirX;

    public void OnMove(float input_x)
    {
        dirX = input_x;
    }

    void UpdateMove()
    {
        dirX = move.Round(dirX, 1);
        dirX = Mathf.Clamp(dirX, -1, 1);

        move.UpdateMove(move.speed * dirX, Vector3.right);

        EventManager.Current.OnTryFaceX(gameObject, dirX);
    }    
}
