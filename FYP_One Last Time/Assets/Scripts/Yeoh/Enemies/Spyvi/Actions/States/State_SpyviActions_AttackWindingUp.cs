using UnityEngine;

public class State_SpyviActions_AttackWindingUp : BaseState
{
    public override string stateName => "Attack Winding Up";

    SpyviActions action;

    public State_SpyviActions_AttackWindingUp(StateMachine_SpyviActions sm)
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
        action.AllowMoveX = false;
        action.AllowMoveY = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        //action.AllowJump = toggle;
        //action.AllowParry = toggle;
        action.AllowHurt = toggle;
        //action.AllowStun = toggle;
    }
}
