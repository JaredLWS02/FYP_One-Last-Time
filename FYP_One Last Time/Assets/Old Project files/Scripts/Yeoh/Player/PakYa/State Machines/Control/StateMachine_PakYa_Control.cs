using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PakYa))]

public class StateMachine_PakYa_Control : MonoBehaviour
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

        State_PakYa_Control_None none = new(this);
        State_PakYa_Control_Player player = new(this);

        // HUB TRANSITIONS ================================================================================

        none.AddTransition(player, (timeInState) =>
        {
            if(
                pakya.pilot.type == Pilot.Type.Player //&&
            ){
                return true;
            }
            return false;
        });
        
        
        
        // RETURN TRANSITIONS ================================================================================

        player.AddTransition(none, (timeInState) =>
        {
            if(
                pakya.pilot.type != Pilot.Type.Player //||
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
