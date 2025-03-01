using UnityEngine;

public class State_SpyviAgent_AI_Idle : BaseState
{
    public override string stateName => "AI Idle";

    SpyviAgent agent;

    public State_SpyviAgent_AI_Idle(StateMachine_SpyviAgent sm)
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
