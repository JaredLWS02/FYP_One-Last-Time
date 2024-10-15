using UnityEngine;

public class State_Wolf_Control_AI : BaseState
{
    public override string Name => "AI Controls";

    Wolf wolf;

    // SUB STATE MACHINE ================================================================================

    BaseState defaultSubState;

    public State_Wolf_Control_AI(StateMachine_Wolf_Control sm)
    {
        wolf = sm.wolf;

        subsm = new StateMachine();
        
        // SUB STATES ================================================================================

        State_Wolf_Control_AI_Idle idle = new(sm);
        State_Wolf_Control_AI_Seeking seeking = new(sm);
        State_Wolf_Control_AI_Fleeing fleeing = new(sm);
        State_Wolf_Control_AI_Staying staying = new(sm);

        // HUB TRANSITIONS ================================================================================

        idle.AddTransition(seeking, (timeInState) =>
        {
            if(
                wolf.ai.action == WolfAI.Action.Seeking //&&
            ){
                return true;
            }
            return false;
        });
        
        idle.AddTransition(fleeing, (timeInState) =>
        {
            if(
                wolf.ai.action == WolfAI.Action.Fleeing //&&
            ){
                return true;
            }
            return false;
        });
        
        idle.AddTransition(staying, (timeInState) =>
        {
            if(
                wolf.ai.action == WolfAI.Action.Staying //&&
            ){
                return true;
            }
            return false;
        });
        
        
        
        // RETURN TRANSITIONS ================================================================================

        seeking.AddTransition(idle, (timeInState) =>
        {
            if(
                wolf.ai.action != WolfAI.Action.Seeking //||
            ){
                return true;
            }
            return false;
        });
        
        fleeing.AddTransition(idle, (timeInState) =>
        {
            if(
                wolf.ai.action != WolfAI.Action.Fleeing //||
            ){
                return true;
            }
            return false;
        });
        
        staying.AddTransition(idle, (timeInState) =>
        {
            if(
                wolf.ai.action != WolfAI.Action.Staying //||
            ){
                return true;
            }
            return false;
        });
        
        

        // DEFAULT ================================================================================
        
        defaultSubState = idle;
        subsm.SetInitialState(defaultSubState);
    }

    protected override void OnEnter()
    {
        Debug.Log($"{wolf.gameObject.name} State: {Name}");

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
