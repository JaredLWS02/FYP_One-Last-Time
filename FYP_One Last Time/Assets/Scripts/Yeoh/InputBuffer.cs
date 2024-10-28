using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BufferedAction
{
    public string actionName;
    public float bufferLeft;

    // ctor
    public BufferedAction(string action_name, float buffer_time)
    {
        actionName = action_name;
        DoBuffer(buffer_time);
    }

    public void DoBuffer(float buffer_time)
    {
        bufferLeft = buffer_time;
    }

    public void UpdateBuffer()
    {
        bufferLeft -= Time.deltaTime;
    }

    public bool HasBuffer() => bufferLeft>0;
}

// ============================================================================

public class InputBuffer : MonoBehaviour
{
    public GameObject owner;

    // ============================================================================
    
    EventManager EventM;

    void OnEnable()
    {
        EventM = EventManager.Current;

        EventM.AddInputBufferEvent += OnAddBuffer;
        EventM.RemoveInputBufferEvent += OnRemoveBuffer;
    }
    void OnDisable()
    {
        EventM.AddInputBufferEvent -= OnAddBuffer;
        EventM.RemoveInputBufferEvent -= OnRemoveBuffer;
    }
    
    // ============================================================================
    
    public List<BufferedAction> actions = new();

    void OnAddBuffer(GameObject who, string action_name, float buffer_time)
    {
        if(who!=owner) return;

        if(HasAction(action_name, out var action))
        {
            action.DoBuffer(buffer_time);
        }        
        else
        {
            actions.Add(new BufferedAction(action_name, buffer_time));
        }
    }

    bool HasAction(string action_name, out BufferedAction out_action)
    {
        foreach(var action in actions)
        {
            if(action.actionName == action_name)
            {
                out_action = action;
                return true;
            }
        }
        out_action = null;
        return false;
    }
    
    // ============================================================================

    List<BufferedAction> actionsToRemove = new();

    void Update()
    {
        foreach(var action in actions)
        {
            EventM.OnInputBuffering(owner, action.actionName);
            
            action.UpdateBuffer();

            // if no buffer left
            if(!action.HasBuffer())
            {
                // temp list to mark actions for removal
                actionsToRemove.Add(action);
            }
        }

        // now remove from actual list
        foreach(var action in actionsToRemove)
        {
            actions.Remove(action);
        }
        actionsToRemove.Clear();
    }

    void OnRemoveBuffer(GameObject who, string action_name)
    {
        if(who!=owner) return;

        if(HasAction(action_name, out var action))
        {
            // temp list to mark actions for removal
            // ignore if already marked
            if(actionsToRemove.Contains(action)) return;

            actionsToRemove.Add(action);
        }
    }
}
