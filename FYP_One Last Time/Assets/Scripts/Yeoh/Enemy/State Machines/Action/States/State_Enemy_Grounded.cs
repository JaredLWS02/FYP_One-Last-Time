using UnityEngine;

public class State_Enemy_Grounded : BaseState
{
    public override string Name => "Grounded";

    EnemyAI ai;

    public State_Enemy_Grounded(StateMachine_Enemy sm)
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
        ai.AllowMoveX = true;
        ai.AllowMoveY = true;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        ai.AllowJump = toggle;
        ai.AllowAutoJump = toggle;
    }
}
