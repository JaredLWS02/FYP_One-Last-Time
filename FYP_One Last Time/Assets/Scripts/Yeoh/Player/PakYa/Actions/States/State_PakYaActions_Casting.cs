using UnityEngine;

public class State_PakYaActions_Casting : BaseState
{
    public override string Name => "Casting";

    PakYaActions action;

    public State_PakYaActions_Casting(StateMachine_PakYaActions sm)
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
        action.AllowMoveX = false;
        action.AllowMoveY = false;
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
