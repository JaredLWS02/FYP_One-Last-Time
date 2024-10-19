using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Pilot))]

[RequireComponent(typeof(SideMove))]
[RequireComponent(typeof(SideTurn))]
[RequireComponent(typeof(JumpScript))]
[RequireComponent(typeof(GroundCheck))]
[RequireComponent(typeof(AbilityCaster))]

public class PakYa : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;

    SideMove move;
    SideTurn turn;
    JumpScript jump;
    GroundCheck ground;
    AbilityCaster caster;

    void Awake()
    {
        pilot = GetComponent<Pilot>();

        move = GetComponent<SideMove>();
        turn = GetComponent<SideTurn>();
        jump = GetComponent<JumpScript>();
        ground = GetComponent<GroundCheck>();
        caster = GetComponent<AbilityCaster>();
    }

    void Start()
    {
        EventManager.Current.OnSpawn(gameObject);
    }

    // ============================================================================

    void OnEnable()
    {
        EventManager.Current.TryMoveXEvent += OnTryMoveX;
        EventManager.Current.TryFaceXEvent += OnTryFaceX;
        EventManager.Current.TryMoveYEvent += OnTryMoveY;
        EventManager.Current.TryJumpEvent += OnTryJump;
        EventManager.Current.TryStartCastEvent += OnTryStartCast;

        PlayerManager.Current.Register(gameObject);
    }
    void OnDisable()
    {
        EventManager.Current.TryMoveXEvent -= OnTryMoveX;
        EventManager.Current.TryFaceXEvent -= OnTryFaceX;
        EventManager.Current.TryMoveYEvent -= OnTryMoveY;
        EventManager.Current.TryJumpEvent -= OnTryJump;
        EventManager.Current.TryStartCastEvent -= OnTryStartCast;

        PlayerManager.Current.Unregister(gameObject);
    }

    // ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowDash;
    public bool AllowCast;

    // ============================================================================

    void OnInputMove(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        Vector2 moveInput = value.Get<Vector2>();

        EventManager.Current.OnTryMoveX(gameObject, moveInput.x);
        EventManager.Current.OnTryFaceX(gameObject, moveInput.x);
        EventManager.Current.OnTryMoveY(gameObject, moveInput.y);
    }

    void OnTryMoveX(GameObject who, float input_x)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) input_x=0;

        EventManager.Current.OnMoveX(gameObject, input_x);

        move.OnMove(input_x);
    }

    void OnTryFaceX(GameObject who, float input_x)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) return;

        EventManager.Current.OnFaceX(gameObject, input_x);

        turn.TryFlip(input_x);
    }

    void OnTryMoveY(GameObject who, float input_y)
    {
        if(who!=gameObject) return;

        if(!AllowMoveY) input_y=0;

        EventManager.Current.OnMoveY(gameObject, input_y); // send to one way platform

        //climb.OnMoveY(gameObject, input_y);
    }

    // ============================================================================
    
    void OnInputJump(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        float input = value.Get<float>();

        EventManager.Current.OnTryJump(gameObject, input);
    }

    void OnTryJump(GameObject who, float input)
    {
        if(who!=gameObject) return;

        if(!AllowJump) return;

        EventManager.Current.OnJump(gameObject, input);

        jump.OnJump(input);
    }

    // ============================================================================

    void OnInputHeal()
    {
        if(!pilot.IsPlayer()) return;

        EventManager.Current.OnTryStartCast(gameObject, "Heal");
    }

    void OnTryStartCast(GameObject who, string ability_name)
    {
        if(who!=gameObject) return;

        if(!AllowCast) return;

        EventManager.Current.OnStartCast(gameObject, ability_name);

        caster.StartCast(ability_name);
    }

    // ============================================================================

    public bool IsGrounded()
    {
        return ground.IsGrounded();
    }    

    public bool IsDashing()
    {
        return false;
    }
    
    public bool IsCasting()
    {
        return caster.isCasting;
    }    
    
}
