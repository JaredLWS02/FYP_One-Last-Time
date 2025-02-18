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
        State_AnayaActions_Ability ability = new(this);
        State_AnayaActions_Stunned stunned = new(this);
        
        // HUB TRANSITIONS ================================================================================
                
        idle.AddTransition(casting, (timeInState) =>
        {
            if(
                action.IsCasting() &&
                !action.IsDoingAbility() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        idle.AddTransition(ability, (timeInState) =>
        {
            if(
                action.IsDoingAbility() &&
                !action.IsStunned() //&&
            ){
                return true;
            }
            return false;
        });
                
        idle.AddTransition(stunned, (timeInState) =>
        {
            if(
                action.IsStunned() //&&
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
                action.IsDoingAbility() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        ability.AddTransition(idle, (timeInState) =>
        {
            if(
                !action.IsDoingAbility() ||
                action.IsStunned() //||
            ){
                return true;
            }
            return false;
        });

        stunned.AddTransition(idle, (timeInState) =>
        {
            if(
                !action.IsStunned() //||
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
