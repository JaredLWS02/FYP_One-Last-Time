using UnityEngine;

public class State_Enemy_Control_AI_Seeking : BaseState
{
    public override string Name => "AI Seeking";

    EnemyAI enemy;

    public State_Enemy_Control_AI_Seeking(StateMachine_Enemy_Control sm)
    {
        enemy = sm.enemy;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{enemy.gameObject.name} SubState: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        enemy.SeekEnemy();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
