using UnityEngine;

public class State_EnemyAgent_Pilot_AI : BaseState
{
    public override string Name => "AI Pilot";

    EnemyAgent agent;

    // SUB STATE MACHINE ================================================================================

    BaseState defaultSubState;

    public State_EnemyAgent_Pilot_AI(StateMachine_EnemyAgent_Pilot sm)
    {
        agent = sm.agent;

        subsm = new StateMachine();
        
        // SUB STATES ================================================================================

        State_EnemyAgent_Pilot_AI_Idle idle = new(sm);
        State_EnemyAgent_Pilot_AI_Wander wander = new(sm);
        State_EnemyAgent_Pilot_AI_Seek seek = new(sm);
        State_EnemyAgent_Pilot_AI_Flee flee = new(sm);
        State_EnemyAgent_Pilot_AI_Return returnn = new(sm);

        // HUB TRANSITIONS ================================================================================

        idle.AddTransition(wander, (timeInState) =>
        {
            if(
                agent.wander &&
                !agent.targeting.target //&&
                //!agent.ShouldReturn() //&&
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(seek, (timeInState) =>
        {
            if(
                agent.targeting.CanSeeTarget() &&
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
                agent.flee &&
                agent.targeting.target &&
                agent.ShouldFlee() //&&
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(returnn, (timeInState) =>
        {
            if(
                agent.returner &&
                !agent.ShouldFlee() //&&
                //agent.targeting.ShouldReturn() //&&
            ){
                return true;
            }
            return false;
        });

        
        // RETURN TRANSITIONS ================================================================================

        wander.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.wander ||
                agent.targeting.CanSeeTarget() //||
                //agent.ShouldReturn() //||
            ){
                return true;
            }
            return false;
        });

        seek.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.targeting.target ||
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
                !agent.flee ||
                !agent.targeting.target ||
                !agent.ShouldFlee() //||
            ){
                return true;
            }
            return false;
        });

        returnn.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.returner ||
                agent.ShouldFlee() //||
                //agent.returner.IsAtSpawnpoint() //||
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
