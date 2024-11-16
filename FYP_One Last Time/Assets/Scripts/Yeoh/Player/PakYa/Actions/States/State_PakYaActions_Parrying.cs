using UnityEngine;

public class State_PakYaActions_Parrying : BaseState
{
    public override string Name => "Parrying";

    PakYaActions action;

    public State_PakYaActions_Parrying(StateMachine_PakYaActions sm)
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
        action.AllowFlip = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        action.AllowDash = toggle;
        action.AllowAttack = toggle;
        action.AllowParry = toggle;
    }
}
