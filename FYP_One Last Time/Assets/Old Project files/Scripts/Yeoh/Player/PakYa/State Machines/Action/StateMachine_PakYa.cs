using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PakYa))]

public class StateMachine_PakYa : MonoBehaviour
{
    [HideInInspector]
    public PakYa pakya;

    void Awake()
    {
        pakya = GetComponent<PakYa>();

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
        State_PakYa_Grounded grounded = new(this);
        State_PakYa_MidAir midair = new(this);
        State_PakYa_Dashing dashing = new(this);
        State_PakYa_Casting casting = new(this);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(grounded, (timeInState) =>
        {
            if(
                pakya.IsGrounded() &&
                !pakya.IsDashing() &&
                !pakya.IsCasting()
            ){
                return true;
            }
            return false;
        });

        hub.AddTransition(midair, (timeInState) =>
        {
            if(
                !pakya.IsGrounded() &&
                !pakya.IsDashing() &&
                !pakya.IsCasting()
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(dashing, (timeInState) =>
        {
            if(
                pakya.IsDashing() &&
                !pakya.IsCasting()
            ){
                return true;
            }
            return false;
        });
                
        hub.AddTransition(casting, (timeInState) =>
        {
            if(
                pakya.IsGrounded() &&
                !pakya.IsDashing() &&
                pakya.IsCasting()
            ){
                return true;
            }
            return false;
        });
                
        
        
        // RETURN TRANSITIONS ================================================================================

        grounded.AddTransition(hub, (timeInState) =>
        {
            if(
                !pakya.IsGrounded() ||
                pakya.IsDashing() ||
                pakya.IsCasting()
            ){
                return true;
            }
            return false;
        });

        midair.AddTransition(hub, (timeInState) =>
        {
            if(
                pakya.IsGrounded() ||
                pakya.IsDashing() ||
                pakya.IsCasting()
            ){
                return true;
            }
            return false;
        });

        dashing.AddTransition(hub, (timeInState) =>
        {
            if(
                !pakya.IsDashing() ||
                pakya.IsCasting()
            ){
                return true;
            }
            return false;
        });

        casting.AddTransition(hub, (timeInState) =>
        {
            if(
                !pakya.IsGrounded() ||
                pakya.IsDashing() ||
                !pakya.IsCasting()
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
