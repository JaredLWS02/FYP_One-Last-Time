using UnityEngine;

public class State_PakYa_Control_Player : BaseState
{
    public override string Name => "Player Controls";

    PakYa pakya;

    public State_PakYa_Control_Player(StateMachine_PakYa_Control sm)
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
    }

    protected override void OnExit()
    {
        ToggleAllow(false);
    }

    void ToggleAllow(bool toggle)
    {

    }
}
