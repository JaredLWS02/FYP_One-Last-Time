using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionManager))]

public class StateMachine_Action : MonoBehaviour
{
    [HideInInspector]
    public ActionManager action;

    void Awake()
    {
        action = GetComponent<ActionManager>();

        Initialize();
    }

    // STATE MACHINE ================================================================================

    StateMachine sm;
    BaseState defaultState;

    void Initialize()
    {
        sm = new StateMachine();
        
        // STATES ================================================================================

        State_Hub hub = new();
        State_Action_Grounded grounded = new(this);
        State_Action_MidAir midAir = new(this);
        State_Action_AutoJumping autoJumping = new(this);
        State_Action_Dashing dashing = new(this);
        State_Action_AttackWindingUp attackWindingUp = new(this);
        State_Action_AttackReleasing attackReleasing = new(this);
        State_Action_ParryRaised parryRaised = new(this);
        State_Action_ParryLowering parryLowering = new(this);
        State_Action_ParrySuccessing parrySuccessing = new(this);
        State_Action_Casting casting = new(this);
        State_Action_Cast cast = new(this);
        State_Action_Stunned stunned = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                action.IsGrounded() &&
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });

        hub.AddTransition(midAir, (timeInState) =>
        {
            if(
                !action.IsGrounded() &&
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(autoJumping, (timeInState) =>
        {
            if(
                action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(dashing, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(attackWindingUp, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(attackReleasing, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(parryRaised, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(parryLowering, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(parrySuccessing, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(casting, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                action.IsCasting() &&
                !action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(cast, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                action.IsCast() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(stunned, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttackWindingUp() &&
                !action.IsAttackReleasing() &&
                !action.IsParryRaised() &&
                !action.IsParryLowering() &&
                !action.IsParrySuccessing() &&
                !action.IsCasting() &&
                !action.IsCast() &&
                action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        // RETURN TRANSITIONS ================================================================================

        grounded.AddTransition(hub, (timeInState) =>
        {
            if(
                !action.IsGrounded() ||
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        midAir.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsGrounded() ||
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        autoJumping.AddTransition(hub, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        dashing.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                !action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        attackWindingUp.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                !action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        attackReleasing.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                !action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        parryRaised.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                !action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        parryLowering.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                !action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        parrySuccessing.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                !action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        casting.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                !action.IsCasting() ||
                action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        cast.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                !action.IsCast() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        stunned.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttackWindingUp() ||
                action.IsAttackReleasing() ||
                action.IsParryRaised() ||
                action.IsParryLowering() ||
                action.IsParrySuccessing() ||
                action.IsCasting() ||
                action.IsCast() ||
                !action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        // DEFAULT ================================================================================
        
        defaultState = hub;
        sm.SetInitialState(defaultState);
    }

    void Update()
    {
        sm.Tick(Time.deltaTime);
    }

    void OnDisable()
    {
        if(sm!=null)
        {
            sm.currentState.Exit(); // call OnExit on current state
            sm.SetState(defaultState); // Change back to default state
        }
    }
}
