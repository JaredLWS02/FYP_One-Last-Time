using UnityEngine;

public class State_Enemy_Control_AI_Idle : BaseState
{
    public override string Name => "AI Idle";

    EnemyAI ai;

    public State_Enemy_Control_AI_Idle(StateMachine_Enemy_Control sm)
    {
        ai = sm.ai;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{ai.gameObject.name} SubState: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        ai.SetGoalWander();

        ai.FaceMoveDir();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
