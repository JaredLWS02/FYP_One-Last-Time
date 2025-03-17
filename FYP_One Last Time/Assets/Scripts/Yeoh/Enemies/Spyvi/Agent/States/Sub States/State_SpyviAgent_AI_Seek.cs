using UnityEngine;

public class State_SpyviAgent_AI_Seek : BaseState
{
    public override string stateName => "AI Seek";

    SpyviAgent agent;

    public State_SpyviAgent_AI_Seek(StateMachine_SpyviAgent sm)
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

        agent.events.OnSeekEnter?.Invoke();
    }

    protected override void OnUpdate(float deltaTime)
    {
        agent.targeting.FaceTarget();

        CheckBehaviour();
    }
    
    protected override void OnExit()
    {
        ToggleAllow(false);

        agent.targeting.RevertRadar();

        agent.flip?.RevertFlipDelay();

        agent.UnregisterEnemyCombat();

        agent.events.OnSeekExit?.Invoke();
    }

    void ToggleAllow(bool toggle)
    {
        
    }

    // ============================================================================

    void CheckBehaviour()
    {
        string behaviour = "Seek";

        //RandomPicker random_picker = agent.randomSeekBehaviour;
        RandomPicker random_picker = null;
        if(random_picker) behaviour = random_picker.currentOption;

        switch(behaviour)
        {
            case "Seek": Seek(); break;
            case "Wander": Wander(); break;
            case "Flee": Flee(); break;
            default: Seek(); break;
        }
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
}
