using UnityEngine;

public class State_Enemy_Control_AI : BaseState
{
    public override string Name => "AI Control";

    EnemyAI ai;

    // SUB STATE MACHINE ================================================================================

    BaseState defaultSubState;

    public State_Enemy_Control_AI(StateMachine_Enemy_Control sm)
    {
        ai = sm.ai;

        subsm = new StateMachine();
        
        // SUB STATES ================================================================================

        State_Enemy_Control_AI_Idle idle = new(sm);
        State_Enemy_Control_AI_Attacking attacking = new(sm);
        State_Enemy_Control_AI_Fleeing fleeing = new(sm);

        // HUB TRANSITIONS ================================================================================

        idle.AddTransition(attacking, (timeInState) =>
        {
            if(
                ai.GetEnemy() &&
                ai.IsHealthy()
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(fleeing, (timeInState) =>
        {
            if(
                ai.GetEnemy() &&
                !ai.IsHealthy()
            ){
                return true;
            }
            return false;
        });

        
        // RETURN TRANSITIONS ================================================================================

        attacking.AddTransition(idle, (timeInState) =>
        {
            if(
                !ai.GetEnemy() ||
                !ai.IsHealthy()
            ){
                return true;
            }
            return false;
        });

        fleeing.AddTransition(idle, (timeInState) =>
        {
            if(
                !ai.GetEnemy() ||
                ai.IsHealthy()
            ){
                return true;
            }
            return false;
        });

        
        // DEFAULT ================================================================================
        
        defaultSubState = idle;
        subsm.SetInitialState(defaultSubState);
    }

    protected override void OnEnter()
    {
        Debug.Log($"{ai.gameObject.name} State: {Name}");

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
