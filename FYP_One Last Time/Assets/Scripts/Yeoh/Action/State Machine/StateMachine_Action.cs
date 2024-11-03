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
        State_Action_WindingUpAttack windingUpAttack = new(this);
        State_Action_ReleasingAttack releasingAttack = new(this);
        State_Action_TryingToParry tryingToParry = new(this);
        State_Action_Parrying parrying = new(this);
        State_Action_Casting casting = new(this);
        State_Action_Healing healing = new(this);
        State_Action_Stunned stunned = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                action.IsGrounded() &&
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
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
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
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
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
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
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(windingUpAttack, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(releasingAttack, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(tryingToParry, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                                
        hub.AddTransition(parrying, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
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
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                action.IsCasting() &&
                !action.IsHealing() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(healing, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                action.IsHealing() &&
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
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsCasting() &&
                !action.IsHealing() &&
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
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
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
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
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
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
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
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        windingUpAttack.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                !action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        releasingAttack.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                !action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        tryingToParry.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                !action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        parrying.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                !action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
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
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                !action.IsCasting() ||
                action.IsHealing() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        healing.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                !action.IsHealing() ||
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
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsCasting() ||
                action.IsHealing() ||
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
