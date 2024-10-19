using UnityEngine;

public class State_Enemy_Control_None : BaseState
{
    public override string Name => "No Control";

    EnemyAI ai;

    public State_Enemy_Control_None(StateMachine_Enemy_Control sm)
    {
        ai = sm.ai;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{ai.gameObject.name} State: {Name}");

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
