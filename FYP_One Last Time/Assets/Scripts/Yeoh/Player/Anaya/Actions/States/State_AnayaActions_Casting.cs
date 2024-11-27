using UnityEngine;

public class State_AnayaActions_Casting : BaseState
{
    public override string stateName => "Casting";

    AnayaActions action;

    public State_AnayaActions_Casting(StateMachine_AnayaActions sm)
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

    }
}
