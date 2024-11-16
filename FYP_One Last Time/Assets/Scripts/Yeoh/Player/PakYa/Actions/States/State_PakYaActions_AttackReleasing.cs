using UnityEngine;

public class State_PakYaActions_AttackReleasing : BaseState
{
    public override string Name => "Attack Releasing";

    PakYaActions action;

    public State_PakYaActions_AttackReleasing(StateMachine_PakYaActions sm)
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
        action.AllowMoveX = true; //false;
        action.AllowMoveY = true; //false;
        action.AllowFlip = false;
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
