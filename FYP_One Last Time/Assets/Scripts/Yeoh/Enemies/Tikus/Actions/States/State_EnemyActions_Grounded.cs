using UnityEngine;

public class State_EnemyActions_Grounded : BaseState
{
    public override string stateName => "Grounded";

    EnemyActions action;

    public State_EnemyActions_Grounded(StateMachine_EnemyActions sm)
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
        action.AllowAutoJump = toggle;
        action.AllowAttack = toggle;
        action.AllowHurt = toggle;
        action.AllowStun = toggle;
    }
}
