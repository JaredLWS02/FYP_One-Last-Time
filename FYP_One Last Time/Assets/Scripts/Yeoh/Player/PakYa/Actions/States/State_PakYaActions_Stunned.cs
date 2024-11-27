using UnityEngine;

public class State_PakYaActions_Stunned : BaseState
{
    public override string stateName => "Stunned";

    PakYaActions action;

    public State_PakYaActions_Stunned(StateMachine_PakYaActions sm)
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
        action.AllowParry = toggle;
        action.AllowHurt = toggle;
        //action.AllowStun = toggle;
    }
}
