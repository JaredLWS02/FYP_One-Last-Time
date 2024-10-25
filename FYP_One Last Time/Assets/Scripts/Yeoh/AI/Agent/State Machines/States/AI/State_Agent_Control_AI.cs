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
        State_Agent_Control_AI_Attacking attacking = new(sm);
        State_Agent_Control_AI_Fleeing fleeing = new(sm);
        State_Agent_Control_AI_Returning returning = new(sm);

        // HUB TRANSITIONS ================================================================================

        idle.AddTransition(attacking, (timeInState) =>
        {
            if(
                agent.GetEnemy() &&
                agent.IsOkHP() &&
                !agent.ShouldReturn() //&&
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(fleeing, (timeInState) =>
        {
            if(
                agent.GetEnemy() &&
                agent.IsLowHP() //&&
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(returning, (timeInState) =>
        {
            if(
                agent.ShouldReturn() //&&
            ){
                return true;
            }
            return false;
        });

        
        // RETURN TRANSITIONS ================================================================================

        attacking.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.GetEnemy() ||
                agent.IsLowHP() ||
                agent.ShouldReturn() //||
            ){
                return true;
            }
            return false;
        });

        fleeing.AddTransition(idle, (timeInState) =>
        {
            if(
                !agent.GetEnemy() ||
                !agent.IsLowHP() //||
            ){
                return true;
            }
            return false;
        });

        returning.AddTransition(idle, (timeInState) =>
        {
            if(
                agent.IsAtSpawnpoint() //||
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
        Debug.Log($"{agent.gameObject.name} State: {Name}");

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
