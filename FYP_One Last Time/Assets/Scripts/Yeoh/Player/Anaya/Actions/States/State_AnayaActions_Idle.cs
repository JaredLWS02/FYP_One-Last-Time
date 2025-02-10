using System.Collections;
using UnityEngine;

public class State_AnayaActions_Idle : BaseState
{
    public override string stateName => "Idle";

    AnayaActions action;

    public State_AnayaActions_Idle(StateMachine_AnayaActions sm)
    {
        action = sm.action;
    }

    protected override void OnEnter()
    {
        Debug.Log($"{action.owner.name} State: {stateName}");

        aaa_crt = action.StartCoroutine(aaaaaa());
    }

    protected override void OnUpdate(float deltaTime)
    {
        // because a 1 frame gap where this will be true
        //if(timeInState>0.1f)
        //ToggleAllow(true);
    }

    Coroutine aaa_crt;

    IEnumerator aaaaaa()
    {
        yield return new WaitForSeconds(.1f);
        ToggleAllow(true);
    }

    protected override void OnExit()
    {
        ToggleAllow(false);

        action.StopCoroutine(aaa_crt);
    }

    // ================================================================================

    void ToggleAllow(bool toggle)
    {
        action.AllowAbility = toggle;
    }
}
