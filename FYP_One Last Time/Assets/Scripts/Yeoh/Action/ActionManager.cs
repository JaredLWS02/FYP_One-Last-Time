using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SideMove))]
[RequireComponent(typeof(SideTurn))]
[RequireComponent(typeof(JumpScript))]
[RequireComponent(typeof(GroundCheck))]
[RequireComponent(typeof(AttackScript))]
[RequireComponent(typeof(AbilityCaster))]

public class ActionManager : MonoBehaviour
{
    SideMove move;
    SideTurn turn;
    JumpScript jump;
    AgentAutoJump autoJump;
    GroundCheck ground;
    AttackScript attack;
    AbilityCaster caster;

    void Awake()
    {
        move = GetComponent<SideMove>();
        turn = GetComponent<SideTurn>();
        jump = GetComponent<JumpScript>();
        autoJump = GetComponent<AgentAutoJump>();
        ground = GetComponent<GroundCheck>();
        attack = GetComponent<AttackScript>();
        caster = GetComponent<AbilityCaster>();
    }

    // ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowAutoJump;
    public bool AllowDash;
    public bool AllowAttack;
    public bool AllowCast;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryMoveEvent += OnTryMove;
        EventM.TryFaceXEvent += OnTryFaceX;
        EventM.TryJumpEvent += OnTryJump;
        EventM.TryAutoJumpEvent += OnTryAutoJump;
        EventM.TryAttackEvent += OnTryAttack;
        EventM.TryStartCastEvent += OnTryStartCast;
    }
    void OnDisable()
    {
        EventM.TryMoveEvent -= OnTryMove;
        EventM.TryFaceXEvent -= OnTryFaceX;
        EventM.TryJumpEvent -= OnTryJump;
        EventM.TryAutoJumpEvent -= OnTryAutoJump;
        EventM.TryAttackEvent -= OnTryAttack;
        EventM.TryStartCastEvent -= OnTryStartCast;
    }

    // ============================================================================

    void OnTryMove(GameObject who, Vector2 input)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) input.x=0;
        if(!AllowMoveY) input.y=0;

        EventM.OnMove(gameObject, input);

        move.Move(input.x);
        //climb.Move(gameObject, input.y);
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

    void OnTryAutoJump(GameObject who, Vector3 jump_dir)
    {
        if(who!=gameObject) return;

        if(!autoJump) return;

        if(!AllowAutoJump) return;

        autoJump.StartJump();

        float dot_x = Vector3.Dot(jump_dir, Vector3.right);

        turn.UpdateFlip(dot_x);
    }

    // ============================================================================
    
    [Header("Attacks")]
    public List<AttackCombo> attackCombos = new();

    void OnTryAttack(GameObject who, string attackName)
    {
        if(who!=gameObject) return;

        if(!AllowAttack) return;

        foreach(var combo in attackCombos)
        {
            if(combo.comboName == attackName)
            {
                combo.DoBuffer();
                break;
            }
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

    public bool IsAutoJumping()
    {
        if(!autoJump) return false;
        return autoJump.isJumping;
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
