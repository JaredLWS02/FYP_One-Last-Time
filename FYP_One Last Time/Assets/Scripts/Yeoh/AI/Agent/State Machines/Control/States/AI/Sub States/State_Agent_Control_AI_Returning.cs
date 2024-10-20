using UnityEngine;

public class State_Agent_Control_AI_Returning : BaseState
{
    public override string Name => "AI Returning";

    AgentAI agent;

    public State_Agent_Control_AI_Returning(StateMachine_Agent_Control sm)
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
        agent.SetGoalSpawnpoint();
        
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
