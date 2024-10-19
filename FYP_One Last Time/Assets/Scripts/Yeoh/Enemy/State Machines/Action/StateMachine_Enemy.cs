using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]

public class StateMachine_Enemy : MonoBehaviour
{
    [HideInInspector]
    public EnemyAI ai;

    void Awake()
    {
        ai = GetComponent<EnemyAI>();

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
        State_Enemy_Grounded grounded = new(this);
        State_Enemy_MidAir midair = new(this);
        State_Enemy_AutoJumping autojumping = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                ai.IsGrounded() &&
                !ai.IsAutoJumping() //&&
            ){
                return true;
            }
            return false;
        });

        hub.AddTransition(midair, (timeInState) =>
        {
            if(
                !ai.IsGrounded() &&
                !ai.IsAutoJumping() //&&
            ){
                return true;
            }
            return false;
        });                

        hub.AddTransition(autojumping, (timeInState) =>
        {
            if(
                ai.IsAutoJumping() //&&
            ){
                return true;
            }
            return false;
        });                

        
        // RETURN TRANSITIONS ================================================================================

        grounded.AddTransition(hub, (timeInState) =>
        {
            if(
                !ai.IsGrounded() ||
                ai.IsAutoJumping() //||
            ){
                return true;
            }
            return false;
        });

        midair.AddTransition(hub, (timeInState) =>
        {
            if(
                ai.IsGrounded() ||
                ai.IsAutoJumping() //||
            ){
                return true;
            }
            return false;
        });

        autojumping.AddTransition(hub, (timeInState) =>
        {
            if(
                !ai.IsAutoJumping() //||
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
