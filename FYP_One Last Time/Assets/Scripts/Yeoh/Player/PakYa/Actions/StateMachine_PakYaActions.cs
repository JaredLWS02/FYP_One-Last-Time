using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PakYaActions))]

public class StateMachine_PakYaActions : MonoBehaviour
{
    [HideInInspector]
    public PakYaActions action;

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
        State_PakYaActions_Dashing dashing = new(this);
        State_PakYaActions_AttackWindingUp attackWindingUp = new(this);
        State_PakYaActions_AttackReleasing attackReleasing = new(this);
        State_PakYaActions_TryingToParry tryingToParry = new(this);
        State_PakYaActions_Parrying parrying = new(this);
        State_PakYaActions_Casting casting = new(this);
        State_PakYaActions_HealAbility healAbility = new(this);
        State_PakYaActions_Stunned stunned = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                action.IsGrounded() &&
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
                
        hub.AddTransition(attackWindingUp, (timeInState) =>
        {
            if(
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
                
        hub.AddTransition(attackReleasing, (timeInState) =>
        {
            if(
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
                
        hub.AddTransition(healAbility, (timeInState) =>
        {
            if(
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

        attackWindingUp.AddTransition(hub, (timeInState) =>
        {
            if(
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

        attackReleasing.AddTransition(hub, (timeInState) =>
        {
            if(
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

        healAbility.AddTransition(hub, (timeInState) =>
        {
            if(
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
