using UnityEngine;

public class State_SpyviBehaviour_Rush : BaseState
{
    public override string stateName => "Behaviour Rush";

    SpyviBehaviour behaviour;

    public State_SpyviBehaviour_Rush(StateMachine_SpyviBehaviour sm)
    {
        behaviour = sm.behaviour;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{behaviour.owner.name} State: {stateName}");

        Toggle(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        
    }

    protected override void OnExit()
    {
        Toggle(false);
    }

    void Toggle(bool toggle)
    {
        behaviour.ToggleRushTrigger(toggle);
    }
}
