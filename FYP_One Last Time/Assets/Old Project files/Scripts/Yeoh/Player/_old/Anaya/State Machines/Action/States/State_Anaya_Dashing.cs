using UnityEngine;

public class State_Anaya_Dashing : BaseState
{
    public override string Name => "Dashing";

    Anaya anaya;

    public State_Anaya_Dashing(StateMachine_Anaya sm)
    {
        anaya = sm.anaya;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{anaya.gameObject.name} State: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        anaya.AllowMoveX = false;
        anaya.AllowMoveY = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        anaya.AllowSwitch = toggle;
        anaya.AllowClimb = toggle;
    }
}
