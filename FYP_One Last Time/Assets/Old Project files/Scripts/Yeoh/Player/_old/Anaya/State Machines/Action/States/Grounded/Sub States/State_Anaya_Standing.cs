using UnityEngine;

public class State_Anaya_Standing : BaseState
{
    public override string Name => "Standing";

    Anaya anaya;

    public State_Anaya_Standing(StateMachine_Anaya sm)
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
        anaya.AllowMoveX = true;
        anaya.AllowMoveY = true;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        anaya.AllowJump = toggle;
        anaya.AllowSwitch = toggle;
        anaya.AllowDash = toggle;
        anaya.AllowClimb = toggle;
        anaya.AllowCrawl = toggle;
        anaya.AllowCommand = toggle;
    }
}
