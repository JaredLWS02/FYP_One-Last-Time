using UnityEngine;

public class State_Agent_MidAir : BaseState
{
    public override string Name => "MidAir";

    AgentAI agent;

    public State_Agent_MidAir(StateMachine_Agent sm)
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
        agent.AllowMoveX = true;
        agent.AllowMoveY = true;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        agent.AllowJump = toggle;
    }
}
