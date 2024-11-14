using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentManager))]

public class StateMachine_Agent : MonoBehaviour
{
    public AgentManager agent {get; private set;}

    void Awake()
    {
        agent = GetComponent<AgentManager>();

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
        State_Agent_Grounded grounded = new(this);
        State_Agent_MidAir midair = new(this);
        State_Agent_AutoJumping autojumping = new(this);
        State_Agent_Attacking attacking = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                agent.IsGrounded() &&
                !agent.IsAutoJumping() &&
                !agent.IsAttacking() //&&
            ){
                return true;
            }
            return false;
        });

        hub.AddTransition(midair, (timeInState) =>
        {
            if(
                !agent.IsGrounded() &&
                !agent.IsAutoJumping() &&
                !agent.IsAttacking() //&&
            ){
                return true;
            }
            return false;
        });                

        hub.AddTransition(autojumping, (timeInState) =>
        {
            if(
                agent.IsAutoJumping() &&
                !agent.IsAttacking() //&&
            ){
                return true;
            }
            return false;
        });   

        hub.AddTransition(attacking, (timeInState) =>
        {
            if(
                !agent.IsAutoJumping() &&
                agent.IsAttacking() //&&
            ){
                return true;
            }
            return false;
        });
                
        
        // RETURN TRANSITIONS ================================================================================

        grounded.AddTransition(hub, (timeInState) =>
        {
            if(
                !agent.IsGrounded() ||
                agent.IsAutoJumping() ||
                agent.IsAttacking() //||
            ){
                return true;
            }
            return false;
        });

        midair.AddTransition(hub, (timeInState) =>
        {
            if(
                agent.IsGrounded() ||
                agent.IsAutoJumping() ||
                agent.IsAttacking() //||
            ){
                return true;
            }
            return false;
        });

        autojumping.AddTransition(hub, (timeInState) =>
        {
            if(
                !agent.IsAutoJumping() ||
                agent.IsAttacking() //||
            ){
                return true;
            }
            return false;
        });
        
        attacking.AddTransition(hub, (timeInState) =>
        {
            if(
                agent.IsAutoJumping() ||
                !agent.IsAttacking() //||
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
