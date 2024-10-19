using UnityEngine;

public class State_Enemy_AutoJumping : BaseState
{
    public override string Name => "AutoJumping";

    EnemyAI ai;

    public State_Enemy_AutoJumping(StateMachine_Enemy sm)
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
        ai.AllowMoveX = false;
        ai.AllowMoveY = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
