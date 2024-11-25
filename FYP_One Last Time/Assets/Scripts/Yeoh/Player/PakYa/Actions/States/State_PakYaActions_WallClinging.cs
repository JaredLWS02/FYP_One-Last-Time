using UnityEngine;

public class State_PakYaActions_WallClinging : BaseState
{
    public override string Name => "Wall Clinging";

    PakYaActions action;

    public State_PakYaActions_WallClinging(StateMachine_PakYaActions sm)
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
        action.AllowFlip = true;
        action.AllowWallCling = true;
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
