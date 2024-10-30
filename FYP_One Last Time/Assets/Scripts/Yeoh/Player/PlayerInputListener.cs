using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Pilot))]

public class PlayerInputListener : MonoBehaviour
{
    [HideInInspector]
    public Pilot pilot;

    void Awake()
    {
        pilot = GetComponent<Pilot>();
    }

    // Move ============================================================================

    Vector2 moveInput;

    void Update()
    {
        if(pilot.IsNone()) moveInput = Vector2.zero;

        EventM.OnTryMove(gameObject, moveInput);
        EventM.OnTryFlip(gameObject, moveInput.x);
    }

    void OnInputMove(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        moveInput = value.Get<Vector2>();
    }

    // Jump ============================================================================

    public float jumpBuffer=.2f;

    void OnInputJump(InputValue value)
    {
        if(!pilot.IsPlayer()) return;

        float jumpInput = value.Get<float>();

        if(jumpInput>0) //press
        {
            EventM.OnAddInputBuffer(gameObject, "Jump", jumpBuffer);
        }
        else //release
        {
            EventM.OnTryJumpCut(gameObject);
        }
    }

    // Dash ============================================================================

    public float dashBuffer=.2f;

    void OnInputDash()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(gameObject, "Dash", dashBuffer);
    }

    // Attack ============================================================================

    public float attackBuffer=.2f;

    void OnInputLightAttack()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(gameObject, "LightAttack", attackBuffer);
    }

    void OnInputHeavyAttack()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(gameObject, "HeavyAttack", attackBuffer);
    }

    // Parry ============================================================================
    
    public float parryBuffer=.2f;

    void OnInputParry()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(gameObject, "Parry", parryBuffer);
    }

    // Ability ============================================================================

    public float abilityBuffer=.2f;

    void OnInputAbility1()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(gameObject, "Ability1", abilityBuffer);
    }

    void OnInputAbility2()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(gameObject, "Ability2", abilityBuffer);
    }

    void OnInputAbility3()
    {
        if(!pilot.IsPlayer()) return;

        EventM.OnAddInputBuffer(gameObject, "Ability3", abilityBuffer);
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
        if(who!=gameObject) return;

        switch(input_name)
        {
            case "Jump": EventM.OnTryJump(gameObject); break;

            case "Dash": EventM.OnTryDash(gameObject); break;

            case "LightAttack":
            {
                EventM.OnTryCombo(gameObject, "Light Combo");
                EventM.OnTryRiposteCombo(gameObject, "Riposte Combo");
            }
            break;

            case "HeavyAttack":
            {
                EventM.OnTryCombo(gameObject, "Heavy Combo");
                EventM.OnTryRiposteCombo(gameObject, "Riposte Combo");
            }
            break;

            case "Parry": EventM.OnTryRaiseParry(gameObject); break;

            case "Ability1": EventM.OnTryAbility(gameObject, "Heal"); break;

            case "Ability2": EventM.OnTryAbility(gameObject, "Heal"); break;

            case "Ability3": EventM.OnTryAbility(gameObject, "Heal"); break;
        }
    }

    // ============================================================================    

    void OnJumped(GameObject jumper)
    {
        if(jumper!=gameObject) return;
        
        EventM.OnRemoveInputBuffer(gameObject, "Jump");
    }
    
    void OnDashed(GameObject who)
    {
        if(who!=gameObject) return;
        
        EventM.OnRemoveInputBuffer(gameObject, "Dash");
    }
    
    void OnDashCancelled(GameObject who)
    {
        if(who!=gameObject) return;
        
        EventM.OnRemoveInputBuffer(gameObject, "Dash");
    }
    
    void OnAttacked(GameObject attacker, AttackSO attackSO)
    {
        if(attacker!=gameObject) return;

        EventM.OnRemoveInputBuffer(gameObject, "LightAttack");
        EventM.OnRemoveInputBuffer(gameObject, "HeavyAttack");
    }

    void OnAttackCancelled(GameObject attacker)
    {
        if(attacker!=gameObject) return;

        EventM.OnRemoveInputBuffer(gameObject, "LightAttack");
        EventM.OnRemoveInputBuffer(gameObject, "HeavyAttack");
    }

    void OnRaisedParry(GameObject defender)
    {
        if(defender!=gameObject) return;

        EventM.OnRemoveInputBuffer(gameObject, "Parry");
    }
    
    void OnParryCancelled(GameObject defender)
    {
        if(defender!=gameObject) return;

        EventM.OnRemoveInputBuffer(gameObject, "Parry");
    }

    void OnCasting(GameObject caster, AbilitySO abilitySO)
    {
        if(caster!=gameObject) return;

        EventM.OnRemoveInputBuffer(gameObject, "Ability1");
        EventM.OnRemoveInputBuffer(gameObject, "Ability2");
        EventM.OnRemoveInputBuffer(gameObject, "Ability3");
    }

    void OnCastCancelled(GameObject caster)
    {
        if(caster!=gameObject) return;

        EventM.OnRemoveInputBuffer(gameObject, "Ability1");
        EventM.OnRemoveInputBuffer(gameObject, "Ability2");
        EventM.OnRemoveInputBuffer(gameObject, "Ability3");
    }

}
