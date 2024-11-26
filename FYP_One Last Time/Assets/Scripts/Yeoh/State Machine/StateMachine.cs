using UnityEngine;

// This state machine approach ensures the initial state needs to be set first
// and calls the Enter method right after.
public class StateMachine
{
    public BaseState currentState;

    bool initialized = false;

    public void SetInitialState(BaseState state)
    {
        currentState = state;
        currentState.Enter();
        initialized = true;
    }

    public void SetState(BaseState state)
    {
        currentState = state;
        currentState.Enter();
    }

    // ============================================================================

    public void Tick(float deltaTime)
    {
        if(!initialized)
        {
            Debug.LogWarning("Call SetInitialState first!");
            return;
        }

        currentState.Update(deltaTime);

        if(currentState.IsMetastate())
        {
            if(currentState.subsm.currentState.TryGetNextTransition(out BaseState next_substate))
            {
                currentState.subsm.currentState.Exit(next_substate);
                currentState.subsm.SetState(next_substate);
                return;
            }
        }

        // else exit and change current state
        if(currentState.TryGetNextTransition(out BaseState next))
        {
            currentState.Exit();
            SetState(next);
        }
    }
}

// ============================================================================

// This is like the trigger you see in the Animator for transitions
public class Trigger
{
    bool triggered;

    public void Enable() => triggered=true;

    public bool Check()
    {
        if(triggered)
        {
            triggered=false;
            return true;
        }
        return false;
    }
}