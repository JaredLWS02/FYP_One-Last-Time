using UnityEngine;

public class State_AnayaActions_Idle : BaseState
{
    public override string stateName => "Idle";

    AnayaActions action;

    public State_AnayaActions_Idle(StateMachine_AnayaActions sm)
    {
        action = sm.action;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{action.owner.name} State: {stateName}");

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
        action.AllowAbility = toggle;
    }
}
