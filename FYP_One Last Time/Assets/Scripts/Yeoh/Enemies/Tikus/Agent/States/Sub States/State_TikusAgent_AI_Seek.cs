using UnityEngine;

public class State_TikusAgent_AI_Seek : BaseState
{
    public override string stateName => "AI Seek";

    TikusAgent agent;

    public State_TikusAgent_AI_Seek(StateMachine_TikusAgent sm)
    {
        agent = sm.agent;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{agent.owner.name} SubState: {stateName}");

        ToggleAllow(true);

        agent.targeting.ExpandRadar();

        agent.flip?.SetFlipDelay(agent.seekFlipDelay);

        agent.RegisterEnemyCombat();
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

        agent.UnregisterEnemyCombat();
    }

    void ToggleAllow(bool toggle)
    {

    }
}
