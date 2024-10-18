using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Turn2D))]

public class SideTurn2D : MonoBehaviour
{
    Turn2D turn;

    void Awake()
    {
        turn = GetComponent<Turn2D>();
    }

    void FixedUpdate()
    {
        turn.UpdateTurn(faceR ? Vector3.right : Vector3.left);
    }

    // ============================================================================

    public bool faceR=true;
    public bool reverse;

    public void TryTurn(float dir_x)
    {
        if(reverse)
        {
            if((dir_x>0 && faceR) || (dir_x<0 && !faceR))
            {
                Turn();
            }
        }
        else
        {
            if((dir_x<0 && faceR) || (dir_x>0 && !faceR))
            {
                Turn();
            }
        }
    }

    void Turn()
    {
        faceR=!faceR;
        //transform.Rotate(0, 180, 0);
    }
}
