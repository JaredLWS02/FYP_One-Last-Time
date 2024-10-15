using UnityEngine;

// This state machine approach ensures the initial state needs to be set first
// and calls the Enter method right after.
public class StateMachine
{
    public BaseState currentState;

    private bool initialized = false;

    public void SetInitialState(BaseState s)
    {
        currentState = s;
        currentState.Enter();
        initialized = true;
    }

    public void SetState(BaseState s)
    {
        currentState = s;
        currentState.Enter();
    }

    public void Tick(float deltaTime)
    {
        if(!initialized)
        {
            Debug.LogWarning("Call SetInitialState first!");
            return;
        }

        currentState.Update(deltaTime);

        if(currentState.TryGetNextTransition(out BaseState next))
        {
            currentState.Exit();
            currentState = next;
            currentState.Enter();
        }
    }
}

// This is like the trigger you see in the Animator for transitions
public class Trigger
{
    bool triggered;

    public void Enable()
    {
        triggered=true;
    }
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