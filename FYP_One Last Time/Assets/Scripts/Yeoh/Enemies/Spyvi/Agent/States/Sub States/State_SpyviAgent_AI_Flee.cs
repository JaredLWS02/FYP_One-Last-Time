using UnityEngine;

public class State_SpyviAgent_AI_Flee : BaseState
{
    public override string stateName => "AI Flee";

    SpyviAgent agent;

    public State_SpyviAgent_AI_Flee(StateMachine_SpyviAgent sm)
    {
        agent = sm.agent;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{agent.owner.name} SubState: {stateName}");

        ToggleAllow(true);

        agent.targeting.ExpandRadar();
    }

    protected override void OnUpdate(float deltaTime)
    {
        CheckBehaviour();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);

        agent.targeting.RevertRadar();
    }

    void ToggleAllow(bool toggle)
    {

    }

    // ============================================================================

    void CheckBehaviour()
    {
        string behaviour = "Flee";

        //RandomPicker random_picker = agent.randomFleeBehaviour;
        RandomPicker random_picker = null;
        if(random_picker) behaviour = random_picker.currentOption;

        switch(behaviour)
        {
            case "Flee": Flee(); break;
            case "Wander": Wander(); break;
            default: Flee(); break;
        }
    }

    void Wander()
    {
        agent.wander.SetGoalToWander();
        agent.targeting.FaceTarget();
    }

    void Flee()
    {
        agent.targeting.SetThreatToTarget();
        agent.move.FaceMoveDir();
    }
}
