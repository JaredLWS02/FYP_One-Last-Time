using UnityEngine;

public class State_Action_AttackReleasing : BaseState
{
    public override string Name => "Attack Releasing";

    ActionManager action;

    public State_Action_AttackReleasing(StateMachine_Action sm)
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
        action.AllowMoveX = false;
        action.AllowMoveY = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
