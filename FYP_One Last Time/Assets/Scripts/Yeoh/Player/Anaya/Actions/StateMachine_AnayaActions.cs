using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnayaActions))]

public class StateMachine_AnayaActions : MonoBehaviour
{
    public AnayaActions action {get; private set;}

    void Awake()
    {
        action = GetComponent<AnayaActions>();

        Initialize();
    }

    // STATE MACHINE ================================================================================

    StateMachine sm;
    BaseState defaultState;

    void Initialize()
    {
        sm = new StateMachine();
        
        // STATES ================================================================================

        State_AnayaActions_Idle idle = new(this);
        State_AnayaActions_Casting casting = new(this);
        State_AnayaActions_HealAbility healAbility = new(this);

        // HUB TRANSITIONS ================================================================================
                
        idle.AddTransition(casting, (timeInState) =>
        {
            if(
                action.IsCasting() &&
                !action.IsHealing() //&&
            ){
                return true;
            }
            return false;
        });
                
        idle.AddTransition(healAbility, (timeInState) =>
        {
            if(
                !action.IsCasting() &&
                action.IsHealing() //&&
            ){
                return true;
            }
            return false;
        });
                
        // RETURN TRANSITIONS ================================================================================

        casting.AddTransition(idle, (timeInState) =>
        {
            if(
                !action.IsCasting() ||
                action.IsHealing() //||
            ){
                return true;
            }
            return false;
        });

        healAbility.AddTransition(idle, (timeInState) =>
        {
            if(
                action.IsCasting() ||
                !action.IsHealing() //||
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
