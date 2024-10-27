using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowDash;
    public bool AllowAttack;
    public bool AllowParry;
    public bool AllowCast;
    public bool AllowStun;

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
        EventM.TryParryEvent += OnTryParry;
        EventM.TryRiposteAttackEvent += OnTryRiposteAttack;
        EventM.TryAbilityEvent += OnTryCast;
        EventM.TryStunEvent += OnTryStun;
    }
    void OnDisable()
    {
        EventM.TryMoveEvent -= OnTryMove;
        EventM.TryFaceXEvent -= OnTryFaceX;
        EventM.TryJumpEvent -= OnTryJump;
        EventM.TryAutoJumpEvent -= OnTryAutoJump;
        EventM.TryAttackEvent -= OnTryAttack;
        EventM.TryParryEvent -= OnTryParry;
        EventM.TryRiposteAttackEvent -= OnTryRiposteAttack;
        EventM.TryAbilityEvent -= OnTryCast;
        EventM.TryStunEvent -= OnTryStun;
    }

    // ============================================================================

    [Header("Movement")]
    public SideMove move;

    void OnTryMove(GameObject who, Vector2 input)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) input.x=0;
        if(!AllowMoveY) input.y=0;

        if(move)
        move.Move(input.x);

        //if(climb)
        //climb.Move(gameObject, input.y);

        EventM.OnMove(gameObject, input);
    }

    public SideTurn turn;

    void OnTryFaceX(GameObject who, float input_x)
    {
        if(who!=gameObject) return;

        if(!AllowMoveX) input_x=0;

        if(!turn) return;

        turn.UpdateFlip(input_x);

        EventM.OnFaceX(gameObject, input_x);
    }
    
    // ============================================================================

    [Header("Jump")]
    public JumpScript jump;
    public GroundCheck ground;

    void OnTryJump(GameObject who, float input)
    {
        if(who!=gameObject) return;

        if(!AllowJump) return;

        if(!jump) return;
        
        jump.OnJump(input);        
    }

    public AgentAutoJump autoJump;

    void OnTryAutoJump(GameObject who, Vector3 jump_dir)
    {
        if(who!=gameObject) return;

        if(!AllowJump) return;

        if(!autoJump) return;

        autoJump.StartJump();

        if(!turn) return;
        
        float dot_x = Vector3.Dot(jump_dir, Vector3.right);

        turn.UpdateFlip(dot_x);
    }

    // ============================================================================
    
    [Header("Attack")]
    public AttackScript attack;
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
    
    [Header("Parry")]
    public ParryScript parry;

    void OnTryParry(GameObject who)
    {
        if(who!=gameObject) return;

        if(!AllowParry) return;

        if(!parry) return;

        parry.DoBuffer();
    }

    void OnTryRiposteAttack(GameObject who, string attackName)
    {
        if(who!=gameObject) return;

        if(!AllowAttack) return;

        if(!IsRiposteActive()) return;

        OnTryAttack(who, attackName);
    }  

    // ============================================================================

    [Header("Ability")]
    public AbilityCaster caster;
    public HealAbility heal;

    void OnTryCast(GameObject who, string ability_name)
    {
        if(who!=gameObject) return;

        if(!AllowCast) return;

        if(!caster) return;
        
        if(ability_name=="Heal")
        {
            if(heal) heal.DoBuffer();
        }
    }

    // ============================================================================

    [Header("Stun")]
    public StunScript stun;

    void OnTryStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=gameObject) return;

        if(!AllowStun) return;

        if(!stun) return;

        stun.Stun(gameObject, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    public bool IsGrounded()
    {
        if(!ground) return false;
        return ground.IsGrounded();
    }

    public bool IsAutoJumping()
    {
        if(!autoJump) return false;
        return autoJump.isJumping;
    }

    public bool IsDashing()
    {
        //if(!dash) return false;
        return false;
    }
    
    public bool IsAttackWindingUp()
    {
        if(!attack) return false;
        return attack.isWindingUp;
    }
    public bool IsAttacking()
    {
        if(!attack) return false;
        return attack.isAttacking;
    }
    
    public bool IsParryRaised()
    {
        if(!parry) return false;
        return parry.isParryRaised;
    }
    public bool IsParryLowering()
    {
        if(!parry) return false;
        return parry.isParryLowering;
    }
    public bool IsRiposteActive()
    {
        if(!parry) return false;
        return parry.IsRiposteActive();
    }
    
    public bool IsCasting()
    {
        if(!caster) return false;
        return caster.isCasting;
    }
    public bool IsCast()
    {
        if(!caster) return false;
        return caster.isCast;
    }

    public bool IsStunned()
    {
        if(!stun) return false;
        return stun.isStunned;
    }
    
}
