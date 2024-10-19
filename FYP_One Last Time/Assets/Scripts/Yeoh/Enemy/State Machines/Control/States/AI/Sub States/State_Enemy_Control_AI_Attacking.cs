using UnityEngine;

public class State_Enemy_Control_AI_Attacking : BaseState
{
    public override string Name => "AI Attacking";

    EnemyAI ai;

    public State_Enemy_Control_AI_Attacking(StateMachine_Enemy_Control sm)
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
        if(ai.IsEnemyTooClose())
        {
            ai.SetThreatEnemy();
        }
        else
        {
            ai.SetGoalEnemy();
        }

        ai.FaceEnemy();
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
