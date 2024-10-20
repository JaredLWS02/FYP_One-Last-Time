using UnityEngine;

public class State_Agent_Control_AI_Attacking : BaseState
{
    public override string Name => "AI Attacking";

    AgentAI agent;

    public State_Agent_Control_AI_Attacking(StateMachine_Agent_Control sm)
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
        if(agent.IsEnemyTooClose())
        {
            agent.SetThreatEnemy();
        }
        else
        {
            agent.SetGoalEnemy();
        }

        agent.FaceEnemy();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
