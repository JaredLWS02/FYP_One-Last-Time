using UnityEngine;

public class State_Enemy_Control_AI_Returning : BaseState
{
    public override string Name => "AI Returning";

    EnemyAI ai;

    public State_Enemy_Control_AI_Returning(StateMachine_Enemy_Control sm)
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
        ai.SetGoalSpawnpoint();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
