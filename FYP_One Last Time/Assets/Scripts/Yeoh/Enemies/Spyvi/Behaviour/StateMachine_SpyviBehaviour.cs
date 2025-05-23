using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpyviBehaviour))]

public class StateMachine_SpyviBehaviour : MonoBehaviour
{
    public SpyviBehaviour behaviour {get; private set;}

    void Awake()
    {
        behaviour = GetComponent<SpyviBehaviour>();

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
        State_SpyviBehaviour_Laser laser = new(this);
        State_SpyviBehaviour_ShootTyre tyre = new(this);
        State_SpyviBehaviour_RevUp revUp = new(this);

        // HUB TRANSITIONS ================================================================================

        idle.AddTransition(rush, (timeInState) =>
        {
            if(
                behaviour.CurrentBehaviour() == behaviour.rushKeyword //&&
            ){
                return true;
            }
            return false;
        });
        
        idle.AddTransition(laser, (timeInState) =>
        {
            if(
                behaviour.CurrentBehaviour() == behaviour.laserKeyword //&&
            ){
                return true;
            }
            return false;
        });
        
        idle.AddTransition(tyre, (timeInState) =>
        {
            if(
                behaviour.CurrentBehaviour() == behaviour.shootTyreKeyword //&&
            ){
                return true;
            }
            return false;
        });
        
        idle.AddTransition(revUp, (timeInState) =>
        {
            if(
                behaviour.CurrentBehaviour() == behaviour.revUpKeyword //&&
            ){
                return true;
            }
            return false;
        });
        
        // RETURN TRANSITIONS ================================================================================

        rush.AddTransition(idle, (timeInState) =>
        {
            if(
                behaviour.CurrentBehaviour() != behaviour.rushKeyword //||
            ){
                return true;
            }
            return false;
        });

        laser.AddTransition(idle, (timeInState) =>
        {
            if(
                behaviour.CurrentBehaviour() != behaviour.laserKeyword //||
            ){
                return true;
            }
            return false;
        });

        tyre.AddTransition(idle, (timeInState) =>
        {
            if(
                behaviour.CurrentBehaviour() != behaviour.shootTyreKeyword //||
            ){
                return true;
            }
            return false;
        });

        revUp.AddTransition(idle, (timeInState) =>
        {
            if(
                behaviour.CurrentBehaviour() != behaviour.revUpKeyword //||
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
