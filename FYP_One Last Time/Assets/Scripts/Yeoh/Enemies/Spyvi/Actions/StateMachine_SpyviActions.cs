using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpyviActions))]

public class StateMachine_SpyviActions : MonoBehaviour
{
    public SpyviActions action {get; private set;}

    void Awake()
    {
        action = GetComponent<SpyviActions>();

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
        State_SpyviActions_Grounded grounded = new(this);
        State_SpyviActions_MidAir midAir = new(this);
        State_SpyviActions_AutoJumping autoJumping = new(this);
        State_SpyviActions_AttackWindingUp attackWindingUp = new(this);
        State_SpyviActions_AttackReleasing attackReleasing = new(this);
        State_SpyviActions_TryingToParry tryingToParry = new(this);
        State_SpyviActions_Parrying parrying = new(this);
        State_SpyviActions_Stunned stunned = new(this);
        State_SpyviActions_Phasing phasing = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                action.IsGrounded() &&
                !action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() &&
                !action.IsPhasing() //&&
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
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() &&
                !action.IsPhasing() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(autoJumping, (timeInState) =>
        {
            if(
                action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() &&
                !action.IsPhasing() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(attackWindingUp, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() &&
                !action.IsPhasing() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(attackReleasing, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() &&
                !action.IsPhasing() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(tryingToParry, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() &&
                !action.IsPhasing() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(parrying, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                action.IsParrying() &&
                !action.IsStunned() &&
                !action.IsPhasing() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(stunned, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                action.IsStunned() &&
                !action.IsPhasing() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(phasing, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsTryingToParry() &&
                !action.IsParrying() &&
                !action.IsStunned() &&
                action.IsPhasing() //&&
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
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() ||
                action.IsPhasing() //||
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
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() ||
                action.IsPhasing() //||
            ){
                return true;
            }
            return false;
        });

        autoJumping.AddTransition(hub, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() ||
                action.IsPhasing() //||
            ){
                return true;
            }
            return false;
        });

        attackWindingUp.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                !action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() ||
                action.IsPhasing() //||
            ){
                return true;
            }
            return false;
        });

        attackReleasing.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsWindingUpAttack() ||
                !action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() ||
                action.IsPhasing() //||
            ){
                return true;
            }
            return false;
        });

        tryingToParry.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                !action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() ||
                action.IsPhasing() //||
            ){
                return true;
            }
            return false;
        });

        parrying.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                !action.IsParrying() ||
                action.IsStunned() ||
                action.IsPhasing() //||
            ){
                return true;
            }
            return false;
        });

        stunned.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                !action.IsStunned() ||
                action.IsPhasing() //||
            ){
                return true;
            }
            return false;
        });

        phasing.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsTryingToParry() ||
                action.IsParrying() ||
                action.IsStunned() ||
                !action.IsPhasing() //||
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
