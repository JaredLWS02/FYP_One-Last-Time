using UnityEngine;

public class State_Anaya_Control_AI_Fleeing : BaseState
{
    public override string Name => "AI Fleeing";

    Anaya anaya;

    public State_Anaya_Control_AI_Fleeing(StateMachine_Anaya_Control sm)
    {
        anaya = sm.anaya;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{anaya.gameObject.name} SubState: {Name}");

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
