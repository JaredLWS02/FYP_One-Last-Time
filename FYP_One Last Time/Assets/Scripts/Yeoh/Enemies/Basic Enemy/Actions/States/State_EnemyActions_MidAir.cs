using UnityEngine;

public class State_EnemyActions_MidAir : BaseState
{
    public override string Name => "MidAir";

    EnemyActions action;

    public State_EnemyActions_MidAir(StateMachine_EnemyActions sm)
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
        action.AllowHurt = toggle;
        action.AllowStun = toggle;
    }
}
