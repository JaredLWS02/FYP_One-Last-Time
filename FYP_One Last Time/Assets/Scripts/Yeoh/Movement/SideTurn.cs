using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurnScript))]

public class SideTurn : MonoBehaviour
{
    TurnScript turn;

    void Awake()
    {
        turn = GetComponent<TurnScript>();
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

        if(sprite)
        sprite.flipX = !faceR;
    }

    [Header("If Using Billboard")]
    public SpriteRenderer sprite;
}
