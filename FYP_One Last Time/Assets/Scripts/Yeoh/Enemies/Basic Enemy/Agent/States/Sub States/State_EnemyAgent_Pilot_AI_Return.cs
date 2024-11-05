using UnityEngine;

public class State_EnemyAgent_Pilot_AI_Return : BaseState
{
    public override string Name => "AI Return";

    EnemyAgent agent;

    public State_EnemyAgent_Pilot_AI_Return(StateMachine_EnemyAgent_Pilot sm)
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
        agent.SetGoalToReturn();
        
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
