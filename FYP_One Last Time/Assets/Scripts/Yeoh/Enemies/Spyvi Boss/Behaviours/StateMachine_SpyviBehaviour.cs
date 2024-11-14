using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpyviActions))]

public class StateMachine_SpyviBehaviour : MonoBehaviour
{
    public SpyviActions spyvi {get; private set;}

    void Awake()
    {
        spyvi = GetComponent<SpyviActions>();

        Initialize();
    }

    // STATE MACHINE ================================================================================

    StateMachine sm;
    BaseState defaultState;

    void Initialize()
    {
        sm = new StateMachine();
        
        // STATES ================================================================================

        State_SpyviBehaviour_Idle idle = new(this);
        State_SpyviBehaviour_Rush rush = new(this);
        //State_SpyviBehaviour_Rush rush = new(this);

        // HUB TRANSITIONS ================================================================================

        idle.AddTransition(rush, (timeInState) =>
        {
            if(
                spyvi.CurrentBehaviour() == spyvi.rushKeyword //&&
            ){
                return true;
            }
            return false;
        });
        
        // RETURN TRANSITIONS ================================================================================

        rush.AddTransition(idle, (timeInState) =>
        {
            if(
                spyvi.CurrentBehaviour() != spyvi.rushKeyword //||
            ){
                return true;
            }
            return false;
        });

        // DEFAULT ================================================================================
        
        defaultState = idle;
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
