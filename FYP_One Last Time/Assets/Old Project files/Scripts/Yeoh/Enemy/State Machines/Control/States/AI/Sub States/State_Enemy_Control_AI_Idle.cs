using UnityEngine;

public class State_Enemy_Control_AI_Idle : BaseState
{
    public override string Name => "AI Idle";

    EnemyAI enemy;

    public State_Enemy_Control_AI_Idle(StateMachine_Enemy_Control sm)
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
        enemy.SeekWander();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
