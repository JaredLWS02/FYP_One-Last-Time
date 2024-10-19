using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]

public class StateMachine_Enemy_Control : MonoBehaviour
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

        State_Enemy_Control_None none = new(this);
        State_Enemy_Control_AI ai = new(this);

        // HUB TRANSITIONS ================================================================================

        none.AddTransition(ai, (timeInState) =>
        {
            if(
                this.ai.pilot.IsAI() //&&
            ){
                return true;
            }
            return false;
        });
        
        
        
        // RETURN TRANSITIONS ================================================================================

        ai.AddTransition(none, (timeInState) =>
        {
            if(
                !this.ai.pilot.IsAI() //||
            ){
                return true;
            }
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
