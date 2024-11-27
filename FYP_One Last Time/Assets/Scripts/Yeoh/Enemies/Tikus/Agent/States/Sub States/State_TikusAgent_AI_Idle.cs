using UnityEngine;

public class State_TikusAgent_AI_Idle : BaseState
{
    public override string stateName => "AI Idle";

    TikusAgent agent;

    public State_TikusAgent_AI_Idle(StateMachine_TikusAgent sm)
    {
        agent = sm.agent;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{agent.owner.name} SubState: {stateName}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        agent.vehicle.SetGoalToSelf();

        agent.move.FaceMoveDir();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
