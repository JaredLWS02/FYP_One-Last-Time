using UnityEngine;

public class State_Wolf_Control_AI_Staying : BaseState
{
    public override string Name => "AI Staying";

    Wolf wolf;

    public State_Wolf_Control_AI_Staying(StateMachine_Wolf_Control sm)
    {
        wolf = sm.wolf;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{wolf.gameObject.name} SubState: {Name}");

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
