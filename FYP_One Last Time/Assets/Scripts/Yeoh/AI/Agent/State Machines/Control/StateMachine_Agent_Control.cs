using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentAI))]

public class StateMachine_Agent_Control : MonoBehaviour
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

        State_Agent_Control_None none = new(this);
        State_Agent_Control_AI ai = new(this);

        // HUB TRANSITIONS ================================================================================

        none.AddTransition(ai, (timeInState) =>
        {
            // if(
            //     this.agent.pilot.IsAI() //&&
            // ){
            //     return true;
            // }
            // return false;
            return true;
        });
        
        
        
        // RETURN TRANSITIONS ================================================================================

        ai.AddTransition(none, (timeInState) =>
        {
            // if(
            //     !this.agent.pilot.IsAI() //||
            // ){
            //     return true;
            // }
            return false;
        });

        

        // DEFAULT ================================================================================
        
        defaultState = none;
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
