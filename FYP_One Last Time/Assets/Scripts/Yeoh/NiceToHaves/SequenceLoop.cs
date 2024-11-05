using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SequenceLoop
{
    public List<string> options = new();
    public int index=0;

    // ============================================================================
    
    public string CurrentOption() => options[index];

    public void Next()
    {
        index++;
        if(index >= options.Count)
        Reset();
    }

    public void Reset() => index=0;
}
