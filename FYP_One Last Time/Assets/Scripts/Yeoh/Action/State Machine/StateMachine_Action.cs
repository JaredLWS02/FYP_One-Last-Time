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
        State_Action_MidAir midair = new(this);
        State_Action_AutoJumping autojumping = new(this);
        State_Action_Dashing dashing = new(this);
        State_Action_Attacking attacking = new(this);
        State_Action_Casting casting = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                action.IsGrounded() &&
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttacking() &&
                !action.IsCasting() //&&
            ){
                return true;
            }
            return false;
        });

        hub.AddTransition(midair, (timeInState) =>
        {
            if(
                !action.IsGrounded() &&
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttacking() &&
                !action.IsCasting() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(autojumping, (timeInState) =>
        {
            if(
                action.IsAutoJumping() &&
                !action.IsDashing() &&
                !action.IsAttacking() &&
                !action.IsCasting() //&&
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
                !action.IsAttacking() &&
                !action.IsCasting() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(attacking, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsDashing() &&
                action.IsAttacking() &&
                !action.IsCasting() //&&
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
                !action.IsAttacking() &&
                action.IsCasting() //&&
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
                action.IsAttacking() ||
                action.IsCasting() //||
            ){
                return true;
            }
            return false;
        });

        midair.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsGrounded() ||
                action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttacking() ||
                action.IsCasting() //||
            ){
                return true;
            }
            return false;
        });

        autojumping.AddTransition(hub, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() ||
                action.IsDashing() ||
                action.IsAttacking() ||
                action.IsCasting() //||
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
                action.IsAttacking() ||
                action.IsCasting() //||
            ){
                return true;
            }
            return false;
        });

        attacking.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsDashing() ||
                !action.IsAttacking() ||
                action.IsCasting() //||
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
                action.IsAttacking() ||
                !action.IsCasting() //||
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
