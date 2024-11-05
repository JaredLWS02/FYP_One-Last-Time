using UnityEngine;

public class State_EnemyAgent_Pilot_AI_Wander : BaseState
{
    public override string Name => "AI Wander";

    EnemyAgent agent;

    public State_EnemyAgent_Pilot_AI_Wander(StateMachine_EnemyAgent_Pilot sm)
    {
        agent = sm.agent;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{agent.owner.name} SubState: {Name}");

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
