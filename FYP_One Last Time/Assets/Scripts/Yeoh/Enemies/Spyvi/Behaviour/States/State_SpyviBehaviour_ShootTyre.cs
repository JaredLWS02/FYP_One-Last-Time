using UnityEngine;

public class State_SpyviBehaviour_ShootTyre : BaseState
{
    public override string stateName => "Behaviour Shoot Tyre";

    SpyviBehaviour behaviour;

    public State_SpyviBehaviour_ShootTyre(StateMachine_SpyviBehaviour sm)
    {
        behaviour = sm.behaviour;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{behaviour.owner.name} State: {stateName}");

        Toggle(true);
    }

    protected override void OnUpdate(float deltaTime)
    {
        
    }

    protected override void OnExit()
    {
        Toggle(false);
    }

    void Toggle(bool toggle)
    {
        behaviour.rushTrigger.SetActive(!toggle);
        behaviour.laserTrigger.SetActive(!toggle);
        behaviour.shootTyreTrigger.SetActive(toggle);
        behaviour.revUpTrigger.SetActive(!toggle);
    }
}
