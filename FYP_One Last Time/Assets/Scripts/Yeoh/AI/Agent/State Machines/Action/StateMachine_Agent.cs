using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentAI))]

public class StateMachine_Agent : MonoBehaviour
{
    [HideInInspector]
    public AgentAI agent;

    void Awake()
    {
        agent = GetComponent<AgentAI>();

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

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                agent.IsGrounded() &&
                !agent.IsAutoJumping() //&&
            ){
                return true;
            }
            return false;
        });

        hub.AddTransition(midair, (timeInState) =>
        {
            if(
                !agent.IsGrounded() &&
                !agent.IsAutoJumping() //&&
            ){
                return true;
            }
            return false;
        });                

        hub.AddTransition(autojumping, (timeInState) =>
        {
            if(
                agent.IsAutoJumping() //&&
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
                agent.IsAutoJumping() //||
            ){
                return true;
            }
            return false;
        });

        midair.AddTransition(hub, (timeInState) =>
        {
            if(
                agent.IsGrounded() ||
                agent.IsAutoJumping() //||
            ){
                return true;
            }
            return false;
        });

        autojumping.AddTransition(hub, (timeInState) =>
        {
            if(
                !agent.IsAutoJumping() //||
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
