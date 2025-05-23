using UnityEngine;

public class State_PakYa_Grounded : BaseState
{
    public override string Name => "Grounded";

    PakYa pakya;

    public State_PakYa_Grounded(StateMachine_PakYa sm)
    {
        pakya = sm.pakya;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{pakya.gameObject.name} State: {Name}");

        ToggleAllow(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        pakya.AllowMoveX = true;
        pakya.AllowMoveY = true;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        pakya.AllowJump = toggle;
        pakya.AllowDash = toggle;
        pakya.AllowAttack = toggle;
        pakya.AllowCast = toggle;
    }
}
