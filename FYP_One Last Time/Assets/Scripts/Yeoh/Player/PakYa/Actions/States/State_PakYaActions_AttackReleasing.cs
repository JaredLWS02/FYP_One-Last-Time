using UnityEngine;

public class State_PakYaActions_AttackReleasing : BaseState
{
    public override string stateName => "Attack Releasing";

    PakYaActions action;

    public State_PakYaActions_AttackReleasing(StateMachine_PakYaActions sm)
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
        action.AllowFlip = false;
        action.AllowWallCling = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        action.AllowHurt = toggle;
    }
}
