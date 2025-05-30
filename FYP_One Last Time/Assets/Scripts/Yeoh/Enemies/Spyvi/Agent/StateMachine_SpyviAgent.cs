using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpyviAgent))]

public class StateMachine_SpyviAgent : MonoBehaviour
{
    [HideInInspector]
    public SpyviAgent agent;

    void Awake()
    {
        agent = GetComponent<SpyviAgent>();

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
        State_SpyviAgent_AI ai = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(ai, (timeInState) =>
        {
            if(
                agent.pilot.IsAI() //&&
            ){
                return true;
            }
            return false;
        });
        
        // RETURN TRANSITIONS ================================================================================

        ai.AddTransition(hub, (timeInState) =>
        {
            if(
                !agent.pilot.IsAI() //||
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
