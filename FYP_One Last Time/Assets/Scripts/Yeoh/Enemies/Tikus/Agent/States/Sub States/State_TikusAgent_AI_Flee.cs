using UnityEngine;

public class State_TikusAgent_AI_Flee : BaseState
{
    public override string stateName => "AI Flee";

    TikusAgent agent;

    public State_TikusAgent_AI_Flee(StateMachine_TikusAgent sm)
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
        string behaviour = agent.randomFleeBehaviour.currentOption;

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

    protected override void OnExit()
    {
        ToggleAllow(false);

        agent.targeting.RevertRadar();
    }

    void ToggleAllow(bool toggle)
    {

    }
}
