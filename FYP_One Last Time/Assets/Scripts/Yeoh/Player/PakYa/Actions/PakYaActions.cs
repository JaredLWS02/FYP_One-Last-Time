using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PakYaActions : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;
        
        EventM.TryMoveEvent += OnTryMove;
        EventM.TryFlipEvent += OnTryFlip;
        EventM.TryJumpEvent += OnTryJump;
        EventM.TryJumpCutEvent += OnTryJumpCut;
        EventM.TryDashEvent += OnTryDash;
        EventM.TryComboEvent += OnTryCombo;
        EventM.TryRaiseParryEvent += OnTryRaiseParry;
        EventM.TryRiposteComboEvent += OnTryRiposteCombo;
        EventM.TryHurtEvent += OnTryHurt;
        EventM.TryStunEvent += OnTryStun;
    }
    void OnDisable()
    {
        EventM.TryMoveEvent -= OnTryMove;
        EventM.TryFlipEvent -= OnTryFlip;
        EventM.TryJumpEvent -= OnTryJump;
        EventM.TryJumpCutEvent -= OnTryJumpCut;
        EventM.TryDashEvent -= OnTryDash;
        EventM.TryComboEvent -= OnTryCombo;
        EventM.TryRaiseParryEvent -= OnTryRaiseParry;
        EventM.TryRiposteComboEvent -= OnTryRiposteCombo;
        EventM.TryHurtEvent -= OnTryHurt;
        EventM.TryStunEvent -= OnTryStun;
    }

    // ============================================================================

    [Header("Hold Toggles")]
    public bool AllowMoveX;
    public bool AllowMoveY;
    public bool AllowFlip;
    public bool AllowWallCling;
    
    [Header("Toggles")]
    public bool AllowJump;
    public bool AllowDash;
    public bool AllowAttack;
    public bool AllowParry;
    public bool AllowHurt;
    public bool AllowStun;

    // ============================================================================

    void Update()
    {
        cling.allowWallCling = AllowWallCling;
    }

    // ============================================================================

    void OnTryMove(GameObject who, Vector2 input)
    {
        if(who!=owner) return;

        if(!AllowMoveX) input.x=0;
        if(!AllowMoveY) input.y=0;

        EventM.OnMove(owner, input);
    }

    void OnTryFlip(GameObject who, float input_x)
    {
        if(who!=owner) return;

        if(!AllowFlip) return;

        EventM.OnFlip(owner, input_x);
    }
    
    // ============================================================================

    void OnTryJump(GameObject who)
    {
        if(who!=owner) return;

        if(!AllowJump) return;

        EventM.OnJump(owner);
    }

    void OnTryJumpCut(GameObject who)
    {
        if(who!=owner) return;

        if(!AllowJump) return;

        EventM.OnJumpCut(owner);
    }

    // ============================================================================    
    
    void OnTryDash(GameObject who)
    {
        if(who!=owner) return;

        if(!AllowDash) return;

        EventM.OnDash(owner);
    }
    
    // ============================================================================    

    void OnTryCombo(GameObject who, string combo_name)
    {
        if(who!=owner) return;

        if(!AllowAttack) return;

        if(IsRiposteActive()) return;

        EventM.OnCombo(owner, combo_name);
    }

    void OnTryRiposteCombo(GameObject who, string combo_name)
    {
        if(who!=owner) return;

        if(!AllowAttack) return;

        if(!IsRiposteActive()) return;

        EventM.OnCombo(owner, combo_name);
    }  

    // ============================================================================
    
    void OnTryRaiseParry(GameObject who)
    {
        if(who!=owner) return;

        if(!AllowParry) return;

        EventM.OnRaiseParry(owner);
    }

    // ============================================================================
    
    void OnTryHurt(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        if(!AllowHurt) return;

        EventM.OnTryParry(owner, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    void OnTryStun(GameObject victim, GameObject attacker, HurtboxSO hurtbox, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        if(!AllowStun) return;

        EventM.OnStun(owner, attacker, hurtbox, contactPoint);
    }

    // ============================================================================

    [Header("Check Action States")]

    public GroundCheck ground;
    public bool IsGrounded() => ground.IsGrounded();

    public WallCling cling;
    public bool IsClinging() => cling.IsClinging();

    public DashScript dash;
    public bool IsDashing() => dash.IsPerforming();
    
    public AttackScript attack;
    public bool IsWindingUpAttack() => attack.IsWindingUp();
    public bool IsReleasingAttack() => attack.HasReleased();
    
    public TryParryScript tryParry;
    public bool IsTryingToParry() => tryParry.IsPerforming();

    public OnParryScript onParry;
    public bool IsParrying() => onParry.IsPerforming();

    public RiposteScript riposte;
    public bool IsRiposteActive() => riposte.IsRiposteActive();
    
    public StunScript stun;
    public bool IsStunned() => stun.IsPerforming();
}
