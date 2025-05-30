using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PakYaActions))]

public class StateMachine_PakYaActions : MonoBehaviour
{
    public PakYaActions action {get; private set;}

    void Awake()
    {
        action = GetComponent<PakYaActions>();

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
        State_PakYaActions_Grounded grounded = new(this);
        State_PakYaActions_MidAir midAir = new(this);
        State_PakYaActions_WallClinging clinging = new(this);
        State_PakYaActions_Dashing dashing = new(this);
        State_PakYaActions_AttackWindingUp attackWindingUp = new(this);
        State_PakYaActions_AttackReleasing attackReleasing = new(this);
        State_PakYaActions_TryingToParry tryingToParry = new(this);
        State_PakYaActions_Parrying parrying = new(this);
        State_PakYaActions_Stunned stunned = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                action.IsGrounded() &&
                !action.IsClinging() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
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
                !action.IsClinging() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(clinging, (timeInState) =>
        {
            if(
                action.IsClinging() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(dashing, (timeInState) =>
        {
            if(
                !action.IsClinging() &&
                action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(attackWindingUp, (timeInState) =>
        {
            if(
                !action.IsClinging() &&
                !action.IsDashing() &&
                action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(attackReleasing, (timeInState) =>
        {
            if(
                !action.IsClinging() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(tryingToParry, (timeInState) =>
        {
            if(
                !action.IsClinging() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                                
        hub.AddTransition(parrying, (timeInState) =>
        {
            if(
                !action.IsClinging() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                action.IsParrying() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(stunned, (timeInState) =>
        {
            if(
                !action.IsClinging() &&
                !action.IsDashing() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
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
                action.IsClinging() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
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
                action.IsClinging() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        clinging.AddTransition(hub, (timeInState) =>
        {
            if(
                !action.IsClinging() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        dashing.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsClinging() ||
                !action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        attackWindingUp.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsClinging() ||
                action.IsDashing() ||
                !action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        attackReleasing.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsClinging() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                !action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        tryingToParry.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsClinging() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                !action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        parrying.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsClinging() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                !action.IsParrying() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        stunned.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsClinging() ||
                action.IsDashing() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
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
