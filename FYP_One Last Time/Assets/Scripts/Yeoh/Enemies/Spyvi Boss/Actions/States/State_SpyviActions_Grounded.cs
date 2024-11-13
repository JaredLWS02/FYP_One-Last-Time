using UnityEngine;

public class State_SpyviActions_Grounded : BaseState
{
    public override string Name => "Grounded";

    SpyviActions action;

    public State_SpyviActions_Grounded(StateMachine_SpyviActions sm)
    {
        action = sm.action;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{action.owner.name} State: {Name}");

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
        action.AllowAutoJump = toggle;
        action.AllowHurt = toggle;
        action.AllowStun = toggle;
    }
}
