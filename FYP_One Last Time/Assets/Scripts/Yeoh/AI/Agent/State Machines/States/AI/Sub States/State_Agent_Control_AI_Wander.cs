using UnityEngine;

public class State_Agent_Control_AI_Wander : BaseState
{
    public override string Name => "AI Wander";

    AgentManager agent;

    public State_Agent_Control_AI_Wander(StateMachine_Agent_Control sm)
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
        agent.SetGoalToWander();

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
