using UnityEngine;

public class State_SpyviActions_MidAir : BaseState
{
    public override string Name => "MidAir";

    SpyviActions action;

    public State_SpyviActions_MidAir(StateMachine_SpyviActions sm)
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
        action.AllowHurt = toggle;
        action.AllowStun = toggle;
    }
}
