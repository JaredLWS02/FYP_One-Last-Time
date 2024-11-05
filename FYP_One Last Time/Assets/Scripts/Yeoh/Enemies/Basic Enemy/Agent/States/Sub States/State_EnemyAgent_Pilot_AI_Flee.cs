using UnityEngine;

public class State_EnemyAgent_Pilot_AI_Flee : BaseState
{
    public override string Name => "AI Flee";

    EnemyAgent agent;

    public State_EnemyAgent_Pilot_AI_Flee(StateMachine_EnemyAgent_Pilot sm)
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
        string behaviour = agent.GetRandomFleeBehaviour();

        switch(behaviour)
        {
            case "Flee": Flee(); break;
            case "Wander": Wander(); break;
            default: Flee(); break;
        }
    }

    void Wander()
    {
        agent.SetGoalToWander();
        agent.FaceTarget();
    }

    void Flee()
    {
        agent.SetThreatToTarget();
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
