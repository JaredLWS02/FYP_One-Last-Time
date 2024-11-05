using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class PakYaActionInputs : MonoBehaviour
{
    public GameObject owner;
    public Pilot pilot;

    // Move ============================================================================

    Vector2 moveInput;

    void Update()
    {
        if(pilot.IsNone()) moveInput = Vector2.zero;

        EventM.OnTryMove(owner, moveInput);
        EventM.OnTryFlip(owner, moveInput.x);
    }

    void OnInputMove(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        moveInput = value.Get<Vector2>();
    }

    // Jump ============================================================================

    [Header("Input Buffers")]
    public float jumpBuffer=.2f;

    void OnInputJump(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        float jumpInput = value.Get<float>();

        if(jumpInput>0) //press
        {
            EventM.OnAddInputBuffer(owner, "Jump", jumpBuffer);
        }
        else //release
        {
            EventM.OnTryJumpCut(owner);
        }
    }

    // Dash ============================================================================

    public float dashBuffer=.2f;

    void OnInputDash()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(owner, "Dash", dashBuffer);
    }

    // Attack ============================================================================

    public float attackBuffer=.2f;

    void OnInputLightAttack()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(owner, "LightAttack", attackBuffer);
    }

    void OnInputHeavyAttack()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(owner, "HeavyAttack", attackBuffer);
    }

    // Parry ============================================================================
    
    public float parryBuffer=.2f;

    void OnInputParry()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(owner, "Parry", parryBuffer);
    }

    // Ability ============================================================================

    public float abilityBuffer=.2f;

    void OnInputAbility1()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(owner, "Ability1", abilityBuffer);
    }

    void OnInputAbility2()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(owner, "Ability2", abilityBuffer);
    }

    void OnInputAbility3()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(owner, "Ability3", abilityBuffer);
    }
    
    // Input Buffer ============================================================================

    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.InputBufferingEvent += OnInputBuffering;

        EventM.JumpedEvent += OnJumped;
        EventM.DashedEvent += OnDashed;
        EventM.DashCancelledEvent += OnDashCancelled;
        EventM.AttackedEvent += OnAttacked;
        EventM.AttackCancelledEvent += OnAttackCancelled;
        EventM.RaisedParryEvent += OnRaisedParry;
        EventM.ParryCancelledEvent += OnParryCancelled;
        EventM.CastingEvent += OnCasting;
        EventM.CastCancelledEvent += OnCastCancelled;
    }
    void OnDisable()
    {
        EventM.InputBufferingEvent -= OnInputBuffering;

        EventM.JumpedEvent -= OnJumped;
        EventM.DashedEvent -= OnDashed;
        EventM.DashCancelledEvent -= OnDashCancelled;
        EventM.AttackedEvent -= OnAttacked;
        EventM.AttackCancelledEvent -= OnAttackCancelled;
        EventM.RaisedParryEvent -= OnRaisedParry;
        EventM.ParryCancelledEvent -= OnParryCancelled;
        EventM.CastingEvent -= OnCasting;
        EventM.CastCancelledEvent -= OnCastCancelled;
    }
    
    // ============================================================================    

    void OnInputBuffering(GameObject who, string input_name)
    {
        if(who!=owner) return;

        switch(input_name)
        {
            case "Jump": EventM.OnTryJump(owner); break;

            case "Dash": EventM.OnTryDash(owner); break;

            case "LightAttack":
            {
                EventM.OnTryCombo(owner, "Light Combo");
                EventM.OnTryRiposteCombo(owner, "Riposte Combo");
            }
            break;

            case "HeavyAttack":
            {
                EventM.OnTryCombo(owner, "Heavy Combo");
                EventM.OnTryRiposteCombo(owner, "Riposte Combo");
            }
            break;

            case "Parry": EventM.OnTryRaiseParry(owner); break;

            case "Ability1": EventM.OnTryAbility(owner, "Heal"); break;

            case "Ability2": EventM.OnTryAbility(owner, "Heal"); break;

            case "Ability3": EventM.OnTryAbility(owner, "Heal"); break;
        }
    }

    // ============================================================================    

    void OnJumped(GameObject jumper)
    {
        if(jumper!=owner) return;
        
        EventM.OnRemoveInputBuffer(owner, "Jump");
    }
    
    void OnDashed(GameObject who)
    {
        if(who!=owner) return;
        
        EventM.OnRemoveInputBuffer(owner, "Dash");
    }
    
    void OnDashCancelled(GameObject who)
    {
        if(who!=owner) return;
        
        EventM.OnRemoveInputBuffer(owner, "Dash");
    }
    
    void OnAttacked(GameObject attacker, AttackSO attackSO)
    {
        if(attacker!=owner) return;

        EventM.OnRemoveInputBuffer(owner, "LightAttack");
        EventM.OnRemoveInputBuffer(owner, "HeavyAttack");
    }

    void OnAttackCancelled(GameObject attacker)
    {
        if(attacker!=owner) return;

        EventM.OnRemoveInputBuffer(owner, "LightAttack");
        EventM.OnRemoveInputBuffer(owner, "HeavyAttack");
    }

    void OnRaisedParry(GameObject defender)
    {
        if(defender!=owner) return;

        EventM.OnRemoveInputBuffer(owner, "Parry");
    }
    
    void OnParryCancelled(GameObject defender)
    {
        if(defender!=owner) return;

        EventM.OnRemoveInputBuffer(owner, "Parry");
    }

    void OnCasting(GameObject caster, AbilitySO abilitySO)
    {
        if(caster!=owner) return;

        EventM.OnRemoveInputBuffer(owner, "Ability1");
        EventM.OnRemoveInputBuffer(owner, "Ability2");
        EventM.OnRemoveInputBuffer(owner, "Ability3");
    }

    void OnCastCancelled(GameObject caster)
    {
        if(caster!=owner) return;

        EventM.OnRemoveInputBuffer(owner, "Ability1");
        EventM.OnRemoveInputBuffer(owner, "Ability2");
        EventM.OnRemoveInputBuffer(owner, "Ability3");
    }

}
