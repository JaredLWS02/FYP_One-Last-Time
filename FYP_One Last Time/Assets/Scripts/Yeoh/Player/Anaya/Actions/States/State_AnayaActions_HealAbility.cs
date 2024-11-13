using UnityEngine;

public class State_AnayaActions_HealAbility : BaseState
{
    public override string Name => "Heal Ability";

    AnayaActions action;

    public State_AnayaActions_HealAbility(StateMachine_AnayaActions sm)
    {
        action = sm.action;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{action.owner.name} State: {Name}");

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
