using UnityEngine;

public class State_Agent_Control_AI : BaseState
{
    public override string Name => "AI Control";

    AgentManager agent;

    // SUB STATE MACHINE ================================================================================

    BaseState defaultSubState;

    public State_Agent_Control_AI(StateMachine_Agent_Control sm)
    {
        agent = sm.agent;

        subsm = new StateMachine();
        
        // SUB STATES ================================================================================

        State_Agent_Control_AI_Idle idle = new(sm);
        State_Agent_Control_AI_Wander wander = new(sm);
        State_Agent_Control_AI_Seek seek = new(sm);
        State_Agent_Control_AI_Flee flee = new(sm);
        State_Agent_Control_AI_Return returnn = new(sm);

        // HUB TRANSITIONS ================================================================================

        idle.AddTransition(wander, (timeInState) =>
        {
            if(
                agent.allowWander &&
                !agent.GetTarget() //&&
                //!agent.ShouldReturn() //&&
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(seek, (timeInState) =>
        {
            if(
                agent.allowSeek &&
                agent.GetTarget() &&
                !agent.ShouldFlee() //&&
                //!agent.ShouldReturn() //&&
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(flee, (timeInState) =>
        {
            if(
                agent.allowFlee &&
                agent.GetTarget() &&
                agent.ShouldFlee() //&&
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(returnn, (timeInState) =>
        {
            if(
                agent.allowReturn &&
                !agent.ShouldFlee() //&&
                //agent.ShouldReturn() //&&
            ){
                return true;
            }
            return false;
        });

        
        // RETURN TRANSITIONS ================================================================================

        wander.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.allowWander ||
                agent.GetTarget() //||
                //agent.ShouldReturn() //||
            ){
                return true;
            }
            return false;
        });

        seek.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.allowSeek ||
                !agent.GetTarget() ||
                agent.ShouldFlee() //||
                //agent.ShouldReturn() //||
            ){
                return true;
            }
            return false;
        });

        flee.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.allowFlee ||
                !agent.GetTarget() ||
                !agent.ShouldFlee() //||
            ){
                return true;
            }
            return false;
        });

        returnn.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.allowReturn ||
                agent.ShouldFlee() //||
                //agent.IsAtSpawnpoint() //||
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
        Debug.Log($"{agent.owner.name} State: {Name}");

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
