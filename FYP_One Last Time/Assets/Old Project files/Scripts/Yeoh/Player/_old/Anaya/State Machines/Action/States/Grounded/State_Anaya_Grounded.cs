using UnityEngine;

public class State_Anaya_Grounded : BaseState
{
    public override string Name => "Grounded";

    Anaya anaya;

    // SUB STATE MACHINE ================================================================================

    BaseState defaultSubState;

    public State_Anaya_Grounded(StateMachine_Anaya sm)
    {
        anaya = sm.anaya;

        subsm = new StateMachine();
        
        // SUB STATES ================================================================================

        State_Hub hub = new();
        State_Anaya_Standing standing = new(sm);
        State_Anaya_Crawling crawling = new(sm);
        State_Anaya_Commanding commanding = new(sm);

        // HUB TRANSITIONS ================================================================================

        hub.AddTransition(standing, (timeInState) =>
        {
            if(
                !anaya.IsCrawling() //&&
            ){
                return true;
            }
            return false;
        });

        hub.AddTransition(crawling, (timeInState) =>
        {
            if(
                anaya.IsCrawling() //&&
            ){
                return true;
            }
            return false;
        });
        
        hub.AddTransition(commanding, (timeInState) =>
        {
            // if(
            //     &&
            // ){
            //     return true;
            // }
            return false;
        });
        
        
        
        // RETURN TRANSITIONS ================================================================================

        standing.AddTransition(hub, (timeInState) =>
        {
            if(
                anaya.IsCrawling() //||
            ){
                return true;
            }
            return false;
        });

        crawling.AddTransition(hub, (timeInState) =>
        {
            if(
                !anaya.IsCrawling() //||
            ){
                return true;
            }
            return false;
        });
        
        commanding.AddTransition(hub, (timeInState) =>
        {
            // if(
            //     ||
            // ){
            //     return true;
            // }
            return false;
        });
        
        

        // DEFAULT ================================================================================
        
        defaultSubState = hub;
        subsm.SetInitialState(defaultSubState);
    }

    protected override void OnEnter()
    {
        Debug.Log($"{anaya.gameObject.name} State: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
