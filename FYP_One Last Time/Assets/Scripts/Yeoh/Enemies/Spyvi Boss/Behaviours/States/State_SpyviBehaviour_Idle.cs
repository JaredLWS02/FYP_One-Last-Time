using UnityEngine;

public class State_SpyviBehaviour_Idle : BaseState
{
    public override string stateName => "Behaviour Idle";

    SpyviActions spyvi;

    public State_SpyviBehaviour_Idle(StateMachine_SpyviBehaviour sm)
    {
        spyvi = sm.spyvi;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{spyvi.owner.name} State: {stateName}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        
    }
}
