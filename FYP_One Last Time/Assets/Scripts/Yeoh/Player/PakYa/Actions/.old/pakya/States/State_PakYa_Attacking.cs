using UnityEngine;

public class State_PakYa_Attacking : BaseState
{
    public override string Name => "Attacking";

    PakYa pakya;

    public State_PakYa_Attacking(StateMachine_PakYa sm)
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
        pakya.AllowMoveX = false;
        pakya.AllowMoveY = false;
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {
        pakya.AllowAttack = toggle;
    }
}
