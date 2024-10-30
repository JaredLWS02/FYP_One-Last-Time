using UnityEngine;

public class State_Agent_Control_AI_Fleeing : BaseState
{
    public override string Name => "AI Fleeing";

    AgentManager agent;

    public State_Agent_Control_AI_Fleeing(StateMachine_Agent_Control sm)
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
        agent.SetGoalWander();
    }

    void Flee()
    {
        agent.SetThreatEnemy();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
