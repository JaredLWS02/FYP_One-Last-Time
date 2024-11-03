using UnityEngine;

public class State_Agent_Control_None : BaseState
{
    public override string Name => "No Control";

    AgentManager agent;

    public State_Agent_Control_None(StateMachine_Agent_Control sm)
    {
        agent = sm.agent;
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
