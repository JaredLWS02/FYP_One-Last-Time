using UnityEngine;

public class State_Agent_Control_AI_Attacking : BaseState
{
    public override string Name => "AI Attacking";

    AgentManager agent;

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
        string behaviour = agent.GetRandomAttackBehaviour();

        switch(behaviour)
        {
            case "Attack": Attack(); break;
            case "Wander": Wander(); break;
            case "Flee": Flee(); break;
            default: Attack(); break;
        }

        agent.FaceEnemy();
    }

    void Attack()
    {
        if(agent.IsEnemyTooClose())
        {
            agent.SetThreatEnemy();
        }
        else
        {
            agent.SetGoalEnemy();
        }
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
