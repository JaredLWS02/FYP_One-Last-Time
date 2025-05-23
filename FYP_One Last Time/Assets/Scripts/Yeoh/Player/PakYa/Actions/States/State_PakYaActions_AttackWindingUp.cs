using UnityEngine;

public class State_PakYaActions_AttackWindingUp : BaseState
{
    public override string stateName => "Winding Up Attack";

    PakYaActions action;

    public State_PakYaActions_AttackWindingUp(StateMachine_PakYaActions sm)
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
        action.AllowFlip = true;
        action.AllowWallCling = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        action.AllowDash = toggle;
        action.AllowParry = toggle;
        action.AllowHurt = toggle;
        action.AllowStun = toggle;
    }
}
