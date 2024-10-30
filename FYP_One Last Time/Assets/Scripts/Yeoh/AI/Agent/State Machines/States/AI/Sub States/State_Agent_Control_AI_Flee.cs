using UnityEngine;

public class State_Agent_Control_AI_Flee : BaseState
{
    public override string Name => "AI Flee";

    AgentManager agent;

    public State_Agent_Control_AI_Flee(StateMachine_Agent_Control sm)
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
        string behaviour = agent.GetRandomFleeBehaviour();

        switch(behaviour)
        {
            case "Flee": Flee(); break;
            case "Wander": Wander(); break;
            default: Flee(); break;
        }

        agent.FaceMoveDir();
    }

    void Wander()
    {
        agent.SetGoalToWander();
    }

    void Flee()
    {
        agent.SetThreatToTarget();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
