using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BufferedInput
{
    public string inputName;
    public float bufferLeft;

    // ctor
    public BufferedInput(string input_name, float buffer_time)
    {
        inputName = input_name;
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
    
    public List<BufferedInput> inputs = new();

    void OnAddBuffer(GameObject who, string input_name, float buffer_time)
    {
        if(who!=owner) return;

        if(HasAction(input_name, out var input))
        {
            input.DoBuffer(buffer_time);
        }        
        else
        {
            inputs.Add(new BufferedInput(input_name, buffer_time));
        }
    }

    bool HasAction(string input_name, out BufferedInput out_input)
    {
        foreach(var action in inputs)
        {
            if(action.inputName == input_name)
            {
                out_input = action;
                return true;
            }
        }
        out_input = null;
        return false;
    }
    
    // ============================================================================

    List<BufferedInput> inputsToRemove = new();

    void Update()
    {
        foreach(var input in inputs)
        {
            EventM.OnInputBuffering(owner, input.inputName);
            
            input.UpdateBuffer();

            // if no buffer left
            if(!input.HasBuffer())
            {
                // temp list to mark actions for removal
                inputsToRemove.Add(input);
            }
        }

        // now remove from actual list
        foreach(var input in inputsToRemove)
        {
            inputs.Remove(input);
        }
        inputsToRemove.Clear();
    }

    void OnRemoveBuffer(GameObject who, string input_name)
    {
        if(who!=owner) return;

        if(HasAction(input_name, out var input))
        {
            // temp list to mark actions for removal
            // ignore if already marked
            if(inputsToRemove.Contains(input)) return;

            inputsToRemove.Add(input);
        }
    }
}
