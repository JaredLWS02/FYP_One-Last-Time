using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyActions))]

public class StateMachine_EnemyActions : MonoBehaviour
{
    public EnemyActions action {get; private set;}

    void Awake()
    {
        action = GetComponent<EnemyActions>();

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
        State_EnemyActions_Grounded grounded = new(this);
        State_EnemyActions_MidAir midAir = new(this);
        State_EnemyActions_AutoJumping autoJumping = new(this);
        State_EnemyActions_AttackWindingUp attackWindingUp = new(this);
        State_EnemyActions_AttackReleasing attackReleasing = new(this);
        State_EnemyActions_Stunned stunned = new(this);
        State_EnemyActions_Death death = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                action.IsGrounded() &&
                !action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsStunned() &&
                !action.IsDead() //&&
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
                !action.IsStunned() &&
                !action.IsDead() //&&
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
                !action.IsStunned() &&
                !action.IsDead() //&&
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
                !action.IsStunned() &&
                !action.IsDead() //&&
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
                !action.IsStunned() &&
                !action.IsDead() //&&
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
                action.IsStunned() &&
                !action.IsDead() //&&
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(death, (timeInState) =>
        {
            if(
                !action.IsAutoJumping() &&
                !action.IsWindingUpAttack() &&
                !action.IsReleasingAttack() &&
                !action.IsStunned() &&
                action.IsDead() //&&
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
                action.IsStunned() ||
                action.IsDead() //||
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
                action.IsStunned() ||
                action.IsDead() //||
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
                action.IsStunned() ||
                action.IsDead() //||
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
                action.IsStunned() ||
                action.IsDead() //||
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
                action.IsStunned() ||
                action.IsDead() //||
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
                !action.IsStunned() ||
                action.IsDead() //||
            ){
                return true;
            }
            return false;
        });

        death.AddTransition(hub, (timeInState) =>
        {
            if(
                action.IsAutoJumping() ||
                action.IsWindingUpAttack() ||
                action.IsReleasingAttack() ||
                action.IsStunned() ||
                !action.IsDead() //||
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
