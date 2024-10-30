using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorActions : MonoBehaviour
{
    public GameObject owner;
    public Animator anim;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.JumpedEvent += OnJumped;
        EventM.AutoJumpedEvent += OnAutoJumped;
    }
    void OnDisable()
    {
        EventM.JumpedEvent -= OnJumped;
        EventM.AutoJumpedEvent -= OnAutoJumped;
    }

    // ============================================================================

    [Header("Grounded")]
    public GroundCheck ground;
    public string groundedBoolName = "IsGrounded";

    void FixedUpdate()
    {
        if(ground)
        anim.SetBool(groundedBoolName, ground.IsGrounded());
    }

    // ============================================================================

    [Header("Jump")]
    public AnimPreset jumpAnim;

    void OnJumped(GameObject jumper)
    {
        if(jumper!=owner) return;

        jumpAnim.Play(owner);
    }

    void OnAutoJumped(GameObject jumper, Vector3 jump_dir)
    {
        if(jumper!=owner) return;

        jumpAnim.Play(owner);
    }
}
