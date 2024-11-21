using UnityEngine;

public class State_EnemyAgent_Pilot_AI_Seek : BaseState
{
    public override string Name => "AI Seek";

    EnemyAgent agent;

    public State_EnemyAgent_Pilot_AI_Seek(StateMachine_EnemyAgent_Pilot sm)
    {
        agent = sm.agent;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{agent.owner.name} SubState: {Name}");

        ToggleAllow(true);

        agent.targeting.ExpandRadar();

        agent.flip?.SetFlipDelay(agent.seekFlipDelay);
    }

    protected override void OnUpdate(float deltaTime)
    {
        string behaviour = agent.randomSeekBehaviour.currentOption;

        switch(behaviour)
        {
            case "Seek": Seek(); break;
            case "Wander": Wander(); break;
            case "Flee": Flee(); break;
            default: Seek(); break;
        }

        agent.targeting.FaceTarget();
    }

    void Seek()
    {
        if(agent.targeting.IsTargetTooClose())
        {
            agent.targeting.SetThreatToTarget();
        }
        else
        {
            agent.targeting.SetGoalToTarget();
        }
    }

    void Wander()
    {
        agent.wander.SetGoalToWander();
    }

    void Flee()
    {
        agent.targeting.SetThreatToTarget();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);

        agent.targeting.RevertRadar();

        agent.flip?.RevertFlipDelay();
    }

    void ToggleAllow(bool toggle)
    {

    }
}
