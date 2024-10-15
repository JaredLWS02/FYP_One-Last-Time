using UnityEngine;

public class State_Enemy_MidAir : BaseState
{
    public override string Name => "MidAir";

    EnemyAI enemy;

    public State_Enemy_MidAir(StateMachine_Enemy sm)
    {
        enemy = sm.enemy;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{enemy.gameObject.name} State: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        enemy.AllowMoveX = true;
        enemy.AllowMoveY = true;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        enemy.AllowJump = toggle;
    }
}
