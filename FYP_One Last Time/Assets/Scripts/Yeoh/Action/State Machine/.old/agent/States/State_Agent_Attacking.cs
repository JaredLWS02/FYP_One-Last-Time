using UnityEngine;

public class State_Agent_Attacking : BaseState
{
    public override string Name => "Attacking";

    AgentManager agent;

    public State_Agent_Attacking(StateMachine_Agent sm)
    {
        agent = sm.agent;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{agent.gameObject.name} State: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        agent.AllowMoveX = false;
        agent.AllowMoveY = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        agent.AllowAttack = toggle;
    }
}
