using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TikusAgent))]

public class StateMachine_TikusAgent : MonoBehaviour
{
    [HideInInspector]
    public TikusAgent agent;

    void Awake()
    {
        agent = GetComponent<TikusAgent>();

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
        State_TikusAgent_AI ai = new(this);

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
