using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPicker : RandomUpdate
{
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // ============================================================================
    
    [System.Serializable]
    public class Option
    {
        public string name;
        public float weight=1;
    }

    [Header("Random Picker")]
    public List<Option> options = new();

    void Reset() => options = new() { new Option() };

    public string currentOption;

    // ============================================================================
    
    protected override void OnRandomUpdate()
    {
        RandomizeOption();
    }

    [ContextMenu("Randomize")]
    void RandomizeOption()
    {
        if(options.Count<=0) return;

        if(options.Count<=1)
        {
            currentOption = options[0].name;
            return;
        }

        float total_weight=0;

        foreach(var option in options)
        {
            total_weight += option.weight;
        }

        float random_value = Random.Range(0f, total_weight);
        
        float cumulative_weight=0;

        foreach(var option in options)
        {
            cumulative_weight += option.weight;

            if(random_value <= cumulative_weight)
            {
                currentOption = option.name; // Set the current option
                return; // Exit once the option is found
            }
        }
    }
}
