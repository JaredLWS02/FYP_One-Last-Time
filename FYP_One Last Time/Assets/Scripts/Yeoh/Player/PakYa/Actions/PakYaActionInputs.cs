using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PakYaActionInputs : MonoBehaviour
{
    public GameObject owner;
    public Pilot pilot;

    [Header("Input Buffers")]
    public InputBuffer inputBuffer;
    public float jumpBuffer=.2f;
    public float dashBuffer=.2f;
    public float attackBuffer=.2f;
    public float parryBuffer=.2f;

    // ============================================================================

    void Update()
    {
        CheckMove();

        if(!pilot.IsPlayer()) return;

        CheckJump();
        CheckDash();
        CheckLightAttack();
        CheckHeavyAttack();
        CheckParry();
    }

    void CheckMove()
    {
        Vector2 moveAxis = pilot.IsNone() ? Vector2.zero : InputM.moveAxis;

        EventM.OnTryMove(owner, moveAxis);
        EventM.OnTryFlip(owner, moveAxis.x);
    }

    void CheckJump()
    {
        if(InputM.jumpKeyDown) //press
        {
            inputBuffer.AddBuffer("Jump", jumpBuffer);
        }
        else if(InputM.jumpKeyUp) //release
        {
            EventM.OnTryJumpCut(owner);
        }
    }

    void CheckDash()
    {
        if(InputM.dashKeyDown)
        inputBuffer.AddBuffer("Dash", dashBuffer);
    }

    void CheckLightAttack()
    {
        if(InputM.lightAttackKeyDown)
        inputBuffer.AddBuffer("LightAttack", attackBuffer);
    }

    void CheckHeavyAttack()
    {
        if(InputM.heavyAttackKeyDown)
        inputBuffer.AddBuffer("HeavyAttack", attackBuffer);
    }

    void CheckParry()
    {
        if(InputM.parryKeyDown)
        inputBuffer.AddBuffer("Parry", parryBuffer);
    }
    
    // ============================================================================

    InputManager InputM;
    EventManager EventM;

    void OnEnable()
    {
        InputM = InputManager.Current;
        EventM = EventManager.Current;

        inputBuffer.InputBufferingEvent += OnInputBuffering;

        EventM.JumpedEvent += OnJumped;
        EventM.DashedEvent += OnDashed;
        EventM.DashCancelledEvent += OnDashCancelled;
        EventM.AttackedEvent += OnAttacked;
        EventM.AttackCancelledEvent += OnAttackCancelled;
        EventM.RaisedParryEvent += OnRaisedParry;
        EventM.ParryCancelledEvent += OnParryCancelled;
    }
    void OnDisable()
    {
        inputBuffer.InputBufferingEvent -= OnInputBuffering;

        EventM.JumpedEvent -= OnJumped;
        EventM.DashedEvent -= OnDashed;
        EventM.DashCancelledEvent -= OnDashCancelled;
        EventM.AttackedEvent -= OnAttacked;
        EventM.AttackCancelledEvent -= OnAttackCancelled;
        EventM.RaisedParryEvent -= OnRaisedParry;
        EventM.ParryCancelledEvent -= OnParryCancelled;
    }
    
    // ============================================================================    

    void OnInputBuffering(string input_name)
    {
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
        }
    }

    // ============================================================================    

    void OnJumped(GameObject jumper)
    {
        if(jumper!=owner) return;
        
        inputBuffer.RemoveBuffer("Jump");
    }
    
    void OnDashed(GameObject who)
    {
        if(who!=owner) return;
        
        inputBuffer.RemoveBuffer("Dash");
    }
    
    void OnDashCancelled(GameObject who)
    {
        if(who!=owner) return;
        
        inputBuffer.RemoveBuffer("Dash");
    }
    
    void OnAttacked(GameObject attacker, AttackSO attackSO)
    {
        if(attacker!=owner) return;

        inputBuffer.RemoveBuffer("LightAttack");
        inputBuffer.RemoveBuffer("HeavyAttack");
    }

    void OnAttackCancelled(GameObject attacker)
    {
        if(attacker!=owner) return;

        inputBuffer.RemoveBuffer("LightAttack");
        inputBuffer.RemoveBuffer("HeavyAttack");
    }

    void OnRaisedParry(GameObject defender)
    {
        if(defender!=owner) return;

        inputBuffer.RemoveBuffer("Parry");
    }
    
    void OnParryCancelled(GameObject defender)
    {
        if(defender!=owner) return;

        inputBuffer.RemoveBuffer("Parry");
    }

}
