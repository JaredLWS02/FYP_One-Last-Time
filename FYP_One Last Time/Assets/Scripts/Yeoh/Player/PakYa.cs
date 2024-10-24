using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SideMove))]
[RequireComponent(typeof(SideTurn))]
[RequireComponent(typeof(JumpScript))]
[RequireComponent(typeof(GroundCheck))]

[RequireComponent(typeof(AttackScript))]
[RequireComponent(typeof(AbilityCaster))]

public class PakYa : MonoBehaviour
{
    SideMove move;
    SideTurn turn;
    JumpScript jump;
    GroundCheck ground;

    AttackScript attack;
    AbilityCaster caster;

    void Awake()
    {
        move = GetComponent<SideMove>();
        turn = GetComponent<SideTurn>();
        jump = GetComponent<JumpScript>();
        ground = GetComponent<GroundCheck>();

        attack = GetComponent<AttackScript>();
        caster = GetComponent<AbilityCaster>();
    }

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryMoveEvent += OnTryMove;
        EventM.TryFaceXEvent += OnTryFaceX;
        EventM.TryJumpEvent += OnTryJump;

        EventM.TryAttackEvent += OnTryAttack;
        EventM.TryStartCastEvent += OnTryStartCast;

        PlayerManager.Current.Register(gameObject);
    }
    void OnDisable()
    {
        EventM.TryMoveEvent -= OnTryMove;
        EventM.TryFaceXEvent -= OnTryFaceX;
        EventM.TryJumpEvent -= OnTryJump;

        EventM.TryAttackEvent -= OnTryAttack;
        EventM.TryStartCastEvent -= OnTryStartCast;

        PlayerManager.Current.Unregister(gameObject);
    }

    // ============================================================================

    void Start()
    {
        EventM.OnSpawn(gameObject);
    }

    // ============================================================================
    
    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowDash;
    public bool AllowAttack;
    public bool AllowCast;

    // ============================================================================

    void OnTryMove(GameObject who, Vector2 input)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) input.x=0;
        if(!AllowMoveY) input.y=0;

        move.UpdateMove(input.x);
        //climb.OnMoveY(gameObject, input.y);

        EventM.OnMove(gameObject, input);
        EventM.OnTryFaceX(gameObject, input.x);
    }

    void OnTryFaceX(GameObject who, float input_x)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) input_x=0;

        EventM.OnFaceX(gameObject, input_x);

        turn.UpdateFlip(input_x);
    }
    
    // ============================================================================

    void OnTryJump(GameObject who, float input)
    {
        if(who!=gameObject) return;

        if(!AllowJump) return;

        jump.OnJump(input);        
    }

    // ============================================================================
    
    [Header("Attacks")]
    public AttackCombo lightCombo;
    public AttackCombo heavyCombo;

    void OnTryAttack(GameObject who, string type)
    {
        if(who!=gameObject) return;

        if(!AllowAttack) return;

        switch(type)
        {
            case "Light": lightCombo.DoBuffer(); break;
            case "Heavy": heavyCombo.DoBuffer(); break;
        }
    }    

    // ============================================================================

    void OnTryStartCast(GameObject who, string ability_name)
    {
        if(who!=gameObject) return;

        if(!AllowCast) return;
        
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
    
    public bool IsAttacking()
    {
        return attack.isAttacking;
    }
    
    public bool IsCasting()
    {
        return caster.isCasting;
    }    
    
}
