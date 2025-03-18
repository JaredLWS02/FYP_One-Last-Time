using UnityEngine;

public class State_EnemyActions_AttackWindingUp : BaseState
{
    public override string stateName => "Attack Winding Up";

    EnemyActions action;

    public State_EnemyActions_AttackWindingUp(StateMachine_EnemyActions sm)
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
        //action.AllowJump = toggle;
        action.AllowHurt = toggle;
        //action.AllowStun = toggle;
    }
}
