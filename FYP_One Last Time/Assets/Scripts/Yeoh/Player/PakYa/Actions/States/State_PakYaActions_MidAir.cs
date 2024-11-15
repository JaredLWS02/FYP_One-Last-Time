using UnityEngine;

public class State_PakYaActions_MidAir : BaseState
{
    public override string Name => "MidAir";

    PakYaActions action;

    public State_PakYaActions_MidAir(StateMachine_PakYaActions sm)
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
        action.AllowDash = toggle;
        action.AllowAttack = toggle;
        action.AllowParry = toggle;
        action.AllowHurt = toggle;
        action.AllowStun = toggle;
    }
}
