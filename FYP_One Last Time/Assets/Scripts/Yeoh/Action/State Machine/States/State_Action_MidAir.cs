using UnityEngine;

public class State_Action_MidAir : BaseState
{
    public override string Name => "MidAir";

    ActionManager action;

    public State_Action_MidAir(StateMachine_Action sm)
    {
        action = sm.action;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{action.gameObject.name} State: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        action.AllowMoveX = true;
        action.AllowMoveY = true;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        action.AllowJump = toggle;
        action.AllowDash = toggle;
    }
}
