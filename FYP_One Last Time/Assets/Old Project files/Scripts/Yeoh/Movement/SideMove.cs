using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ForceVehicle2D))]

public class SideMove : MonoBehaviour
{
    ForceVehicle2D vehicle;

    void Awake()
    {
        vehicle = GetComponent<ForceVehicle2D>();
    }

    // Event Manager ============================================================================

    void OnEnable()
    {
        EventManager.Current.MoveXEvent += OnMoveX;
    }
    void OnDisable()
    {
        EventManager.Current.MoveXEvent -= OnMoveX;
    }    

    // Events ============================================================================

    public bool canMove=true;

    void OnMoveX(GameObject mover, float input_x)
    {
        if(mover!=gameObject) return;
        if(!canMove) return;

        dirX = input_x;

        isMoving=true;
    }

    // ============================================================================

    bool isMoving;
    public float dirX;

    void FixedUpdate()
    {
        if(!isMoving) dirX=0;

        Move();

        isMoving=false;
    }

    void Move()
    {
        dirX = vehicle.Round(dirX, 1);
        dirX = Mathf.Clamp(dirX, -1, 1);

        vehicle.Move(vehicle.maxSpeed * dirX, Vector2.right);

        TryFlip();
    }

    // ============================================================================

    [Header("Flip")]
    public bool faceR=true;
    public bool reverse;

    void TryFlip()
    {
        if(reverse)
        {
            if((dirX>0 && faceR) || (dirX<0 && !faceR))
            {
                Flip();
            }
        }
        else
        {
            if((dirX<0 && faceR) || (dirX>0 && !faceR))
            {
                Flip();
            }
        }
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        faceR=!faceR;
    }
}
