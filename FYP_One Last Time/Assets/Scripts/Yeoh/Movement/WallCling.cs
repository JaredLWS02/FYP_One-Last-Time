using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WallCling : MonoBehaviour
{
    public GameObject owner;
    public BaseRaycast ray;
    public GroundCheck ground;
    public Rigidbody rb;

    // ============================================================================

    InputManager InputM;
    EventManager EventM;

    void OnEnable()
    {
        InputM = InputManager.Current;
        EventM = EventManager.Current;
    }

    // ============================================================================

    bool IsMovingToWall()
    {
        if(!ray.IsHitting()) return false;

        Vector2 move_input = InputM.moveAxis;
        if(move_input == Vector2.zero) return false;

        // y input for the z axis
        Vector3 move_dir = new(move_input.x, 0, move_input.y);
        move_dir = move_dir.normalized;

        // move dir based on camera orientation
        Vector3 cam_move_dir = Camera.main.transform.TransformDirection(move_dir).normalized;

        Vector3 dir_to_wall = (ray.rayHit.point - owner.transform.position).normalized;

        float dot = Vector3.Dot(cam_move_dir, dir_to_wall);
        return dot>0;
    }

    // ============================================================================

    public bool allowWallCling=true;

    void FixedUpdate()
    {
        CheckIsClinging();
        CheckCling();
        SetAnimator();
    }

    // ============================================================================

    public float minYVelocityToCling=10;

    public bool isClinging=false;

    void CheckIsClinging()
    {
        bool clinging = !ground.IsGrounded() && IsMovingToWall() && rb.velocity.y<=minYVelocityToCling;

        if(clinging)
        {
            if(!isClinging)
            {
                isClinging=true;
                ToggleCling(true);
            }
        }
        else
        {
            if(isClinging)
            {
                isClinging=false;
                ToggleCling(false);
            }
        }
    }

    public bool instantBrake=true;

    void ToggleCling(bool toggle)
    {
        if(toggle && instantBrake)
        rb.velocity = new(rb.velocity.x, 0, rb.velocity.z);

        wallClingEvents.OnToggleCling?.Invoke(toggle);
    }

    // ============================================================================

    public float wallSlideSpeed = -2.5f;

    void CheckCling()
    {
        if(!allowWallCling) return;
        if(!isClinging) return;

        EventM.OnCancelDash(owner);

        if(rb.velocity.y < wallSlideSpeed)
        rb.velocity = new(rb.velocity.x, wallSlideSpeed, rb.velocity.z);
    }

    // ============================================================================

    [Header("Animator")]
    public Animator anim;
    public string clingingBoolName = "IsClinging";

    void SetAnimator()
    {
        anim?.SetBool(clingingBoolName, isClinging);
    }

    // ============================================================================

    [System.Serializable]
    public struct WallClingEvents
    {
        public UnityEvent<bool> OnToggleCling;
    }
    [Space]
    public WallClingEvents wallClingEvents;
}
