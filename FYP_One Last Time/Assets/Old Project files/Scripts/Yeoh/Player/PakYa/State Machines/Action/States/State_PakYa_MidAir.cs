using UnityEngine;

public class State_PakYa_MidAir : BaseState
{
    public override string Name => "MidAir";

    PakYa pakya;

    public State_PakYa_MidAir(StateMachine_PakYa sm)
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
    }
}
