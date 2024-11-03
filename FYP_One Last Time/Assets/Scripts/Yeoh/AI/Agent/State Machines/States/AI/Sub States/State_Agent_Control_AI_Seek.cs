using UnityEngine;

public class State_Agent_Control_AI_Seek : BaseState
{
    public override string Name => "AI Seek";

    AgentManager agent;

    public State_Agent_Control_AI_Seek(StateMachine_Agent_Control sm)
    {
        agent = sm.agent;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{agent.owner.name} SubState: {Name}");

        ToggleAllow(true);

        agent.ExpandRadarRange();
    }

    protected override void OnUpdate(float deltaTime)
    {
        string behaviour = agent.GetRandomSeekBehaviour();

        switch(behaviour)
        {
            case "Seek": Seek(); break;
            case "Wander": Wander(); break;
            case "Flee": Flee(); break;
            default: Seek(); break;
        }

        agent.FaceTarget();
    }

    void Seek()
    {
        if(agent.IsTargetTooClose())
        {
            agent.SetThreatToTarget();
        }
        else
        {
            agent.SetGoalToSeek();
        }
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

        agent.RevertRadarRange();
    }

    void ToggleAllow(bool toggle)
    {

    }
}
