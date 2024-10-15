using UnityEngine;

public class State_Enemy_Control_AI : BaseState
{
    public override string Name => "AI Control";

    EnemyAI enemy;

    // SUB STATE MACHINE ================================================================================

    BaseState defaultSubState;

    public State_Enemy_Control_AI(StateMachine_Enemy_Control sm)
    {
        enemy = sm.enemy;

        subsm = new StateMachine();
        
        // SUB STATES ================================================================================

        State_Enemy_Control_AI_Idle idle = new(sm);
        State_Enemy_Control_AI_Seeking seeking = new(sm);
        State_Enemy_Control_AI_Attacking attacking = new(sm);

        // HUB TRANSITIONS ================================================================================

        idle.AddTransition(seeking, (timeInState) =>
        {
            if(
                enemy.GetEnemy() &&
                !enemy.IsInAttackRange()
            ){
                return true;
            }
            return false;
        });

        idle.AddTransition(attacking, (timeInState) =>
        {
            if(
                enemy.GetEnemy() &&
                enemy.IsInAttackRange()
            ){
                return true;
            }
            return false;
        });
        


        // RETURN TRANSITIONS ================================================================================

        seeking.AddTransition(idle, (timeInState) =>
        {
            if(
                !enemy.GetEnemy() ||
                enemy.IsInAttackRange()
            ){
                return true;
            }
            return false;
        });

        attacking.AddTransition(idle, (timeInState) =>
        {
            if(
                !enemy.GetEnemy() ||
                !enemy.IsInAttackRange()
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
        Debug.Log($"{enemy.gameObject.name} State: {Name}");

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
