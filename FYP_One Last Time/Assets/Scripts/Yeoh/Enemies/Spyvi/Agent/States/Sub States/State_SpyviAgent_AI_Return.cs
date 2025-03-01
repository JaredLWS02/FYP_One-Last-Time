using UnityEngine;

public class State_SpyviAgent_AI_Return : BaseState
{
    public override string stateName => "AI Return";

    SpyviAgent agent;

    public State_SpyviAgent_AI_Return(StateMachine_SpyviAgent sm)
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
        agent.returner.SetGoalToReturn();
        
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
