using UnityEngine;

public class State_Agent_Control_AI_Idle : BaseState
{
    public override string Name => "AI Idle";

    AgentManager agent;

    public State_Agent_Control_AI_Idle(StateMachine_Agent_Control sm)
    {
        agent = sm.agent;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{agent.gameObject.name} SubState: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        agent.SetGoalWander();

        agent.FaceMoveDir();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
