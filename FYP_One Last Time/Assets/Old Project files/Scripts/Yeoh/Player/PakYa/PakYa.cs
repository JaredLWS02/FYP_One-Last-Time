using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Pilot))]

[RequireComponent(typeof(Jump2D))]
[RequireComponent(typeof(AbilityCaster))]

public class PakYa : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;

    Jump2D jump;
    AbilityCaster caster;

    void Awake()
    {
        pilot = GetComponent<Pilot>();

        jump = GetComponent<Jump2D>();
        caster = GetComponent<AbilityCaster>();
    }

    void Start()
    {
        EventManager.Current.OnSpawn(gameObject);
    }

    // Actions ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowDash;
    public bool AllowCast;

    // Event Manager ============================================================================

    void OnEnable()
    {
        EventManager.Current.TryMoveXEvent += OnTryMoveX;
        EventManager.Current.TryMoveYEvent += OnTryMoveY;
        EventManager.Current.TryJumpEvent += OnTryJump;
        EventManager.Current.TryStartCastEvent += OnTryStartCast;

        PlayerManager.Current.Register(gameObject);
    }
    void OnDisable()
    {
        EventManager.Current.TryMoveXEvent -= OnTryMoveX;
        EventManager.Current.TryMoveYEvent -= OnTryMoveY;
        EventManager.Current.TryJumpEvent -= OnTryJump;
        EventManager.Current.TryStartCastEvent -= OnTryStartCast;

        PlayerManager.Current.Unregister(gameObject);
    }

    // Move ============================================================================

    Vector2 moveInput;

    void OnInputMove(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        UpdateMoveInput();
    } 

    void UpdateMoveInput()
    {
        if(!pilot.IsPlayer()) moveInput=Vector2.zero;

        if(moveInput==Vector2.zero) return;

        EventManager.Current.OnTryMoveX(gameObject, moveInput.x);
        EventManager.Current.OnTryMoveY(gameObject, moveInput.y);
    }

    void OnTryMoveX(GameObject mover, float input_x)
    {
        if(mover!=gameObject) return;

        if(!AllowMoveX) return;

        EventManager.Current.OnMoveX(gameObject, input_x);
    }

    void OnTryMoveY(GameObject mover, float input_y)
    {
        if(mover!=gameObject) return;

        if(!AllowMoveY) return;

        EventManager.Current.OnMoveY(gameObject, input_y);
    }

    // Jump ============================================================================
    
    void OnInputJump(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        float input = value.Get<float>();

        EventManager.Current.OnTryJump(gameObject, input);
    }

    void OnTryJump(GameObject jumper, float input)
    {
        if(jumper!=gameObject) return;

        if(!AllowJump) return;

        EventManager.Current.OnJump(gameObject, input);
    }

    // Cast ============================================================================

    void OnInputHeal()
    {
        if(!pilot.IsPlayer()) return;

        EventManager.Current.OnTryStartCast(gameObject, "Heal");
    }

    void OnTryStartCast(GameObject caster, string ability_name)
    {
        if(caster!=gameObject) return;

        if(!AllowCast) return;

        EventManager.Current.OnStartCast(gameObject, ability_name);
    }

    // ============================================================================

    public bool IsGrounded()
    {
        return jump.IsGrounded();
    }    

    public bool IsDashing()
    {
        return false;
    }
    
    public bool IsCasting()
    {
        return caster.isCasting;
    }    
    
    // ============================================================================





    //  Old ============================================================================

    void OnInputSwitch()
    {
        if(!pilot.IsPlayer()) return;

        EventManager.Current.OnTrySwitch(gameObject);
    }
}
