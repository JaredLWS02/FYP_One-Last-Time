using UnityEngine;

public class State_Wolf_Control_Player : BaseState
{
    public override string Name => "Player Controls";

    Wolf wolf;

    public State_Wolf_Control_Player(StateMachine_Wolf_Control sm)
    {
        wolf = sm.wolf;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{wolf.gameObject.name} State: {Name}");

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
