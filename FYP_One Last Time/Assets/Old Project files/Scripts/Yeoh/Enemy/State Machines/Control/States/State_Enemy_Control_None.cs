using UnityEngine;

public class State_Enemy_Control_None : BaseState
{
    public override string Name => "No Control";

    EnemyAI enemy;

    public State_Enemy_Control_None(StateMachine_Enemy_Control sm)
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
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
