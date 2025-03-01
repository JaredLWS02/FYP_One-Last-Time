using UnityEngine;

public class State_SpyviActions_AttackReleasing : BaseState
{
    public override string stateName => "Attack Releasing";

    EnemyActions action;

    public State_SpyviActions_AttackReleasing(StateMachine_SpyviActions sm)
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
        action.AllowHurt = toggle;
        action.AllowStun = toggle;
    }
}
