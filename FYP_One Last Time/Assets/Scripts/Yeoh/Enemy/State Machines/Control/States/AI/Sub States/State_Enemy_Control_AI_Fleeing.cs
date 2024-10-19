using UnityEngine;

public class State_Enemy_Control_AI_Fleeing : BaseState
{
    public override string Name => "AI Fleeing";

    EnemyAI ai;

    public State_Enemy_Control_AI_Fleeing(StateMachine_Enemy_Control sm)
    {
        ai = sm.ai;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{ai.gameObject.name} SubState: {Name}");

        ToggleAllow(true);

        ai.SetThreatEnemy();
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
